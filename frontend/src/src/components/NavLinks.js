import React from "react";
import { Link, useLocation } from "react-router-dom";
import groupTypes from "../groupTypes";
import Profile from "./Profile";

function NavLinks({ accountInfo }) {
  const location = useLocation();

  return (
    <div>
      {/* Link to LoginPage with user's name on it if logged in */}
      {/*<Link
        to="/"
        className={
          location.pathname === "/" ? "nav_button active" : "nav_button"
        }
      >
        {accountInfo.account.name}
      </Link> */}
      <div className="nav_button">
        <Profile />
      </div>

      {/* Link to users dashboards, all users have access to this if logged in */}
      <Link
        to="/dashboards"
        className={
          location.pathname === "/dashboards"
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
      {/* If the user is part of the Researcher group then the Customers page link appears */}
      {accountInfo.account.idToken.groups.indexOf(groupTypes.researcher) >=
      0 ? (
        <Link
          to="/customers"
          className={
            location.pathname === "/customers"
              ? "nav_button active"
              : "nav_button"
          }
        >
          Customers
        </Link>
      ) : (
        ""
      )}
    </div>
  );
}

export default NavLinks;
