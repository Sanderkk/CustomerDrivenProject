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
import sendQuery from "../queries/sendQuery";
import { UPDATE_DASHBOARD, DELETE_DASHBOARD, DELETE_CELL } from "../queries/mutations";
import { GET_DASHBOARD_CELLS, GET_DASHBOARD, GET_TIME_SERIES } from "../queries/queries";
import { useApolloClient } from "@apollo/client";
import { BiSave, BiPlus, BiTrash } from "react-icons/bi";
import DashboardCellCard from "./DashboardCellCard";
import { useSelector } from "react-redux";
import groupTypes from "../groupTypes";
import { setQueryData } from "../globalState/actions/queryDataActions";

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
      // TODO: use real userId not just "123"
      // const userId = user.account.accountIdentifier;
      const userId = "123";
      if(typeof currentDashboard === 'undefined' || currentDashboard === null){
        toBeSetAsDashboard = emptyDashboard;
        toBeSetAsDashboard.userId = userId;
        dispatch(setCurrentDashboard(toBeSetAsDashboard));
        setDashboard(toBeSetAsDashboard);
        fetchCells(toBeSetAsDashboard);
      }else{
        // Must make new variable and not use state to remove state's _typename
        const dashboardId = currentDashboard.dashboardId;
        sendQuery(client, GET_DASHBOARD, { userId, dashboardId})
          .then((result) => {
            toBeSetAsDashboard = {
              dashboardId: result.data.dashboard.dashboardId,
              description: result.data.dashboard.description,
              name: result.data.dashboard.name,
            };
            toBeSetAsDashboard.userId = userId;
            dispatch(setCurrentDashboard(toBeSetAsDashboard));
            setDashboard(toBeSetAsDashboard);
            fetchCells(toBeSetAsDashboard);
          }).catch((err) => console.log(err));
      }
    }}, [user]);


  function fetchCells(dashboard){
    if(user !== null && dashboard !== null && typeof dashboard !== 'undefined'){

      // TODO: change to real user
      // const userId = user.account.accountIdentifier;
      const userId = "123"; //Test user with data
      const dashboardId = dashboard.dashboardId;
      if(dashboardId === undefined || dashboardId === null){
        setCells(null);
      } else{
        sendQuery(client, GET_DASHBOARD_CELLS, { userId, dashboardId})
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
        //TODO: when back end returns dashboardId in result. check if dashboard.dashboardId (or state.dashboardId) is undefined. If undefined: add the dashboardId to dashboard.
        // ^ this is to not continuosly create new dashboards when Save is pressed and to not have to route back to dashboards when created new dahsboard
        const dashboardId = dashboard.dashboardId;
        if(dashboardId === undefined || dashboardId === null){
          //Add dashboardId to the newly created dashboard
          let toBeSetAsDashboard = {
            dashboardId: result.data.updateDashboard,
            description: dashboard.description,
            name: dashboard.name,
            userId: dashboard.userId,
          };
          setDashboard(toBeSetAsDashboard);
          dispatch(setCurrentDashboard(toBeSetAsDashboard));
        }else {
          dispatch(setCurrentDashboard(dashboard));
        }
      }).catch((err) => console.log(err));
  }

  const handleDelete = () => {
    // TODO: change to real user
    // const userId = user.account.accountIdentifier;
    const userId = "123"; //Test user with data
    const dashboardId = dashboard.dashboardId;
    sendMutation(client, DELETE_DASHBOARD, { userId, dashboardId })
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
    // TODO: change from userId 123 to real one
    // const userId = user.account.accountIdentifier;
    const userId = "123";
    sendMutation(client, DELETE_CELL, { userId: userId, dashboardId: currentDashboard.dashboardId, cellId: cellId})
      .then(() => {
        fetchCells(currentDashboard)
        
      }).catch((err) => console.log(err));
  }


  const handleAddCell = () => {
    handleSave();
    dispatch(setQueryData(null, undefined));
  }

  const handleEditCell = (cell) => {
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
                          {user.account.idToken.groups.indexOf(groupTypes.researcher) >= 0 ?
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
