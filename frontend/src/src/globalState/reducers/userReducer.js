import { SET_USER, REMOVE_USER } from "../actions/actionTypes";

const initialState = {};

export default function (state = initialState, action) {
  switch (action.type) {
    case SET_USER:
      return action.payload.user;
    case REMOVE_USER:
      return {};
    default:
      return state;
  }
}
