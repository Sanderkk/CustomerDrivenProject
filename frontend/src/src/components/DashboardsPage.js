import React from "react";
import Navbar from "./Navbar";
import LineGraph from "./LineGraph"
import "./componentStyles/DashboardsPage.css";
import QueryBuilder from "./QueryBuilder";

function DashboardsPage() {
  /*
    Main page for /dashboards
  */
 
  return (
    <div>
      <Navbar />
      <h1>Dashboards page</h1>
      <QueryBuilder />
      <LineGraph />
    </div>
  );
}

export default DashboardsPage;
