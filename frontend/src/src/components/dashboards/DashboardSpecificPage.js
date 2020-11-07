import Navbar from "../Navbar";
import "../componentStyles/dashboards/DashboardSpecificPage.css";
import { Link } from "react-router-dom";
import { AzureAD, AuthenticationState } from "react-aad-msal";
import { authProvider } from "../../authProvider";
import AccessDenied from "../AccessDenied";
import { setCurrentDashboard } from "../../globalState/actions/dashboardActions";
import React, { useState, useEffect } from "react";
import { useDispatch } from "react-redux";
import GlobalButton from "../globalComponents/GlobalButton";
import sendMutation from "../../requests/sendMutation";
import sendQuery from "../../requests/sendQuery";
import { UPDATE_DASHBOARD, DELETE_DASHBOARD, DELETE_CELL } from "../../requests/mutations";
import { GET_DASHBOARD_CELLS, GET_DASHBOARD, GET_TIME_SERIES } from "../../requests/queries";
import { useApolloClient } from "@apollo/client";
import { BiSave, BiPlus, BiTrash } from "react-icons/bi";
import DashboardCellCard from "./DashboardCellCard";
import { useSelector } from "react-redux";
import appsettings from "../../appsettings.json";
import { setQueryData } from "../../globalState/actions/queryDataActions";

function DashboardSpecificPage(props) {
  /*
    Page showing specific dashboard
  */

 const emptyDashboard = {
    name: "Name this Dashboard",
    description: "Write description for this Dashboard"
  }
  // let state = props.location.state;
  const dispatch = useDispatch();
  const client = useApolloClient();
  const [dashboard, setDashboard] = useState(null);
  const [cells, setCells] = useState(null);
  const currentDashboard = useSelector(store => store.currentDashboard.input)
  const user = useSelector((store) => store.user.aadResponse);

  
  // If state is null, then set the dashboard as empty, if not, set dashboard as the one given in state
  useEffect(() => {
    if(user !== null){
      var toBeSetAsDashboard = {};
      if(typeof currentDashboard === 'undefined' || currentDashboard === null){
        dispatch(setCurrentDashboard(emptyDashboard));
        setDashboard(emptyDashboard);
        fetchCells(emptyDashboard);
      }else{
        // Must make new variable and not use state to remove state's _typename
        const dashboardId = currentDashboard.dashboardId;
        sendQuery(client, GET_DASHBOARD, { dashboardId})
          .then((result) => {
            toBeSetAsDashboard = {
              dashboardId: result.data.dashboard.dashboardId,
              description: result.data.dashboard.description,
              name: result.data.dashboard.name,
            };
            dispatch(setCurrentDashboard(toBeSetAsDashboard));
            setDashboard(toBeSetAsDashboard);
            fetchCells(toBeSetAsDashboard);
          }).catch((err) => console.log(err));
      }
    }}, [user]);


  function fetchCells(dashboard){
    if(user !== null && dashboard !== null && typeof dashboard !== 'undefined'){
      const dashboardId = dashboard.dashboardId;
      if(dashboardId === undefined || dashboardId === null){
        setCells(null);
      } else{
        sendQuery(client, GET_DASHBOARD_CELLS, { dashboardId})
        .then((result) => {
          
          const cellsWithoutTypeName = JSON.parse(JSON.stringify(result.data.cells))

          // Strip __typename from cellsWithoutTypeName and item list
          delete cellsWithoutTypeName.__typename
          cellsWithoutTypeName.map((item) => (
            // eslint-disable-next-line no-param-reassign
            delete item.__typename,
            delete item.input.__typename,
            delete item.options.__typename
          ))

          setCells(cellsWithoutTypeName);
        }).catch((err) => console.log(err));
      }
    }
  }


  const handleSave = () => {
    sendMutation(client, UPDATE_DASHBOARD, { input: dashboard })
      .then((result) => {
        const dashboardId = dashboard.dashboardId;
        if(dashboardId === undefined || dashboardId === null){
          //Add dashboardId to the newly created dashboard
          let toBeSetAsDashboard = {
            dashboardId: result.data.updateDashboard,
            description: dashboard.description,
            name: dashboard.name,
          };
          setDashboard(toBeSetAsDashboard);
          dispatch(setCurrentDashboard(toBeSetAsDashboard));
        }else {
          dispatch(setCurrentDashboard(dashboard));
        }
      }).catch((err) => console.log(err));
  }

  const handleDelete = () => {
    const dashboardId = dashboard.dashboardId;
    sendMutation(client, DELETE_DASHBOARD, { dashboardId })
      .then(() => {
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


  const handleDeleteCell = (cellId) => {
    sendMutation(client, DELETE_CELL, { dashboardId: currentDashboard.dashboardId, cellId: cellId})
      .then(() => {
        fetchCells(currentDashboard)
        
      }).catch((err) => console.log(err));
  }


  const handleAddCell = () => {
    handleSave();
    dispatch(setQueryData(null, undefined));
  }

  const handleEditCell = (cell) => {
    handleSave();
    const input = cell.input
    sendQuery(client, GET_TIME_SERIES, { input })
      .then((result) => dispatch(setQueryData(input, result.data)))
      .catch((err) => console.log(err));
  }

  return (
    <div>
      {/* AzureAD component because AccessCheckerDecorator doesn't work on this page */}
      <AzureAD provider={authProvider}>
        {({ authenticationState }) => {
          switch (authenticationState) {
            case AuthenticationState.Authenticated:
              return(
                <React.Fragment>
                  <Navbar />
                  <div className="container_div">
                    {dashboard === null || user === null ? 
                      <h1>Loading</h1> 
                    : 
                    <div>
                      <form className="dashboard-form">
                        {/* Only give editing tools to researchers */}
                          {user.account.idToken.groups.indexOf(appsettings["groupTypes"]["researcher"]) >= 0 ?
                            <div>
                              <input className="dashboard_input" type="text" id="name" onChange={handleNameChange} value={dashboard.name} />
                              <div className="add_cell_btn">
                                <Link to={{pathname: `/cell`, state: undefined}}>
                                  <GlobalButton primary={true} btnText="Add Cell" handleButtonClick={handleAddCell}>
                                    <BiPlus />
                                  </GlobalButton>
                                </Link>
                              </div>
                              <div className="save_btn">
                                  <GlobalButton primary={true} btnText="Save" handleButtonClick={handleSave}>
                                    <BiSave />
                                  </GlobalButton>
                              </div>
                              <div className="save_btn">
                                <Link to="/dashboards">
                                  <GlobalButton primary={false} btnText="Delete" handleButtonClick={handleDelete}>
                                    <BiTrash />
                                  </GlobalButton>
                                </Link>
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
                            <div key={cell.cellId} >
                              <DashboardCellCard handleEditCell={handleEditCell} handleDeleteCell={handleDeleteCell} cell={cell}/>
                            </div>
                          );
                        })}
                      </div>
                      }
                    </div>
                    }
                  </div>
                </React.Fragment>
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
