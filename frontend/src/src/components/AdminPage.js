import React, { useState } from "react";

import Navbar from "./Navbar";
import UploadData from "./UploadData";
import EditMetadata from "./admin/EditMetadata";
import SensorAccess from "./admin/SensorAccess";
import UserGroups from "./admin/UserGroups";
import "./componentStyles/AdminPage.css";

function AdminPage() {
  /*
    Main page for /admin
    Contains tab navigation for the different admin pages
  */

  const [activeTab, setActiveTab] = useState(1);
  const tabTitles = ["Upload data", "Metadata", "Sensor access", "User groups"];

  return (
    <div className="admin">
      <Navbar />
      <div className="tab-container">
        <h1>Admin page</h1>
        <ul className="header">
          {tabTitles.map((title, i) => (
            <Tab
              id={i}
              key={i}
              title={title}
              activeID={activeTab}
              handleClick={() => setActiveTab(i)}
            />
          ))}
        </ul>
        <div className="content">
          {
            /* Conditional rendering of tab content */
            {
              0: <UploadData />,
              1: <EditMetadata />,
              2: <SensorAccess />,
              3: <UserGroups />,
            }[activeTab]
          }
        </div>
      </div>
    </div>
  );
}

export default AdminPage;

// General tab component
const Tab = ({ id, title, activeID, handleClick }) => {
  const activeTab = id === activeID ? "tab active" : "tab"; // determine if tab is active

  return (
    <li onClick={() => handleClick(id)} className={activeTab}>
      {title}
    </li>
  );
};
