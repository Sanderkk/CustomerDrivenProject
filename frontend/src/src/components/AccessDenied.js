import React from "react";
import "../App.css";

function AccessDenied() {
  /*
  Access Denied Page
  Shown when a user without the right autharization tries to access a page they are not allowed to
  */

  return (
    <div className="App">
      <header className="App-header">
        <h2>Access Denied</h2>
        <h5>
          You are not authorized to access this page <br />
          If you are supposed to have access, contact your IT responsibe <br />
          (contact Turid for access)
        </h5>
      </header>
    </div>
  );
}

export default AccessDenied;
