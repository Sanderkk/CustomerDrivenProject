import { combineReducers } from "redux";

import userReducer from "./userReducer";
import queryDataReducer from "./queryDataReducer";
import dashboardReducer from "./dashboardReducer";

export default combineReducers({
  //reducerName: reducerImported
  user: userReducer,
  queryData: queryDataReducer,
  currentDashboard: dashboardReducer,
});
