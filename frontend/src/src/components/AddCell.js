import React from "react";
import Navbar from "./Navbar";
import LineGraph from "./LineGraph";
import QueryBuilder from "./QueryBuilder";
import GlobalButton from "./globalComponents/GlobalButton";
import { BiCheck } from "react-icons/bi";
import { BiX } from "react-icons/bi";

import './componentStyles/AddCell.css'

//This component presents a page to the user where he/she can add a new cell
//or modify an existing cell. 
function AddCell() {

    const handleDiscard = () => {
        //TODO: Discard cell
    }

    const handleAddCell = () => {
        //TODO: Add cell
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
                            <LineGraph />
                        </div>
                        <div className="query_builder">
                            <QueryBuilder />
                        </div>
                    </div>
                
                    <div className="options">
                        <h2>Options</h2>
                    </div>
                </div>
            
            </div>
        </div>
    )

}

export default AddCell;