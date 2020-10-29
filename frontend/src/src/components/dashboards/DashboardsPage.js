import React, { useState, useEffect } from "react";
import Navbar from "../Navbar";
import "../componentStyles/dashboards/DashboardsPage.css";
import { Link } from "react-router-dom";
import DashboardPreviewCard from "./DashboardPreviewCard";
import GlobalButton from "../globalComponents/GlobalButton";
import { BiPlus } from "react-icons/bi";
import groupTypes from "../../groupTypes";
import sendQuery from "../../queries/sendQuery";
import { GET_DASHBOARDS } from "../../queries/queries";
import { useApolloClient } from "@apollo/client";
import { useSelector, useDispatch } from "react-redux";
import { setCurrentDashboard } from "../../globalState/actions/dashboardActions";

function DashboardsPage() {
  /*
    Main page for /dashboards
  */

  const client = useApolloClient();
  const user = useSelector((store) => store.user.aadResponse);
  const [dashboards, setDashboards] = useState(null);
  const dispatch = useDispatch();

  // If user is sat in redux, fetch dashboards for that user
  useEffect(() => {
    if (user !== null) {
      sendQuery(client, GET_DASHBOARDS, null)
        .then((result) => {
          setDashboards(result.data.dashboards);
        })
        .catch((err) => console.log(err));
    }
  }, [client, user]);

  const handleDashboardClick = (dashboard) => {
    dispatch(setCurrentDashboard(dashboard));
  };

  return (
    <div>
      <Navbar />
      {user === null || dashboards === null ? (
        <div className="container_div">
          <h1>Loading</h1>
        </div>
      ) : (
        <div className="container_div">
          <h1>Dashboards</h1>
          {user.account.idToken.groups.indexOf(groupTypes.researcher) >= 0 ? (
            // If user is a researcher, show the create dahboard button
            <div id="create_dashboard_btn">
              <Link
                to={{ pathname: `/specific-dashboard` }}
                onClick={() => handleDashboardClick(null)}
              >
                <GlobalButton primary={true} btnText="Create Dashboard">
                  <BiPlus />
                </GlobalButton>
              </Link>
            </div>
          ) : (
            ""
          )}
          {Object.keys(dashboards).length !== 0 ? (
            // If the user has dashboards, show them
            <div>
              <h2>My dashboards</h2>
              <div className="grid_container">
                {dashboards.map((dashboard, i) => {
                  return (
                    <Link
                      key={dashboard.dashboardId}
                      className="dashboard_link"
                      to={{ pathname: `/specific-dashboard` }}
                      onClick={() => handleDashboardClick(dashboard)}
                    >
                      { dashboard.description.length < 100 ? 
                        <DashboardPreviewCard
                          name={dashboard.name}
                          description={dashboard.description}
                        />
                      :
                      <DashboardPreviewCard
                        name={dashboard.name}
                        description={dashboard.description.substr(0,100)+"..."}
                      />
                      }
                    </Link>
                  );
                })}
              </div>
            </div>
          ) : (
            <div>
              {user.account.idToken.groups.indexOf(groupTypes.researcher) >=
              0 ? (
                <div>
                  <p>
                    Looks like you don't have any Dashboards, why not create
                    one?
                  </p>
                  <Link to={{ pathname: `/specific-dashboard`, state: null }}>
                    <GlobalButton primary={true} btnText="Create Dashboard">
                      <BiPlus />
                    </GlobalButton>
                  </Link>
                </div>
              ) : (
                <p>
                  Looks like you don't have access to any Dashboards yet <br />
                  Contact your consultant to get access
                </p>
              )}
            </div>
          )}
        </div>
      )}
    </div>
  );
}

export default DashboardsPage;
