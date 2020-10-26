import React from "react";
import "./componentStyles/Profile.css";
import groupTypes from "../groupTypes";
import { useHistory } from "react-router";
import { AzureAD } from "react-aad-msal";
import { authProvider } from "../authProvider";
import store from "../globalState/store";
import GlobalButton from "./globalComponents/GlobalButton";
import { BiLogOut } from "react-icons/bi";

function Profile() {
  let history = useHistory();

  return (
    <AzureAD provider={authProvider} reduxStore={store}>
      {({ logout, accountInfo }) => (
        <div className="dropdown">
          <React.Fragment>
            <div className="dropbtn">{accountInfo.account.name}</div>
            <div className="dropdown-content">
              <div>
                <span>Email:</span> {accountInfo.account.userName}
              </div>
              <div>
                <h5>Groups</h5>
                {accountInfo.account.idToken.groups.indexOf(
                  groupTypes.researcher
                ) >= 0 && <li>Researcher</li>}
                {accountInfo.account.idToken.groups.indexOf(
                  groupTypes.engineer
                ) >= 0 && <li>Engineer</li>}
                {accountInfo.account.idToken.groups.indexOf(
                  groupTypes.customer
                ) >= 0 && <li>Customer</li>}
              </div>
              <div>
                <GlobalButton
                  primary={true}
                  btnText="Sign out"
                  handleButtonClick={() => {
                    history.push("/"); // route back to front page
                    logout();
                  }}
                >
                  <BiLogOut />
                </GlobalButton>
              </div>
            </div>
          </React.Fragment>
        </div>
      )}
    </AzureAD>
  );
}

export default Profile;
