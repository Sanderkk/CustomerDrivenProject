import {
  SET_CURRENT_DASHBOARD,
} from "../actions/actionTypes";

export const setCurrentDashboard = (input) => {
  return {
    type: SET_CURRENT_DASHBOARD,
    payload: { input },
  };
};
