import React, { useState, useEffect } from "react";
import { useDispatch } from "react-redux";
import { setQueryData } from "../../globalState/actions/queryDataActions";
import "../componentStyles/dashboards/QueryBuilder.css";
import DateTimeRangePicker from "@wojtekmaj/react-datetimerange-picker";
import { GET_TIME_SERIES, GET_SENSORS } from "../../queries/queries";
import { useApolloClient } from "@apollo/client";
import sendQuery from "../../queries/sendQuery";

function QueryBuilder(props) {
  /*
      Graphical query builder for creating queries for time series data
      Takes measurement, measurement fields, fromDate and toDate as input from user
  */
  const dispatch = useDispatch();
  const client = useApolloClient();
  const [sensors, setSensors] = useState({}); // array of table and columns fro radio and checkboxes
  const [sensorType, setSensorType] = useState(""); // e.g. o2concentration_um_
  const [checkedItems, setCheckedItems] = useState({}); // e.g. {1: true, 3: false}
  const [dates, setDates] = useState(props.graphInput !== undefined 
    ? [new Date(props.graphInput.from), new Date(props.graphInput.to)] 
    : [new Date(Date.now() - 604800000), new Date()]); // [fromDate, toDate]

  const initialSensors = () => {
    let initialSensors = props.graphInput.sensors
    let newArr = {}
    if (Object.keys(checkedItems).length === 0) {
      for(let i = 0; i < initialSensors.length; i++) {
        newArr = {...newArr, [initialSensors[i]]: true}
      }
      setCheckedItems(newArr)
    }
  }


  // Validates input, query api and updates global state
  useEffect(() => {
    let mounted = true;
    const existTrueItem = Object.keys(checkedItems).some(
      (k) => checkedItems[k] === true
    ); // checks if any of the checkedIems are true
    if (sensorType.length > 0 && existTrueItem) {
      const input = getQuery();
      if(mounted){
        // fetch time series and update global state
        sendQuery(client, GET_TIME_SERIES, { input })
          .then((result) => dispatch(setQueryData(input, result.data)))
          .catch((err) => console.log(err));
      }
    }
    return () => mounted = false;
    
  }, [client, sensorType, checkedItems, dates]);

  // Fetches data for table and columns when component is loaded
  useEffect(() => {
    sendQuery(client, GET_SENSORS, null)
      .then((result) => {
        setSensors(result.data);
      })
      .catch((err) => console.log(err));
    if(props.graphInput !== undefined) {
      initialSensors()
    }
  }, [client, props.graphInput]);

  const handleCheckboxChange = (event) => {
    setCheckedItems({
      ...checkedItems,
      [event.target.name]: event.target.checked,
    });
  };

  // set measurement and reset checkboxes
  const handleRadioChange = (event) => {
    setSensorType(event.target.name);
  };

  // return whether a measurement column is checked or not
  const isChecked = (name) => {
    const checked = checkedItems[name];
    if (checked === undefined) {
      // necessary to make checkboxes controlled by state
      return false;
    }
    return checked;
  };

  const isMeasurementChecked = (key) => {
    let undefinedList = []
    for(let i = 0; i < key.length; i++) {
      const checked = checkedItems[key[i]]; 
      if (checked === undefined) {
        undefinedList.push(checked)
        // necessary to make checkboxes controlled by state
      }
      else if(isChecked(key[i])) {
        return true;
      }
    }
    if (undefinedList.length > 0) {
      return false;
    }
}

  // Create checkboxes for measurements
  const nameToRadioButton = (name, key) => (
    <div className="qb_checkbox" key={key}>
      <label className="category_buttons">
        {name}
        <input
          type="checkbox"
          value={name}
          name={name}
          checked={isMeasurementChecked(key)}
          onChange={handleRadioChange}
        />
        <span className="checkmark"></span>
      </label>
    </div>
  );

  // Finds a sensors number for it's id and index, the number is used as the label for the checkbox
  function findSensorNumber(sensorId, index){
    let name = "No number";
    sensors.sensors.map(element => {
      element.sensorIds.forEach(id => {
        if(id === sensorId){
          name = element.sensorNumbers[index];
        }
      })
    });
    return name;
  } 

  // Create checkboxes from measurement columns
  const fieldToCheckbox = (field, key) => (
    <div className="qb_checkbox" key={key}>
      <label className="category_buttons">
        {findSensorNumber(field, key)}
        <input
          type="checkbox"
          value={field}
          name={field}
          checked={isChecked(field)}
          onChange={handleCheckboxChange}
        />
        <span className="checkmark"></span>
      </label>
    </div>
  );

  // To make the input UTC and not local time
  const dateToUTCDate = (date) => {
    const MS_PER_MINUTE = 60000;
    return new Date(
      date.getTime() - MS_PER_MINUTE * date.getTimezoneOffset()
    ).toISOString();
  };

  const getQuery = () => {
    const selectedColumns = Object.keys(Object.fromEntries(
      Object.entries(checkedItems).filter(([key, value]) => value))
    );
    const intKeys = []
    selectedColumns.map((id) =>
      intKeys.push(parseInt(id))
    )
    
    return {
      sensors: intKeys,
      specifiedTimePeriod: true,
      from: dateToUTCDate(dates[0]),
      to: dateToUTCDate(dates[1]),
    };

  };

  return (
    <div className="qb_container">
      <div className="qb_box">
        <h3>Select Sensor Type</h3>
        <div>
          {/* Mapping measurement names (.data.tableAndColumns[i].key) to radio buttons*/}
          {Object.keys(sensors).length === 0 &&
          sensors.constructor === Object ? (
            <p>Loading...</p>
          ) : (
            /*data.tableAndColumns.map((obj, i) => nameToRadioButton(obj.key, i))*/
            sensors.sensors.map((obj) =>
              nameToRadioButton(obj.sensorTypeName, obj.sensorIds)
            )
          )}
        </div>
      </div>
      <div className="qb_box">
        <h3>Select {sensorType} Sensors </h3>
        <div>
          {/* If measurement selected: filter data to selected measurement, select value array of first object, 
          remove "time" from array and map array to checkboxes*/}
          {sensorType ? (
            sensors.sensors
              .filter((obj) => obj.sensorTypeName === sensorType)[0]
              .sensorIds.filter((item) => item !== 0)
              .map((item, i) => fieldToCheckbox(item, i))
          ) : (
            <p>Please select a sensor type</p>
          )}
        </div>
      </div>
      <div className="qb_date">
        <h3>Select time</h3>
        <DateTimeRangePicker
          onChange={setDates}
          value={dates}
          calendarIcon={null}
          clearIcon={null}
          disableClock={true}
          maxDate={new Date()}
        />
        <p>UTC Time</p>
      </div>
    </div>
  );
}

export default QueryBuilder;
