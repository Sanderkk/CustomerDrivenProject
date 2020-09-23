import React from "react";
import { Link } from "react-router-dom";
import { useDispatch } from "react-redux";
import { removeUserAction } from "../globalState/actions/userActions";

import "./componentStyles/Navbar.css";
import logo from "../assets/sintef_logo.png";

function Navbar() {
  /*
    Navigation bar to the different pages
  */

  const dispatch = useDispatch(); // redux dispatch to send action

  return (
    <div>
      <nav className="navbar">
        <div>
          <Link to="/">
            <img className="navbar_logo" src={logo} alt="Sintef" />
          </Link>
          <Link
            to="/"
            onClick={() => dispatch(removeUserAction())} // Remove user from redux store
            className="nav_button"
          >
            Log out
          </Link>
          <Link to="/dashboards" className="nav_button">
            Dashboards
          </Link>
          <Link to="/customers" className="nav_button">
            Customers
          </Link>
          <Link to="/admin" className="nav_button">
            Admin Page
          </Link>
          <Link to="/" className="nav_button">
            Home
          </Link>
        </div>
      </nav>
    </div>
  );
}

export default Navbar;
