import React, { useState } from "react";
import { UPLOAD_DATA } from "../../queries/mutations";
import sendMutation from "../../queries/sendMutation";
import { useApolloClient } from "@apollo/client";
import "../componentStyles/admin/UploadData.css";

function UploadDataManually() {
  const [files, setFiles] = useState({});
  const [render, setRender] = useState({
    loading: false,
    error: false,
    success: false,
  });
  const client = useApolloClient();

  // onChange function that reads files on uploading them, files read are encoded as Base64
  function onFileUpload(event) {
    event.preventDefault();
    const id = event.target.id; // file id
    const file_reader = new FileReader();
    const file = event.target.files[0];
    setRender({
      loading: false,
      error: false,
      success: false,
    });
    if (file === undefined) {
      // remove file from state if unselected
      setFiles((prevState) =>
        Object.keys(prevState).reduce(
          (acc, val) =>
            val === id
              ? acc
              : {
                  ...acc,
                  [val]: prevState[val],
                },
          {}
        )
      );
      return;
    }
    file_reader.onload = () => {
      // After uploading the file => appending encoded (base64) file to state
      setFiles((prevState) => ({
        ...prevState,
        [id]: {
          uploaded_file: file_reader.result,
        },
      }));
    };
    file_reader.readAsDataURL(file); // reading the uploaded file
  }

  // handle submit button for form
  function handleSubmit(e) {
    e.preventDefault();
    const input = {
      input: {
        encodedData: files[1].uploaded_file.split(",")[1],
        encodedConfig: files[2].uploaded_file.split(",")[1],
      },
    };
    setRender({
      loading: true,
      error: false,
      success: false,
    });
    sendMutation(client, UPLOAD_DATA, input)
      .then((res) =>
        setRender({
          loading: false,
          error: false,
          success: true,
        })
      )
      .catch((err) =>
        setRender({
          loading: false,
          error: true,
          success: false,
        })
      );
  }

  return (
    //Form for user input.
    <div className="upload_container">
      <form onSubmit={handleSubmit} className="form_container">
        <h3> Upload Sensor Data Manually </h3>
        <div className="upload_button">
          <label>Data file:</label>
          <input
            onChange={onFileUpload}
            id={1}
            accept=".txt, .csv"
            type="file"
          />
        </div>
        <div className="upload_button">
          <label>Config file:</label>
          <input onChange={onFileUpload} id={2} accept=".json" type="file" />
        </div>
        {Object.keys(files).length === 2 ? (
          <button type="submit">Upload</button>
        ) : (
          <button disabled type="submit">
            Select files
          </button>
        )}
        <div>
          {render.error
            ? "Error: Config file is wrong or data is already uploaded"
            : React.Fragment}
          {render.loading ? "Uploading..." : React.Fragment}
          {render.success ? "Success: Data uploaded" : React.Fragment}
        </div>
      </form>
    </div>
  );
}

export default UploadDataManually;
