import React from "react";
import { Link, useLocation } from "react-router-dom";
import groupTypes from "../groupTypes";
import Profile from "./Profile";

function NavLinks({ accountInfo }) {
  const location = useLocation();

  return (
    <div>
      {/* Link to LoginPage with user's name on it if logged in */}
      <div className="nav_button">
        <Profile />
      </div>
      {/* Link to users dashboards, all users have access to this if logged in */}
      <Link
        to="/dashboards"
        className={
          location.pathname.includes("dashboard") ||
          location.pathname === "/cell"
            ? "nav_button active"
            : "nav_button"
        }
      >
        Dashboards
      </Link>
      {/* If the user is a part of the Engineer group then the Admin page link appears */}
      {accountInfo.account.idToken.groups.indexOf(groupTypes.engineer) >= 0 ? (
        <Link
          to="/admin"
          className={
            location.pathname === "/admin" ? "nav_button active" : "nav_button"
          }
        >
          Admin Page
        </Link>
      ) : (
        ""
      )}
    </div>
  );
}

export default NavLinks;
