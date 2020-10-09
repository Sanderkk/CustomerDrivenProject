import React from "react";
import { Link } from "react-router-dom";
import "./componentStyles/Navbar.css";
import logo from "../assets/sintef_logo.png";
import { AzureAD, AuthenticationState } from "react-aad-msal";
import { authProvider } from "../authProvider";
import groupTypes from "../groupTypes";

function Navbar() {
  /*
    Navigation bar to the different pages
  */

  return (
    <div>
      <nav className="navbar">
        <div>
          <Link to="/">
            <img className="navbar_logo" src={logo} alt="Sintef" />
          </Link>
          {/* AzureAD component uses the authProvider to retrieve authenticationState and accountInfo. Either with retrival of Token or by using already retrived Token (hopefully)
          This takes a bit of time, and is the reason some parts of the page uses some extra time before appearing. 
          Might be able to use the redux state instead of the AzureAD component to increase retrival speed. However this did not work when barely tried.*/}
          <AzureAD provider={authProvider}>
            {({ authenticationState, accountInfo }) => {
              switch (authenticationState) {
                case AuthenticationState.Authenticated:
                  return (
                    <div>
                      {/* Link to LoginPage with user's name on it if logged in */}
                      <Link to="/" className="nav_button">
                        {accountInfo.account.name}
                      </Link>
                      {/* Link to users dashboards, all users have access to this if logged in */}
                      <Link to="/dashboards" className="nav_button">
                        My Dashboards
                      </Link>
                      {/* If the user is a part of the Engineer group then the Admin page link appears */}
                      {accountInfo.account.idToken.groups.indexOf(
                        groupTypes.engineer
                      ) >= 0 ? (
                        <Link to="/admin" className="nav_button">
                          Admin Page
                        </Link>
                      ) : (
                        ""
                      )}
                      {/* If the user is part of the Researcher group then the Customers page link appears */}
                      {accountInfo.account.idToken.groups.indexOf(
                        groupTypes.researcher
                      ) >= 0 ? (
                        <Link to="/customers" className="nav_button">
                          Customers
                        </Link>
                      ) : (
                        ""
                      )}
                    </div>
                  );
                default:
                  return {
                    /* If not logged in then show Sign in link */
                  };
              }
            }}
          </AzureAD>
        </div>
      </nav>
    </div>
  );
}

export default Navbar;
