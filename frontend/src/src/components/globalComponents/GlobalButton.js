import React from 'react';
import "../componentStyles/globalStyles/GlobalButton.css";

//This component is a global button. It is customizable with different icons, text and two colors. The color is decided by the primary prop. 
//If primary is true, the color is blue. If primary is false, the color is grey. 
function GlobalButton(props) {

    return(
        <button type="button" className={props.primary ? "global_button_primary" : "global_button_secondary"} onClick={props.handleButtonClick}>
            <div className="flex_wrapper">
                    {props.children} 
                <div className="btn_text"> 
                    {props.btnText}
                </div>
            </div>
            
        </button>
    )
}

export default GlobalButton;