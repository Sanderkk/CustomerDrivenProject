import React from "react";
import ReactDOM from "react-dom";
import { Route, BrowserRouter as Router, Switch } from "react-router-dom";
import "./index.css";
import * as serviceWorker from "./serviceWorker";
import store from "./globalState/store";
import { Provider } from "react-redux";
import { ApolloClient, InMemoryCache, ApolloProvider } from "@apollo/client";
import NotFound from "./components/NotFound";
import LogInPage from "./components/LogInPage";
import AdminPage from "./components/AdminPage";
import DashboardsPage from "./components/dashboards/DashboardsPage";
import AccessCheckerDecotaor from "./components/AccessCheckerDecorator";
import groupTypes from "./groupTypes";
import AddCell from "./components/dashboards/AddCell";
import DashboardSpecificPage from "./components/dashboards/DashboardSpecificPage";

const client = new ApolloClient({
  cache: new InMemoryCache(),
});

// Routes for the different pages and 404 default page

const routing = (
  <Provider store={store}>
    <React.StrictMode>
      <ApolloProvider client={client}>
        <Router>
          <Switch>
            <Route exact path="/" component={LogInPage} />
            <Route
              // Use AccessCheckerDecorator around a main page component to make it so that only a specified groupType has access to that URL
              path="/admin"
              component={() => (
                <AccessCheckerDecotaor
                  mainPage={<AdminPage />}
                  group={groupTypes.engineer}
                />
              )}
            />
            <Route
              path="/dashboards"
              component={() => (
                <AccessCheckerDecotaor
                  mainPage={<DashboardsPage />}
                  group="true"
                  // group ="true" gives access if logged in
                />
              )}
            />
            <Route exact path="/specific-dashboard" component={DashboardSpecificPage} />
            <Route exact path="/cell" component={AddCell} />
            <Route component={NotFound} />
          </Switch>
        </Router>
      </ApolloProvider>
    </React.StrictMode>
  </Provider>
);

ReactDOM.render(routing, document.getElementById("root"));

serviceWorker.unregister();
