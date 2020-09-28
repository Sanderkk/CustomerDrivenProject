import { AAD_LOGIN_SUCCESS, AAD_LOGOUT_SUCCESS } from "../actions/actionTypes";

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
