import { SET_USER, REMOVE_USER } from "../actions/actionTypes";

export const setUserAction = (user) => {
  return {
    type: SET_USER,
    payload: { user },
  };
};

export const removeUserAction = (dispatch) => {
  return {
    type: REMOVE_USER,
  };
};
