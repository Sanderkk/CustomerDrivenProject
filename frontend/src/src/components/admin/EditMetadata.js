import React, { useState, useReducer, useCallback, useEffect } from "react";
import { useApolloClient } from "@apollo/client";
import "../componentStyles/admin/EditMetadata.css";
import sendQuery from "../../queries/sendQuery";
import sendMutation from "../../queries/sendMutation";
import {
  GET_METADATA,
  GET_METADATA_SENSOR_NUMBERS,
} from "../../queries/queries";
import { EDIT_METADATA } from "../../queries/mutations";
import { initialState, fields } from "../../assets/metadata";
import Modal from "../globalComponents/Modal";

function EditMetadata() {
  /*
   Page for editing metadata for different sensors
   */

  const client = useApolloClient();
  const [searchInput, setSearchInput] = useState(""); //search box
  const [existingSensorNumbers, setExistingSensorNumbers] = useState(null); // for dropdown list on search input
  const [form, updateState] = useReducer(reducer, initialState);
  const [render, setRender] = useState({
    // render state
    form: false,
    newSensor: false,
    error: false,
    loading: false,
  });

  // get list of sensors with metadata
  useEffect(() => {
    sendQuery(client, GET_METADATA_SENSOR_NUMBERS).then((res) =>
      setExistingSensorNumbers(res.data.metadata)
    );
  }, [client]);

  const updateForm = useCallback(({ target: { value, name, type } }) => {
    // if the input is a checkbox then use callback function to update
    // the toggle state based on previous state
    if (type === "checkbox") {
      updateState((prevState) => ({
        [name]: !prevState[name],
      }));

      return;
    }
    // if we have to update the root level nodes in the form
    updateState({
      [name]: value,
    });
  }, []);

  // clear state, fetch metadata for searchInput
  const handleSearch = () => {
    if (!searchInput) return;
    updateState(initialState); // clear state
    setRender((prevState) => ({
      ...prevState,
      form: false,
      loading: true,
      error: false,
    }));
    sendQuery(
      client,
      GET_METADATA,
      {
        sensorNumber: searchInput,
        onlyLast: true,
      },
      "network-only"
    )
      .then((result) => {
        const data = result.data.metadata[0];
        if (data) {
          updateState(filterOutNullValues(data));
          setRender((prevState) => ({
            ...prevState,
            form: true,
            loading: false,
          }));
        } else {
          setRender((prevState) => ({
            ...prevState,
            form: false,
            newSensor: true,
            loading: false,
          }));
        }
      })
      .catch((err) =>
        setRender((prevState) => ({
          ...prevState,
          error: "Search failed",
        }))
      );
  };

  const handleSearchInput = (value) => {
    setSearchInput(value);
    setRender((prevState) => ({
      ...prevState,
      newSensor: false,
      form: false,
    }));
  };

  // Save metadata
  const handleSave = () => {
    const input = {
      input: filterOutNullValues(form),
    };
    delete input.input.updatedAt;
    input.input.number = searchInput;
    if (!input.input.name) {
      setRender((prevState) => ({
        ...prevState,
        error: "Name field is required",
      }));
      return;
    }
    sendMutation(client, EDIT_METADATA, input)
      .then((res) => {
        setRender((prevState) => ({
          ...prevState,
          form: false,
        }));
        handleSearch();
      })
      .catch((err) => {
        setRender((prevState) => ({
          ...prevState,
          error: "Saving metadata failed",
        }));
      });
  };

  // Remove null variables and unnecessary fields on a metadata object
  const filterOutNullValues = (object) => {
    return Object.entries(object).reduce(
      (a, [k, v]) =>
        v === null ||
        v === "" ||
        !object.hasOwnProperty(k) ||
        k === "__typename"
          ? a
          : ((a[k] = v), a),
      {}
    );
  };

  return (
    <div className="edit_metadata_container">
      <div className="search">
        <span>Search for sensor number:</span>
        <span>
          <input
            value={searchInput}
            placeholder="sensor number"
            onChange={(e) => handleSearchInput(e.target.value)}
            type="text"
            list="sensor_numbers"
          />

          <datalist id="sensor_numbers">
            {existingSensorNumbers
              ? existingSensorNumbers.map((sensor, i) => (
                  <option key={i} value={sensor.number} />
                ))
              : React.Fragment}
          </datalist>
        </span>
        <span>
          <button onClick={handleSearch}>Search</button>
        </span>
      </div>
      {render.form ? (
        <div>
          <div className="form">
            {Object.keys(fields).map((key, i) => (
              <div className="line" key={i}>
                {fields[key].required ? (
                  <div>
                    {fields[key].name}
                    <span className="required">*</span>:{" "}
                  </div>
                ) : (
                  <div>{fields[key].name}: </div>
                )}
                <input
                  type={fields[key].type}
                  name={key}
                  placeholder={fields[key].placeholder}
                  onChange={updateForm}
                  value={form[key]}
                />
              </div>
            ))}
          </div>
          <div>
            <button className="save_button" onClick={handleSave}>
              Save
            </button>
          </div>
        </div>
      ) : (
        React.Fragment
      )}

      {render.newSensor ? (
        <div className="add_new">
          <p>Sensor {searchInput} was not found.</p>
          <p>Do you want to add it to the database?</p>
          <div>
            <button onClick={() => handleSearchInput("")}>Cancel</button>
            <button
              onClick={() =>
                setRender((prevState) => ({
                  ...prevState,
                  newSensor: false,
                  form: true,
                }))
              }
            >
              Yes
            </button>
          </div>
        </div>
      ) : (
        React.Fragment
      )}

      {render.loading ? <div>Loading...</div> : React.Fragment}
      <Modal
        show={render.error}
        handleClose={() =>
          setRender((prevState) => ({
            ...prevState,
            error: false,
          }))
        }
      >
        <h3>Error</h3>
        <div>{render.error}</div>
      </Modal>
    </div>
  );
}

export default EditMetadata;

const reducer = (state, updateArg) => {
  // check if the type of update argument is a callback function
  if (updateArg.constructor === Function) {
    return { ...state, ...updateArg(state) };
  }

  // if the type of update argument is an object
  if (updateArg.constructor === Object) {
    for (const key in updateArg) {
      // format dates from "yyyy-mm-ddThh:mm:ss.000Z" to "yyyy-mm-dd"
      if (key in fields && fields[key].type === "date") {
        updateArg[key] = updateArg[key].substring(0, 10);
      }
    }
    return { ...state, ...updateArg };
  }
};
