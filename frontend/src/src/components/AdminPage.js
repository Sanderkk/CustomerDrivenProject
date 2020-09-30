import React, { useState } from "react";
import Navbar from "./Navbar";
import UploadData from "./UploadData";
import "./componentStyles/AdminPage.css";

function AdminPage() {
  /*
    Main page for /admin
  */

  return (
    <div>
      <Navbar />
      <h1>Admin page</h1>
      <UploadData />
    </div>
  );
}

export default AdminPage;
