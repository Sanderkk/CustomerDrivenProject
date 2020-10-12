import React, { useState }  from "react";
import "./componentStyles/DashboardCellCard.css";
import { DropdownMenu, MenuItem } from 'react-bootstrap-dropdown-menu';
import ViewMetadata from "./globalComponents/ViewMetadata";
import { Link } from "react-router-dom";
import LineGraph from "./LineGraph";

function DashboardCellCard(props) {
  /*
    Element in the Dashboard Specific page that shows a cell from the dashboard. 
    If clicked, one goes to the add cell page with that cell's info.

    props must have:
      cell - the cell it will show.
    cell must be on this format example:
    {
      id: 1,
      input: {
        sensors: [1, 11],
        specifiedTimePeriode: true,
        from:"2020-08-01T18:21:05.774Z",
        to:"2020-08-19T18:21:05.774Z"
      },
      options: {
        title: "Oksygen graf",
        RYAxis: "kul akse",
        LYAxis: "venstreee"
      }
    }
  */
 
 const [show, setShow] = useState(false);

  return (
    <div className="cell_grid_item">
      <DropdownMenu>
        <Link key={"cell"+props.cell.id} to={{pathname: `/cell`, state: props.cell}}>
          Edit graf
        </Link>
        <MenuItem text="Download"/>
        <MenuItem text="Metadata" onClick={() => setShow(true)}/>
        <MenuItem text="Delete"/>
      </DropdownMenu>
      <ViewMetadata sensorIDs={props.cell.input.sensors} show={show} handleClose={() => setShow(false)}/>  
      <LineGraph key={props.cell.name} options={props.cell.options} input={props.cell.input} cellId={props.cell.id} />
    </div>
  );
}


export default DashboardCellCard;
