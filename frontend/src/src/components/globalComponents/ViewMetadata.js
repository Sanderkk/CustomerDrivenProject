import React, { useState, useEffect } from "react";
import { BiX } from "react-icons/bi";
import "../componentStyles/globalStyles/ViewMetadata.css";
import { GET_METADATA } from "../../queries/queries";
import { useApolloClient } from "@apollo/client";
import sendQuery from "../../queries/sendQuery";

function ViewMetadata({ sensorIDs, show, handleClose }) {
  /*
    Modal for viewing latest metadata for a list of sensors

    :param list(int) sensorIDs: list of sensor ids to fetch and display metadata for
    :param boolean show: open or close the modal
    :function handleClose: method for setting show to false 
  */
  const client = useApolloClient();
  const [metadata, setMetadata] = useState({}); // e.g. {1:{name: "temperature", sensorID: 1}, 4: {name: "humidity", sensorID: 4, number: 34525}}

  // Loops through sensorIDs. Fetch and append metadata to state for each sensorID
  useEffect(() => {
    setMetadata({});
    sensorIDs.forEach((id, index) => {
      sendQuery(client, GET_METADATA, { sensorID: id, onlyLast: true })
        .then((result) => {
          const data = result.data.metadata[0];
          data
            ? appendMetadata(id, filterOutNullValues(data))
            : appendMetadata(id, { Error: "No metadata for sensor" });
        })
        .catch((err) => console.log(err));
    });
  }, [client, sensorIDs]);

  // Append new metadata to state
  const appendMetadata = (id, data) => {
    setMetadata((prevState) => ({
      ...prevState,
      [id]: data,
    }));
  };

  // Remove null variables and unnecessary fields on a metadata object
  const filterOutNullValues = (object) => {
    return Object.entries(object).reduce(
      (a, [k, v]) => (v === null || k === "__typename" ? a : ((a[k] = v), a)),
      {}
    );
  };

  return (
    <div>
      <Modal show={show} handleClose={handleClose}>
        <h1>Metadata for Cell Name</h1>
        {Object.keys(metadata).length === 0 &&
        metadata.constructor === Object ? (
          <p>Loading...</p>
        ) : (
          <div>
            {/* Map each metadata in state */}
            {Object.keys(metadata).map((id, i) => (
              <div key={i}>
                {metadata[id].name ? (
                  <h4>Sensor {metadata[id].name}:</h4>
                ) : (
                  <h4>Sensor {id}:</h4>
                )}
                <div className="metadata">
                  {/* Map each key, value pair inside a metadata object */}
                  {Object.keys(metadata[id]).map((key, j) => (
                    <div className="line" key={j}>
                      <b>{key}: </b>
                      <span>{metadata[id][key]}</span>
                    </div>
                  ))}
                </div>
              </div>
            ))}
          </div>
        )}
      </Modal>
    </div>
  );
}

// Custom modal component
const Modal = ({ handleClose, show, children }) => {
  const showHideClassName = show ? "modal display-block" : "modal display-none";

  return (
    <div className={showHideClassName}>
      <section className="modal-main">
        <BiX onClick={handleClose} />
        {children}
      </section>
    </div>
  );
};

export default ViewMetadata;
