import React from "react";
import "../App.css";
import Navbar from "./Navbar";

function NotFound() {
  /*
  404 Page
  */

  return (
    <div className="App">
      <Navbar />
      <header className="App-header">
        <h2>404 Not Found</h2>
      </header>
    </div>
  );
}

export default NotFound;
