import {authProvider} from "../../authProvider";

const initialState = {
  aadResponse: null,
  accessToken: null,
};

const userReducer = (state = initialState, action) => {
  switch (action.type) {
    case "AAD_LOGIN_SUCCESS":
      // When logged in successfully, retrive access token.
      // nb! not best practice to make reducer call new action, but wanted to get access token after login success sooo Imma put the .getAccessToken() here. sry pattern. 
      authProvider.getAccessToken();
      return { ...state, aadResponse: action.payload };
    case "AAD_LOGOUT_SUCCESS":
      return { ...state, aadResponse: null, accessToken: null };
    case "AAD_ACQUIRED_ACCESS_TOKEN_SUCCESS":
      return { ...state, accessToken: action.payload };
    default:
      return state;
  }
};

export default userReducer;
