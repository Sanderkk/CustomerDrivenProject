import React, { useState } from "react";
import Navbar from "./Navbar";
import LineGraph from "./LineGraph";
import QueryBuilder from "./QueryBuilder";
import GlobalButton from "./globalComponents/GlobalButton";
import { BiCheck, BiX, BiNews } from "react-icons/bi";
import ViewMetadata from "./globalComponents/ViewMetadata";
import { useSelector } from "react-redux";
import { useDispatch } from "react-redux";
import { useApolloClient } from "@apollo/client";
import { UPDATE_CELL } from "../queries/mutations";
import { Link } from "react-router-dom";
import sendMutation from "../queries/sendMutation";

import './componentStyles/AddCell.css'

//This component presents a page to the user where he/she can add a new cell
//or modify an existing cell. 
function AddCell(props) {
  const state = props.location.state;
  const [options, setOptions] = useState(state !== undefined ? state.options : { title: '', rYAxis: '', lYAxis: ''})
  const [show, setShow] = useState(false);
  const input = useSelector(store => store.queryData.input)
  const dashboard = useSelector(store => store.currentDashboard.input)
  const queryData = useSelector(store => store.queryData)
  const user = useSelector((store) => store.user.aadResponse);
  const dispatch = useDispatch();
  const client = useApolloClient();


  const handleAddCell = () => {
    //Check if any of the input-fields have been changed. 
    const isEmpty = Object.values(options).every(x => (x === ''));
    if(!isEmpty) {
      //TODO: send options and input to backend. Input is generated in QueryBuilder and saved in Redux store,
      //while userOptions is generated in AddCell.
      let cellToBeSaved = {}
      cellToBeSaved.input = queryData.input;
      cellToBeSaved.options = options;
      // TODO: real userId
      // cellToBeSaved.userId = user.account.accountIdentifier;
      cellToBeSaved.userId = "123";
      cellToBeSaved.dashboardId = dashboard.dashboardId
      if(state !== undefined){
        cellToBeSaved.cellId = state.cellId;
      }
      sendMutation(client, UPDATE_CELL, { input: cellToBeSaved })
      .then(() => {
      }).catch((err) => console.log(err));
    }
  }

  //Form handling of the user's changes of the options.
  const handleTitleChange = e => {
    e.preventDefault()
    setOptions({...options, title: e.target.value})
  }

  const handlePrimaryChange = e => {
    e.preventDefault()
    setOptions({...options, rYAxis: e.target.value})
  }

  const handleSecondaryChange = e => {
    e.preventDefault()
    setOptions({...options, lYAxis: e.target.value})
  }

  return(
    <div>
      <Navbar />
      <div className="outer_flex_container">
        <div className="add_cell_buttons">
            
          <div className="discard_cell" >
            <Link to="/specific-dashboard">
              <GlobalButton primary={false} btnText="Discard">
                <BiX />
              </GlobalButton>
            </Link>
          </div>
          <div className="add_cell" >
            <Link to="/specific-dashboard">
              <GlobalButton primary={true} btnText="Save Cell" handleButtonClick={handleAddCell}>
                <BiCheck />
              </GlobalButton>
            </Link>
          </div>
        </div>

        <div className="flex_container">
          <div className="graph_container">
          <div className="graph">
              {state !== undefined ?
              <LineGraph options={options} input={state.input} cellId={state.id} />
              :
              <LineGraph options={options} />
              }
            </div>

            <div className="query_builder">
              {state !== undefined ?
              <QueryBuilder graphInput={state.input} />
              :
              <QueryBuilder />
              }
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
              <input type="text" id="primaryAxis" onChange={handlePrimaryChange} value={options.rYAxis}/>

              <label className="options_label" htmlFor="secondaryAxis">
                Left Y-axis:
              </label>
              <input type="text" id="secondaryAxis" onChange={handleSecondaryChange} value={options.lYAxis}/>
          
            </form>

            {input && input.sensors ?
              <div>
                <ViewMetadata 
                sensorIDs={input.sensors}
                show={show}
                handleClose={() => setShow(false)}
                />
                <div className="metadata_button" >
                  <GlobalButton primary={false} btnText="View Metadata" handleButtonClick={() => setShow(true)} >
                    <BiNews />
                  </GlobalButton>
                </div>

                
              </div>
            :
            <div />
            }

          </div>
        </div>
      
      </div>
    </div>
  )

}

export default AddCell;