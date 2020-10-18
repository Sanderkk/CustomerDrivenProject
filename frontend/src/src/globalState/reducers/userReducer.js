import {authProvider} from "../../authProvider";

const initialState = {
  aadResponse: null,
  accessToken: null,
  dashboards: {},
  cells: [],
};

const userReducer = (state = initialState, action) => {
  switch (action.type) {
    case "AAD_LOGIN_SUCCESS":
      // When logged in successfully, retrive access token.
      // nb! not best practice to make reducer call new action, but wanted to get access token after login success sooo Imma put the .getAccessToken() here. sry pattern. 
      authProvider.getAccessToken();
      return {...state, aadResponse: action.payload};
    case "AAD_LOGOUT_SUCCESS":
      return {...state, aadResponse: null};
    case "AAD_ACQUIRED_ACCESS_TOKEN_SUCCESS":
      return {...state, accessToken: action.payload};
    case "UPDATE_DASHBOARD":
      const dashboardCopy = {...state.dashboards}
      let array = dashboardCopy[action.dashboardId]
       ? {...dashboardCopy[action.dashboardId].data}
       : {}
      array[action.cellId] = action.payload
      dashboardCopy[action.dashboardId] = {id: action.dashboardId, data: array}
      return { ...state, dashboards: dashboardCopy }
    case "UPDATE_DASHBOARD_CELL":
      const cellCopy = [...state.cells]
      const ok = state.cells.length > action.id
        ? cellCopy[action.id] = action.payload
        : cellCopy.push(action.payload)
      return { ...state, cells: cellCopy}
    default:
      return state;
  }
};

export default userReducer;
