import React, {useState} from 'react'

import "./componentStyles/UploadData.css";

function UploadDataManually() {
    const [file, setFile] = useState(undefined)
    const [filename, setFilename] = useState('')
    const [sensorID, setSensorID] = useState(0)
  
  
    //Test values
    const filenames = ["filename1", "filename2", "filename3"]
    const sensornames = ["sensor1", "sensor2", "sensor3", "sensor4", "sensor5", "sensor6"]
  
  
    //Sets the file-state to be the user's inputted file.
    const handleFileChange = event => {
      event.preventDefault()
      setFile(event.target.files[0])
    }
    
    const handleFilenameChange = event => {
      setFilename(event.target.value)
    }
  
    const handleSensorChange = event => {
      setSensorID(event.target.value)
    }
  
    const handleSubmit = event => {
      //TO DO: Send data to api using endpoint.
      event.preventDefault()
    }
  
    return (
      //Form for user input.
      <div className="formWrapper">
        <h1 className="header">Upload Data Manually</h1>
        <form onSubmit={handleSubmit}>
          <label className="inputLabel" htmlFor="myFile">
            Upload file:
            <input type="file" id="myFile" name="filename" onChange={handleFileChange} className="chooseFileBtn"></input>
          </label>
          
          <label className="inputLabel" htmlFor="filenameDropdown">
            Existing filename to add sensor to:
            <select className="dropdown" id="filenameDropdown" value={filename} onChange={handleFilenameChange}>
              {filenames.map((filename) => 
                <option value={filename} key={filename}>{filename}</option>
              )}
            </select>
          </label>
  
          <label className="inputLabel" htmlFor="sensorDropdown">
            Sensor:
            {/*TO DO: Use endpoint to fetch sensors belonging to the filename chosen by the user.*/}
            <select className="dropdown" id="sensorDropdown" value={sensorID} onChange={handleSensorChange}>
            {sensornames.map((sensorname) => 
                <option value={sensorname} key={sensorname}>{sensorname}</option>
              )}
            </select>
          </label>

          <input type="submit" value="Upload" />
        </form>
      </div>
    );
  }

export default UploadDataManually