import React from "react";
import Navbar from "./Navbar";
import "./componentStyles/LogInPage.css";

import { AzureAD, AuthenticationState } from "react-aad-msal";
import { authProvider } from "../authProvider";
import store from "../globalState/store";

function LogInPage() {
  /*
    Log in page for all users
  */

  return (
    <div>
      <Navbar />
      <div id="log_in_div">
        <AzureAD provider={authProvider} reduxStore={store}>
          {({ login, logout, authenticationState, error, accountInfo }) => {
            switch (authenticationState) {
              case AuthenticationState.Authenticated:
                return (
                  <div>
                    <h1>Welcome to FishFarmData</h1>
                    <span>
                      You are logged in as {accountInfo.account.name}!
                    </span>
                    <br></br>
                    <br></br>
                    <button className="log_in_out_btn" onClick={logout}>
                      Logout
                    </button>
                  </div>
                );
              case AuthenticationState.Unauthenticated:
                return (
                  <div>
                    <h1>Log in to FishFarmData</h1>
                    {error && (
                      <p>
                        <span>
                          An error occured during authentication, please try
                          again!
                        </span>
                      </p>
                    )}
                    <p>
                      <span>Please log in with your Microsoft account</span>
                      <br></br>
                      <br></br>
                      <button className="log_in_out_btn" onClick={login}>
                        Login
                      </button>
                    </p>
                  </div>
                );
              case AuthenticationState.InProgress:
                return <p>Authenticating...</p>;
              default:
                return <p>This is not supposed to show up</p>;
            }
          }}
        </AzureAD>
      </div>
    </div>
  );
}

export default LogInPage;
