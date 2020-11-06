// authProvider.js
import { MsalAuthProvider, LoginType } from "react-aad-msal";
import appsettings from "./appsettings.json";

// Msal Configurations
const config = {
  auth: {
    clientId: appsettings["AAD"]["clientId"],
    authority: appsettings["AAD"]["authority"],
    redirectURI: appsettings["AAD"]["redirectURI"],
  },
  cache: {
    cacheLocation: "localStorage",
    storeAuthStateInCookie: true,
  },
};

// Authentication Parameters
const authenticationParameters = {
  scopes: appsettings["AAD"]["scopes"],
  forceRefresh: true,
};

// Options
const options = {
  loginType: LoginType.Redirect,
  tokenRefreshUri: window.location.origin + "/auth.html",
};

export const authProvider = new MsalAuthProvider(
  config,
  authenticationParameters,
  options
);
