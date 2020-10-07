// authProvider.js
import { MsalAuthProvider, LoginType } from "react-aad-msal";

// Msal Configurations
const config = {
  auth: {
    clientId: "8a2d7ffd-3754-4913-8277-8ed6a867545f",
    authority:
      "https://login.microsoftonline.com/1ddce0ea-82b3-4340-b681-550781c83ba2/",
    redirectURI: "http://localhost:3013",
  },
  cache: {
    cacheLocation: "localStorage",
    storeAuthStateInCookie: true,
  },
};

// Authentication Parameters
const authenticationParameters = {
  scopes: [
    "api://0ea5e8ad-2ac5-4ea4-bb39-74213a21e4f4/user.read",
    "api://0ea5e8ad-2ac5-4ea4-bb39-74213a21e4f4/access_as_user",
  ],
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
