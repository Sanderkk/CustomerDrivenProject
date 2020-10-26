import React, { useState } from "react";
import "./componentStyles/DashboardCellCard.css";
import ViewMetadata from "./globalComponents/ViewMetadata";
import { Link } from "react-router-dom";
import LineGraph from "./LineGraph";
import { DELETE_CELL } from "../queries/mutations";
import { useSelector } from "react-redux";
import sendMutation from "../queries/sendMutation";
import { useApolloClient } from "@apollo/client";
import store from "../globalState/store";
import { BiCog, BiTrash, BiPencil } from "react-icons/bi";
import { ReactComponent as MetadataIcon } from "../assets/metadata_icon.svg";

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

  const client = useApolloClient();
  const user = useSelector((store) => store.user.aadResponse);

  const handleDeleteCell = () => {
    // TODO: change from userId 123 to real one
    // const userId = user.account.accountIdentifier;
    const userId = "123";
    sendMutation(client, DELETE_CELL, {
      userId: userId,
      dashboardId: store.getState().currentDashboard.input.dashboardId,
      cellId: props.cell.cellId,
    })
      .then(() => {})
      .catch((err) => console.log(err));
  };

  return (
    <div className="cell_grid_item">
      <div className="dropdown">
        <BiCog className="dropbtn" />
        <div className="dropdown_content">
          <div>
            <BiPencil />
            <div>
              <Link
                key={"cell" + props.cell.id}
                to={{ pathname: `/cell`, state: props.cell }}
              >
                Edit graph
              </Link>
            </div>
          </div>
          <div onClick={() => setShow(true)}>
            <MetadataIcon />
            <div>Metadata</div>
          </div>
          <div className="delete_cell" onClick={() => handleDeleteCell()}>
            <BiTrash />
            <div>Delete</div>
          </div>
        </div>
      </div>
      <ViewMetadata
        sensorIDs={props.cell.input.sensors}
        show={show}
        handleClose={() => setShow(false)}
      />
      <LineGraph
        key={props.cell.name}
        options={props.cell.options}
        input={props.cell.input}
        cellId={props.cell.id}
      />
    </div>
  );
}

export default DashboardCellCard;
