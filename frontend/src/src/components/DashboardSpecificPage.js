import React from "react";
import Navbar from "./Navbar";
import "./componentStyles/DashboardsPage.css";
import { Link } from "react-router-dom";
import DashboardPreviewCard from "./DashboardPreviewCard";
import { AzureAD, AuthenticationState } from "react-aad-msal";
import { authProvider } from "../authProvider";
import AccessDenied from "./AccessDenied";
import {useSelector} from "react-redux";

function DashboardSpecificPage(props) {
  /*
    Page showing specific dashboard
  */

  const state = props.location.state;
  const cells = useSelector(state => state.user.dashboards)

  //Check if state is undefined, if so => we have no data and don't know which dashboard this is.
  if(typeof state === 'undefined'){
    return(
      <div>
        <Navbar/>
        <h1>
          Couldn't find which dashboard you wanted to see. (state === undefined)
          If you tried to go to a dashboard through the URL, please don't. Click on the dashboard from the Dashboards page.
        </h1>
      </div>
    )
  }

  const cellId = cells[state.id] ? Object.keys(cells[state.id].data).length : 0

 
  return (
    <div>
      <Navbar />
      {/* AzureAD component because AccessCheckerDecorator doesn't work on this page */}
      <AzureAD provider={authProvider}>
        {({ authenticationState, accountInfo }) => {
          switch (authenticationState) {
            case AuthenticationState.Authenticated:
              return(
                <div>
                  <h1>ID: {state.id}</h1>
                  <h1>Current Cell Id: {cellId}</h1>
                  <h1>
                    {state.dashboardName}
                  </h1>
                  <h2>{state.description}</h2>
                  <nav>
                    <Link to={{pathname: `${state.id}/cell/${cellId}`, state: {dashboardId: state.id, cellId: cellId}}} className="add_cell_btn">
                        Add cell
                    </Link>
                  </nav>
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
