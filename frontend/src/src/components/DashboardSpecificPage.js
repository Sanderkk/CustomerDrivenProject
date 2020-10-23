import Navbar from "./Navbar";
import "./componentStyles/DashboardSpecificPage.css";
import { Link } from "react-router-dom";
import { AzureAD, AuthenticationState } from "react-aad-msal";
import { authProvider } from "../authProvider";
import AccessDenied from "./AccessDenied";
import { setCurrentDashboard } from "../globalState/actions/dashboardActions";
import React, { useState, useEffect } from "react";
import { useDispatch } from "react-redux";
import GlobalButton from "./globalComponents/GlobalButton";
import sendMutation from "../queries/sendMutation";
import { UPDATE_DASHBOARD } from "../queries/mutations";
import { useApolloClient } from "@apollo/client";
import { BiSave, BiPlus } from "react-icons/bi";
import DashboardCellCard from "./DashboardCellCard";
import { useSelector } from "react-redux";
import groupTypes from "../groupTypes";

function DashboardSpecificPage(props) {
  /*
    Page showing specific dashboard
  */

 const emptyDashboard = {
    name: "Name this Dashboard",
    description: "Write description for this Dashboard"
  }
  const state = props.location.state;
  const dispatch = useDispatch();
  const client = useApolloClient();
  const [dashboard, setDashboard] = useState(null);
  const [cells, setCells] = useState(null);
  const user = useSelector((store) => store.user.aadResponse);


  // Cell mock data
  const cellsMockData = [
    {
      id: 1,
      input: {
        sensors: [1],
        specifiedTimePeriode: true,
        from:"2020-08-01T08:21:19.000Z",
        to:"2020-08-30T12:47:21.000Z"
      },
      options: {
        title: "Oksygen graf",
        RYAxis: "kul akse",
        LYAxis: "venstreee"
      }
    },
    {
      id: 5,
      input: {
        sensors: [3],
        specifiedTimePeriode: true,
        from:"2020-08-01T08:21:19.000Z",
        to:"2020-08-30T12:47:21.000Z"
      },
      options: {
        title: "Temp graf",
        RYAxis: "temp",
        LYAxis: "enda mere temp"
      }
    },
    {
      id: 3,
      input: {
        sensors: [1,3],
        specifiedTimePeriode: true,
        from:"2020-08-01T08:21:19.000Z",
        to:"2020-08-30T12:47:21.000Z"
      },
      options: {
        title: "Alle gode ting er 3",
        RYAxis: "3",
        LYAxis: "tre"
      }
    }
  ];

  
  // If state is null, then set the dashboard as empty, if not, set dashboard as the one given in state
  useEffect(() => {
    if(typeof state === 'undefined' || state === null){
      dispatch(setCurrentDashboard(emptyDashboard));
      setDashboard(emptyDashboard);
    }else{
      dispatch(setCurrentDashboard(state));
      setDashboard(state);
    }}, []);

  // If dashboard and user is sat in redux, fetch and set cells for given dashboard
  useEffect(() => {
    if(user !== null && dashboard !== null){
      // TODO: fetch cells and setCells()
      // const userId = user.account.accountIdentifier;
      // const userId = "123"; //Test user with data
      // sendQuery(client, GET_CELLS_FOR_DASHBOARD, { dashboardId })
      // .then((result) => {
      //   setCells(result.data.cells);
      // }).catch((err) => console.log(err));
      setCells(cellsMockData);
    }
  }, [client, user, dashboard]);


  const handleSave = () => {
    sendMutation(client, UPDATE_DASHBOARD, { dashboard })
      .then((result) => {
      }).catch((err) => console.log(err));
  }

  const handleNameChange = e => {
    e.preventDefault()
    setDashboard({...dashboard, name: e.target.value})
  }

  const handleDescriptionChange = e => {
    e.preventDefault()
    setDashboard({...dashboard, description: e.target.value})
  }


  return (
    <div>
      <Navbar />
      {/* AzureAD component because AccessCheckerDecorator doesn't work on this page */}
      <AzureAD provider={authProvider}>
        {({ authenticationState }) => {
          switch (authenticationState) {
            case AuthenticationState.Authenticated:
              return(
                <div className="container_div">
                  {dashboard === null || user === null ? 
                    <h1>Loading</h1> 
                  : 
                  <div>
                    <form className="dashboard-form">
                      {/* Only give editing tools to researchers */}
                        {user.account.idToken.groups.indexOf(groupTypes.researcher) >= 0 ?
                          <div>
                            <input className="dashboard_input" type="text" id="name" onChange={handleNameChange} value={dashboard.name} />
                            <div className="add_cell_btn">
                              <Link to="/cell">
                                <GlobalButton primary={true} btnText="Add Cell">
                                  <BiPlus />
                                </GlobalButton>
                              </Link>
                            </div>
                            <div className="save_btn">
                              <GlobalButton primary={true} btnText="Save" handleButtonClick={handleSave}>
                                <BiSave />
                              </GlobalButton>
                            </div>
                            <textarea className="dashboard_textarea" type="text" id="description" onChange={handleDescriptionChange} value={dashboard.description} />
                          </div>
                          :
                          <div>
                            <input className="dashboard_input" type="text" id="name" onChange={handleNameChange} value={dashboard.name} disabled={true}/>
                            <textarea className="dashboard_textarea" type="text" id="description" onChange={handleDescriptionChange} value={dashboard.description} disabled={true}/>
                          </div>
                        }
                    </form>

                    {cells === null || cells.length === 0 ?
                    <div>
                      <h1>You have no cells</h1>
                    </div>
                    :
                    <div className="cell_grid_container">
                      {cells.map((cell) => {
                        return (
                          <div key={cell.id} >
                            <DashboardCellCard cell={cell}/>
                          </div>
                        );
                      })}
                    </div>
                    }
                  </div>
                  }
                </div>
              )
            default:
              return <AccessDenied></AccessDenied>;
          }
        }}
      </AzureAD>
    </div>
  );
}


export default DashboardSpecificPage;
