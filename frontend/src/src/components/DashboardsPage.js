import React from "react";
import Navbar from "./Navbar";
import "./componentStyles/DashboardsPage.css";
import { Link } from "react-router-dom";
import DashboardPreviewCard from "./DashboardPreviewCard";
import GlobalButton from "./globalComponents/GlobalButton";
import { BiPlus } from "react-icons/bi";
import store from "../globalState/store";
import groupTypes from "../groupTypes";
import AzureAD from "react-aad-msal";
import { authProvider} from "../authProvider";

function DashboardsPage() {
  /*
    Main page for /dashboards
  */

  //TODO: fetch dashboards and make 

  const dashboards = [
    {
      id: 1,
      dashboardName: "My first ",
      description: "This is my first dashboard that I have created. WOHO, it's so much fun to create dashboards!",
    },
    {
      id: 3,
      dashboardName: "My second ",
      description: "This is my second dashboard that I have created. IMO, this one is much better than the first one",
    },
    {
      id: 4,
      dashboardName: ":O",
      description: "Meget kult dashboard",
    },
    {
      id: 6,
      dashboardName: "Master",
      description: "I AM THE DASHBOARD MASTER",
    },
    {
      id: 7,
      dashboardName: "Oh my godness",
      description: "Her er det masse spennende info om fisk",
    },
    {
      id: 9,
      dashboardName: "FiskeMaster",
      description: "I AM THE FISH MASTER",
    }
  ];
 
  return (
    <div>
      <Navbar />
      <div className="container_div">
        <h1>Dashboards</h1>
        <AzureAD provider={authProvider} reduxStore={store}>
        {({ accountInfo }) => {
          if (accountInfo.account.idToken.groups.indexOf(groupTypes.researcher) >= 0) {
            return (<div id="create_dashboard_btn">
            <GlobalButton primary={true} btnText="Create Dashboard">
              <BiPlus />
            </GlobalButton>
          </div>)
          }else{
            return(<div></div>)
          }
        }}
        </AzureAD>
        {/* Might be able to add this renderDashboards() function into the AzureAD component above so one doesn't have to make 2 AzureAD components.*/}
        {renderDashboards(dashboards)}
      </div>
    </div>
  );
}

//Renders what will be shown on the dashboard page, depending on user role/group and if dashboards are empty
function renderDashboards(dashboards){
  if(Object.keys(dashboards).length !== 0){
    return(
      <div>
        <h2>My dashboards</h2>
        <div className="grid_container">
          {dashboards.map((e,i) => {
            return (
              <Link key={e.id} className="dashboard_link" to={{
                pathname: `/dashboard/${e.id}`, state: e
              }} >
                <div >
                  {renderDashboardPreviewCard(e.dashboardName, e.description)}
                </div>
              </Link>
            );
          })}
        </div>
        <h2>Shared with me</h2>
        <nav>
          <Link to="/cell" className="add_cell_btn">
              Add cell
          </Link>
        </nav>
      </div>
    )}else{
      return(
        <AzureAD provider={authProvider} reduxStore={store}>
          {({ accountInfo }) => {
            if (accountInfo.account.idToken.groups.indexOf(groupTypes.customer) >= 0) {
              return (
                <div>
                  <p>Looks like you don't have access to any Dashboards yet <br/>
                  Contact your consultant to get access</p>   
                </div>)
            }else{
              return(
                <div>
                  <p>Looks like you don't have any Dashboards, why not create one?</p>   
                  <GlobalButton primary={true} btnText="Create Dashboard">
                    <BiPlus />
                  </GlobalButton>
                </div>
              )
            }
          }}
        </AzureAD>
      )
    }
}

function renderDashboardPreviewCard(name, description) {
  return <DashboardPreviewCard dashoboardName={name} description={description}/>
}

export default DashboardsPage;
