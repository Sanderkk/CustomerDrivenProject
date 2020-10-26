import React, { useState, useEffect  } from "react";
import Navbar from "./Navbar";
import LineGraph from "./LineGraph";
import QueryBuilder from "./QueryBuilder";
import GlobalButton from "./globalComponents/GlobalButton";
import { BiCheck } from "react-icons/bi";
import { BiX } from "react-icons/bi";
import ViewMetadata from "./globalComponents/ViewMetadata";
import { useSelector } from "react-redux";
import { setQueryData } from "../globalState/actions/queryDataActions";
import { useDispatch } from "react-redux";
import sendQuery from "../queries/sendQuery";
import { useApolloClient } from "@apollo/client";
import { GET_TIME_SERIES } from "../queries/queries";
import { UPDATE_CELL } from "../queries/mutations";
import { Link } from "react-router-dom";
import sendMutation from "../queries/sendMutation";

import './componentStyles/AddCell.css'

//This component presents a page to the user where he/she can add a new cell
//or modify an existing cell. 
function AddCell(props) {
  const state = props.location.state;
  const [options, setOptions] = useState(state !== undefined ? state.options : { title: '', RYAxis: '', LYAxis: ''})
  const [show, setShow] = useState(false);
  const input = useSelector(store => store.queryData.input)
  const dashboard = useSelector(store => store.currentDashboard.input)
  const queryData = useSelector(store => store.queryData)
  const user = useSelector((store) => store.user.aadResponse);
  const dispatch = useDispatch();
  const client = useApolloClient();

  
  // Update global state if editing a graph/cell
  useEffect(() => {
    if(state !== undefined){
      // fetch time series and update global state
      const input = state.input
      sendQuery(client, GET_TIME_SERIES, { input })
        .then((result) => dispatch(setQueryData(input, result.data)))
        .catch((err) => console.log(err));
    }
  }, [client, state]);


  const handleDiscard = () => {
    //TODO: Discard cell
  }

  const handleAddCell = () => {
    //Check if any of the input-fields have been changed. 
    const isEmpty = Object.values(options).every(x => (x === ''));
    console.log(isEmpty)
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
    setOptions({...options, RYAxis: e.target.value})
  }

  const handleSecondaryChange = e => {
    e.preventDefault()
    setOptions({...options, LYAxis: e.target.value})
  }

  return(
    <div>
      <Navbar />
      <div className="outer_flex_container">
        <div className="add_cell_buttons">
            
          <div className="discard_cell" >
            <Link to="/specific-dashboard">
              <GlobalButton primary={false} btnText="Discard" handleButtonClick={handleDiscard}>
                <BiX />
              </GlobalButton>
            </Link>
          </div>
          <div className="add_cell" >
            <GlobalButton primary={true} btnText="Save Cell" handleButtonClick={handleAddCell}>
              <BiCheck />
            </GlobalButton>
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
              <input type="text" id="primaryAxis" onChange={handlePrimaryChange} value={options.RYAxis}/>

              <label className="options_label" htmlFor="secondaryAxis">
                Left Y-axis:
              </label>
              <input type="text" id="secondaryAxis" onChange={handleSecondaryChange} value={options.LYAxis}/>
          
            </form>

            {input && input.sensors ?
              <div>
                <ViewMetadata 
                sensorIDs={input.sensors}
                show={show}
                handleClose={() => setShow(false)}
                />
                {/* TODO: gjør knappen pen */}
                <button type="button" onClick={() => setShow(true)}>
                  Open
                </button>
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