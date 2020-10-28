import React from "react";
import "../App.css";

import { AzureAD, AuthenticationState } from "react-aad-msal";
import { authProvider } from "../authProvider";
import AccessDenied from "./AccessDenied";

function AccessCheckerDecorator(props) {
  /*
    Decorator for main page components.
    Takes in the main page component and group the user has to be part of to be able to access the page component
    If access denied, AccessDenied is rendered instead of the main page component

    To use this decorator, add the main page component as mainPage in props and what groupType the user has to be as group in props.

    NB! Does not work on pages that require props! Ex DashboardSpecificPage.
    (Can maybe get arond this with redux, but idk yet)
  */
 
  return (
<div>
      <AzureAD provider={authProvider}>
        {({ authenticationState, accountInfo }) => {
          switch (authenticationState) {
            case AuthenticationState.Authenticated:
              if (
                accountInfo.account.idToken.groups.indexOf(props.group) >= 0 ||
                props.group === "true"
              ) {
                return props.mainPage;
              } else {
                return <AccessDenied></AccessDenied>;
              }
            default:
              // Can add a new component here if it is wanted to have another page show if not signed in
              return <AccessDenied></AccessDenied>;
          }
        }}
      </AzureAD>
    </div>
  );
}

export default AccessCheckerDecorator;
