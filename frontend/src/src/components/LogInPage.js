import React from "react";
import "./componentStyles/LogInPage.css";
import { Redirect } from "react-router";
import { AzureAD, AuthenticationState } from "react-aad-msal";
import { authProvider } from "../authProvider";
import store from "../globalState/store";

function LogInPage() {
  /*
    Log in page for all users
  */

  return (
    <div>
      <AzureAD provider={authProvider} reduxStore={store}>
        {({ login, authenticationState, error }) => {
          switch (authenticationState) {
            case AuthenticationState.Authenticated:
              setBodyLoggedIn();
              return (
                <div>
                  <Redirect to="/dashboards" />
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
                      Sign In
                    </button>
                    <br />
                    <span>
                      Don't have access? Contact{" "}
                      <a
                        href="mailto:finn.o.bjornson@sintef.no"
                        id="finn_email"
                      >
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
function setBodyLoggedIn() {
  document.body.className = null;
}

function setBodyLoggedOut() {
  document.body.className = "log_in_body";
}

export default LogInPage;
