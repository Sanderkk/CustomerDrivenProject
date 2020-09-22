import React from 'react';
import './componentStyles/Navbar.css';
import logo from '../assets/sintef_logo.png';

function Navbar() {
  return (
    <div>
      <nav className="navbar">
        <div>
            <img className="navbar_logo" src={logo} />
            <button className="nav_button">
                Log out
            </button>
            <button className="nav_button">
                Dashboards
            </button>
            <button className="nav_button">
                Customers
            </button>
            <button className="nav_button">
                Admin Page
            </button>
            <button className="nav_button">
                Home
            </button>
        </div>
      </nav>
    </div>
  );
}

export default Navbar;
