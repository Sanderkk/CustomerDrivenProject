import React from "react";
import ReactDOM from "react-dom";
import { Route, BrowserRouter as Router, Switch } from "react-router-dom";
import "./index.css";
import App from "./App";
import * as serviceWorker from "./serviceWorker";
import store from "./globalState/store";
import { Provider } from "react-redux";
import NotFound from "./components/NotFound";
import AdminPage from "./components/AdminPage";
import CustomersPage from "./components/CustomersPage";
import DashboardsPage from "./components/DashboardsPage";

// Routes for the different pages and 404 default page
const routing = (
  <Provider store={store}>
    <React.StrictMode>
      <Router>
        <Switch>
          <Route exact path="/" component={App} />
          <Route path="/admin" component={AdminPage} />
          <Route path="/customers" component={CustomersPage} />
          <Route path="/dashboards" component={DashboardsPage} />
          <Route component={NotFound} />
        </Switch>
      </Router>
    </React.StrictMode>
  </Provider>
);

ReactDOM.render(routing, document.getElementById("root"));

serviceWorker.unregister();
