import React from "react";
import { Link } from "react-router-dom";
import "./componentStyles/Navbar.css";
import logo from "../assets/sintef_logo.png";
import { AzureAD, AuthenticationState } from "react-aad-msal";
import { authProvider } from "../authProvider";
import store from "../globalState/store";
import NavLinks from "./NavLinks";

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
          <AzureAD provider={authProvider} reduxStore={store}>
            {({ authenticationState, accountInfo }) => {
              switch (authenticationState) {
                case AuthenticationState.Authenticated:
                  return <NavLinks accountInfo={accountInfo} />;
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
