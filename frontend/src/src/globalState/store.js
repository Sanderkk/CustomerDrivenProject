import { createStore, applyMiddleware, compose } from "redux";
import rootReducer from "./reducers/reducer";
import thunk from "redux-thunk";

const initialState = {};

const middlewares = [thunk];

//Create the store with the: rootReducer, initalstate and the middleware, it also enable redux devtools
const store = createStore(
  rootReducer,
  initialState,
  compose(
    applyMiddleware(...middlewares),
    window.__REDUX_DEVTOOLS_EXTENSION__
      ? window.__REDUX_DEVTOOLS_EXTENSION__()
      : (f) => f
  )
);

export default store;
