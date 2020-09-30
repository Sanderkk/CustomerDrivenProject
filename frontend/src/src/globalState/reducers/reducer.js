import { combineReducers } from "redux";

import userReducer from "./userReducer";
import queryDataReducer from "./queryDataReducer";

export default combineReducers({
  //reducerName: reducerImported
  user: userReducer,
  queryData: queryDataReducer,
});
