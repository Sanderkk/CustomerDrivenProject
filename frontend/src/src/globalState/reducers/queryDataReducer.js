import {
  SET_QUERY_DATA,
  SET_QUERY_DATA_INPUT,
  SET_QUERY_DATA_RESPONSE,
  REMOVE_QUERY_DATA,
} from "../actions/actionTypes";

const initialState = {};

export default function (state = initialState, action) {
  switch (action.type) {
    case SET_QUERY_DATA:
      return Object.assign({}, state, {
        input: action.payload.input,
        response: action.payload.response,
      });
    case SET_QUERY_DATA_INPUT:
      return Object.assign({}, state, { input: action.payload.input });
    case SET_QUERY_DATA_RESPONSE:
      return Object.assign({}, state, { response: action.payload.response });
    case REMOVE_QUERY_DATA:
      return {};
    default:
      return state;
  }
}
