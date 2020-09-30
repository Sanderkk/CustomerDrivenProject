import React, { useState, useEffect } from "react";
import { useDispatch } from "react-redux";
import { setQueryData } from "../globalState/actions/queryDataActions";
import "./componentStyles/QueryBuilder.css";
import DateTimeRangePicker from "@wojtekmaj/react-datetimerange-picker";
import { GET_TIME_SERIES } from "../queries/queries";
import { useApolloClient } from "@apollo/client";

function QueryBuilder() {
  /*
      Graphical query builder for creating queries for time series data
      Takes measurement, measurement fields, fromDate and toDate as input from user
  */
  const dispatch = useDispatch();
  const client = useApolloClient();
  // const [data, setData] = useState({}); // array of table and columns fro radio and checkboxes //TODO: implement when endpoint ready
  const [measurement, setMeasurement] = useState(""); // e.g. "Metocean"
  const [checkedItems, setCheckedItems] = useState({}); // e.g. {airHumidity %: true, airPressure hPa: false}
  const [dates, setDates] = useState([new Date(), new Date()]); // [fromDate, toDate]

  // Validates input, query api and updates global state
  useEffect(() => {
    const existTrueItem = Object.keys(checkedItems).some(
      (k) => checkedItems[k] === true
    ); // checks if any of the checkedIems are true
    if (measurement.length > 0 && existTrueItem) {
      const input = getQuery();
      // fetch time series and update global score
      client
        .query({
          query: GET_TIME_SERIES,
          variables: { input },
        })
        .then((result) => dispatch(setQueryData(input, result.data)))
        .catch((err) => console.log(err));
    }
  }); //TODO uncomment when endpoint ready // set checked items

  // Fetches data for table and columns when component is loaded
  /*useEffect(() => {
    client
      .query({
        query: GET_TABLE_AND_COLUMNS,
      })
      .then((result) => {
        setData(result.data);
        console.log(result.data);
      })
      .catch((err) => console.log(err));
  }, [client]);*/ const handleCheckboxChange = (
    event
  ) => {
    setCheckedItems({
      ...checkedItems,
      [event.target.name]: event.target.checked,
    });
  };

  // set measurement and reset checkboxes
  const handleRadioChange = (event) => {
    setMeasurement(event.target.name);
    setCheckedItems({});
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

  // Create radio buttons for measurements
  const nameToRadioButton = (name, key) => (
    <div className="qb_radio" key={key}>
      <label>
        {name}
        <input
          type="radio"
          value={name}
          name={name}
          checked={measurement === name}
          onChange={handleRadioChange}
        />
        <span className="custom_button"></span>
      </label>
    </div>
  );

  // Create checkboxes from measurement columns
  const fieldToCheckbox = (field, key) => (
    <div className="qb_checkbox" key={key}>
      <label className="category_buttons">
        {field}
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
    const selectedColumns = Object.fromEntries(
      Object.entries(checkedItems).filter(([key, value]) => value)
    );
    return {
      tableName: measurement,
      columnNames: ["time", ...Object.keys(selectedColumns)],
      from: dateToUTCDate(dates[0]),
      to: dateToUTCDate(dates[1]),
    };
  };

  return (
    <div className="qb_container">
      <div className="qb_box">
        <h3>Select Measurement</h3>
        <div>
          {/* Mapping measurement names (.data.tableAndColumns[i].key) to radio buttons*/}
          {Object.keys(data).length === 0 && data.constructor === Object ? (
            <p>Loading...</p>
          ) : (
            data.tableAndColumns.map((obj, i) => nameToRadioButton(obj.key, i))
          )}
        </div>
      </div>
      <div className="qb_box">
        <h3>Select Fields</h3>
        <div>
          {/* If measurement selected: filter data to selected measurement, select value array of first object, 
          remove "time" from array and map array to checkboxes*/}
          {measurement ? (
            data.tableAndColumns
              .filter((obj) => obj.key === measurement)[0]
              .value.filter((item) => item !== "time")
              .map((item, i) => fieldToCheckbox(item, i))
          ) : (
            <p>Please select a measurement</p>
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

// TODO: replace with api call
// eslint-disable-next-line
const data = {
  tableAndColumns: [
    {
      key: "tension",
      value: [
        "time",
        "analog_channel_0_rv009_133148",
        "analog_channel_1_rv004_99871",
        "analog_channel_2_rv011_90471",
        "analog_channel_3_rv010_133149",
        "analog_channel_4_rv005_99875",
        "analog_channel_5_rv008_133147",
        "analog_channel_6",
        "analog_channel_7",
      ],
    },
    {
      key: "wavedata_from_bjorn",
      value: [
        "uptime",
        "hm0",
        "time",
        "hm0b",
        "hmax",
        "mdir",
        "mdira",
        "mdirb",
        "sprtp",
        "thhf",
        "thmax",
        "thtp",
        "tm01",
        "tm02",
        "tm02a",
        "tm02b",
        "tp",
        "hm0a",
      ],
    },
  ],
};
