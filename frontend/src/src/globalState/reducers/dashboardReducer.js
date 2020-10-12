import {
  SET_CURRENT_DASHBOARD,
} from "../actions/actionTypes";

const initialState = {};

export default function (state = initialState, action) {
  switch (action.type) {
    case SET_CURRENT_DASHBOARD:
      return Object.assign({}, state, {
        input: action.payload.input,
      });
    default:
      return state;
  }
}
