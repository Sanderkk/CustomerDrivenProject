import React from "react";
import "./componentStyles/DashoboardPreviewCard.css";

function DashboardPreviewCard(props) {
  /*
    Element in the Dashboards page that previews dashboard with name and description. If clicked, one goes to the dashboard.

    props must have:
      name - the name of the dashboard
      description - the description of the dashboard
  */
 
  return (
    <div className="dashboard_grid_item">
      <h3>{props.name}</h3>
      {props.description}
    </div>
  );
}

export default DashboardPreviewCard;
