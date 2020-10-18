import { AAD_LOGIN_SUCCESS, AAD_LOGOUT_SUCCESS, AAD_ACQUIRED_ACCESS_TOKEN_SUCCESS, UPDATE_DASHBOARD_CELL, UPDATE_DASHBOARD } from "../actions/actionTypes";

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

export const updateDashboard = (dashboardId, cellId, options, queryInput) => {
  return {
    type: UPDATE_DASHBOARD,
    dashboardId: dashboardId,
    cellId: cellId,
    payload: {options, queryInput}
  }
}

export const updateDashboardCell = (id, options, queryInput) => {
  return {
    type: UPDATE_DASHBOARD_CELL,
    id: id,
    payload: {options, queryInput}
  }
}