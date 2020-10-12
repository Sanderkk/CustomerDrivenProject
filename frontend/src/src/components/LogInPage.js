import React from "react";
import Navbar from "./Navbar";
import "./componentStyles/LogInPage.css";

import { AzureAD, AuthenticationState } from "react-aad-msal";
import { authProvider } from "../authProvider";
import store from "../globalState/store";
import groupTypes from "../groupTypes";

function LogInPage() {
  /*
    Log in page for all users
  */

  return (
    <div>
      <AzureAD provider={authProvider} reduxStore={store}>
        {({ login, logout, authenticationState, error, accountInfo }) => {
          switch (authenticationState) {
            case AuthenticationState.Authenticated:
              setBodyLoggedIn();
              return (
                <div>
                  <Navbar />
                  <div className="container_div">
                    <h1>Welcome to FishFarmData</h1>
                    <span>You are logged in as {accountInfo.account.name}!</span>
                    <br/>
                    <span>Email: {accountInfo.account.userName}</span>
                    <h3>Groups:</h3>
                    {accountInfo.account.idToken.groups.indexOf(groupTypes.researcher) >= 0 && 
                      <li>Researcher</li>}
                    {accountInfo.account.idToken.groups.indexOf(groupTypes.engineer) >= 0 && 
                    <li>Engineer</li>}
                    {accountInfo.account.idToken.groups.indexOf(groupTypes.customer) >= 0 && 
                    <li>Customer</li>}
                    <br></br>
                    <br></br>
                    <button className="log_in_out_btn" onClick={logout}>
                      Logout
                    </button>
                  </div>
                </div>
              );
            case AuthenticationState.Unauthenticated:
              setBodyLoggedOut();
              return (
                <div id="log_in_div">
                  <div id="log_in_title">FishFarmData</div>
                  <p>Please sign in with your Microsoft account</p>
                  {error && (
                    <p>
                      <span>
                        An error occured during authentication, please try
                        again!
                      </span>
                    </p>
                  )}
                  <p>
                    <br></br>
                    <button className="log_in_out_btn" onClick={login}>
                      Sign in
                    </button>
                    <br />
                    <span>
                      Don't have access? Contact{" "}
                      <a href="mailto:finn.o.bjornson@sintef.no" id="finn_email"> 
                        finn.o.bjornson@sintef.no
                      </a>
                    </span>
                  </p>
                </div>
              );
            case AuthenticationState.InProgress:
              setBodyLoggedOut();
              return <p>Authenticating...</p>;
            default:
              return <p>This is not supposed to show up</p>;
          }
        }}
      </AzureAD>
    </div>
  );
}


//Two functions to change body's class so it has the BG image when logged out
function setBodyLoggedIn(){
  document.body.className = null;
}

function setBodyLoggedOut(){
  document.body.className = "log_in_body";
}


export default LogInPage;
