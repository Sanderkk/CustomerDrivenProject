import React, {useEffect, useState} from "react";
import Navbar from "./Navbar";
import LineGraph from "./LineGraph";
import QueryBuilder from "./QueryBuilder";
import GlobalButton from "./globalComponents/GlobalButton";
import { BiCheck } from "react-icons/bi";
import { BiX } from "react-icons/bi";

import './componentStyles/AddCell.css'
import {useDispatch, useSelector} from "react-redux";
import {updateDashboard} from "../globalState/actions/userActions";
import { useHistory } from "react-router-dom"

//This component presents a page to the user where he/she can add a new cell
//or modify an existing cell. 
function AddCell(props) {
  const [options, setOptions] = useState({ title: 'Title', primaryAxis: '', secondaryAxis: 'Value'})
  const dispatch = useDispatch();
  const history = useHistory();
  const dataSeries = useSelector(state => state.queryData.response)
  const queryInput = useSelector(state => state.queryData.input)
  const [graphdata, setGraphData] = useState([])
  const state = props.location.state;

  useEffect(() => {
    //Update the series displayed in the graph when state is updated in the store.
    if(dataSeries !== null) {
      let numberData = dataSeries.timeSeries.data
      if(numberData !== null) {
        let dataFromStore = numberData.map((sensorData) => ({
          name: sensorData.name,
          data: sensorData.data,
          pointStart: sensorData.startTime / 10000,
          pointInterval: sensorData.interval / 10000,
          yAxis: determineAxis(sensorData.data[10])
        }))
        setGraphData(dataFromStore)
      }
    }
  }, [dataSeries]);

  //Determine whether the line should be on the right or left y-axis.
  let axis2 = 0;
  const determineAxis = (value) => {
    if(value > 50){
      axis2 = 1;
    }
    else {
      axis2 = 0;
    }
    return axis2;
  }

  const handleDiscard = () => {
    //TODO: Discard cell
    history.goBack()
  }

  const handleAddCell = () => {
    //Check if any of the input-fields have been changed.
    const isEmpty = Object.values(options).every(x => (x === ''));
    if(!isEmpty) {
      //TODO: send options and input to backend. Input is generated in QueryBuilder and saved in Redux store,
      //while userOptions is generated in AddCell.
      dispatch(updateDashboard(state.dashboardId, state.cellId, options, queryInput))
      history.goBack()
    }
  }

  //Form handling of the user's changes of the options.
  const handleTitleChange = e => {
    e.preventDefault()
    setOptions({...options, title: e.target.value})
  }

  const handlePrimaryChange = e => {
    e.preventDefault()
    setOptions({...options, primaryAxis: e.target.value})
  }

  const handleSecondaryChange = e => {
    e.preventDefault()
    setOptions({...options, secondaryAxis: e.target.value})
  }

  return(
    <div>
      <Navbar />
      <div className="outer_flex_container">
        <div className="add_cell_buttons">
            
          <div className="discard_cell" >
            <GlobalButton primary={false} btnText="Discard" handleButtonClick={handleDiscard}>
              <BiX />
            </GlobalButton>
          </div>
          <div className="add_cell" >
            <GlobalButton primary={true} btnText="Add Cell" handleButtonClick={handleAddCell}>
              <BiCheck />
            </GlobalButton>
          </div>
        </div>

        <div className="flex_container">
          <div className="graph_container">
            <div className="graph">
              <LineGraph userOptions={options} graphdata={graphdata} />
            </div>
            <div className="query_builder">
              <QueryBuilder />
            </div>
          </div>
      
          <div className="options-wrapper">
            <h2>Graph Options</h2>

            <form className="options-form">

              <label className="options_label" htmlFor="title">
                Chart Title:
              </label>
              <input type="text" id="title" onChange={handleTitleChange} value={options.title} />
              
              <label className="options_label" htmlFor="primaryAxis">
                Right Y-axis:
              </label>
              <input type="text" id="primaryAxis" onChange={handlePrimaryChange} value={options.primaryAxis}/>

              <label className="options_label" htmlFor="secondaryAxis">
                Left Y-axis:
              </label>
              <input type="text" id="secondaryAxis" onChange={handleSecondaryChange} value={options.secondaryAxis}/>
          
            </form>

          </div>
        </div>
      
      </div>
    </div>
  )

}

export default AddCell;