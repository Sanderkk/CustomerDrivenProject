import React, { useState, useEffect } from "react";
import Navbar from "./Navbar";
import "./componentStyles/DashboardsPage.css";
import { Link } from "react-router-dom";
import DashboardPreviewCard from "./DashboardPreviewCard";
import GlobalButton from "./globalComponents/GlobalButton";
import { BiPlus } from "react-icons/bi";
import groupTypes from "../groupTypes";
import sendQuery from "../queries/sendQuery";
import { GET_USER_DASHBOARD } from "../queries/queries";
import { useApolloClient } from "@apollo/client";
import { useSelector } from "react-redux";


function DashboardsPage() {
  /*
    Main page for /dashboards
  */
  
  const client = useApolloClient();
  const user = useSelector((store) => store.user.aadResponse);
  const [dashboards, setDashboards] = useState(null);

  // If user is sat in redux, fetch dashboards for that user
  useEffect(() => {
    if(user !== null){
      // const userId = user.account.accountIdentifier;
      const userId = "123"; //Test user with data
      sendQuery(client, GET_USER_DASHBOARD, { userId })
      .then((result) => {
        console.log(result.data.userDashboards);
        setDashboards(result.data.userDashboards);
      }).catch((err) => console.log(err));
    }
  }, [client, user]);


  return (
    <div>
      <Navbar />
      {user === null || dashboards === null ? 
        <div className="container_div">
          <h1>Loading</h1> 
        </div>
      : 
        <div className="container_div">
          <h1>Dashboards</h1>
            {user.account.idToken.groups.indexOf(groupTypes.researcher) >= 0 ?
              // If user is a researcher, show the create dahboard button
              <div id="create_dashboard_btn">
                <Link to={{pathname: `/specific-dashboard`, state: null}} >
                  <GlobalButton primary={true} btnText="Create Dashboard">
                    <BiPlus />
                  </GlobalButton>
                </Link>
              </div> : "" }
          {Object.keys(dashboards).length !== 0 ? 
            // If the user has dashboards, show them
            <div>
              <h2>My dashboards</h2>
              <div className="grid_container">
                {dashboards.map((e,i) => {
                  return (
                    <Link key={e.id} className="dashboard_link" to={{pathname: `/specific-dashboard`, state: e}}>
                      <DashboardPreviewCard name={e.name} description={e.description}/>
                    </Link>
                  );
                })}
              </div>
              <h2>Shared with me</h2>
            </div>
          :
            <div>
              {user.account.idToken.groups.indexOf(groupTypes.researcher) >= 0 ? 
                <div>
                  <p>Looks like you don't have any Dashboards, why not create one?</p>   
                  <Link to={{pathname: `/specific-dashboard`, state: null}}>
                    <GlobalButton primary={true} btnText="Create Dashboard">
                      <BiPlus />
                    </GlobalButton>
                  </Link>
                </div>
              :
                <p>Looks like you don't have access to any Dashboards yet <br/>
                Contact your consultant to get access</p>
              }
            </div>
          }
        </div>
      }
    </div>
  );
}


export default DashboardsPage;
