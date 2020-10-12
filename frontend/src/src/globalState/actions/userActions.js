import { AAD_LOGIN_SUCCESS, AAD_LOGOUT_SUCCESS, AAD_ACQUIRED_ACCESS_TOKEN_SUCCESS } from "../actions/actionTypes";

export const setUserAction = (user) => {
  return {
    type: AAD_LOGIN_SUCCESS,
    payload: { user },
  };
};

export const removeUserAction = (dispatch) => {
  return {
    type: AAD_LOGOUT_SUCCESS,
  };
};

export const setAccessToken = (token) => {
  return {
    type: AAD_ACQUIRED_ACCESS_TOKEN_SUCCESS,
    payload: { token },
  };
};