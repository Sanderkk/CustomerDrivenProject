import {
  SET_QUERY_DATA,
  SET_QUERY_DATA_INPUT,
  SET_QUERY_DATA_RESPONSE,
  REMOVE_QUERY_DATA,
} from "../actions/actionTypes";

export const setQueryData = (input, response) => {
  return {
    type: SET_QUERY_DATA,
    payload: { input, response },
  };
};

export const setQueryDataInputAction = (input) => {
  return {
    type: SET_QUERY_DATA_INPUT,
    payload: { input },
  };
};

export const setQueryDataResponseAction = (response) => {
  return {
    type: SET_QUERY_DATA_RESPONSE,
    payload: { response },
  };
};

export const removeQueryDataAction = (dispatch) => {
  return {
    type: REMOVE_QUERY_DATA,
  };
};
