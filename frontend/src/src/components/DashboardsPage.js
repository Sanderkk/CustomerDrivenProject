import React from "react";
import Navbar from "./Navbar";
import "./componentStyles/DashboardsPage.css";
import { Link } from "react-router-dom";

function DashboardsPage() {
  /*
    Main page for /dashboards
  */
 
  return (
    <div>
      <Navbar />
      <h1>Dashboards page</h1>
      <nav>
        <Link to="/cell" className="add_cell_btn">
            Add cell
        </Link>
      </nav>
    </div>
  );
}

export default DashboardsPage;
