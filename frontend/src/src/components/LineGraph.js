import React, { useEffect, useState } from 'react';
import Highcharts from 'highcharts/highstock';
import HighchartsReact from 'highcharts-react-official';
import './componentStyles/LineGraph.css'
import { useSelector } from "react-redux";
import { GET_TIME_SERIES } from "../queries/queries";
import { useApolloClient } from "@apollo/client";
import sendQuery from "../queries/sendQuery";


const DisplayHighcharts = (props) => {

  /*
    Component for creating the actual graph. The component listent to changes in redux store,
    and updates the graph whenever the data is changed in store, which is done by the QueryBuilder.
    If input is passed as a prop, the component queries the specified data itself. 

    props:
      options: title and names of the Y-axises. 
      input: input for generating the query.
      cellId: unique id for the cell, generated at backend. 
  */

  const dataSeries = useSelector(store => store.queryData.response)
  const [dataFromBackend, setDataFromBackend] = useState({})
  const [graphdata, setGraphData] = useState([])
  const [userOptions, setUserOptions] = useState({title: '', rYAxis: '', lYAxis: ''})
  const client = useApolloClient();


  useEffect(() => {
    if(props.options !== undefined) {
      setUserOptions({title: props.options.title, 
        rYAxis: props.options.rYAxis, 
        lYAxis: props.options.lYAxis})
    }
  },[props.options]);


  //Update the series displayed in the graph when state is updated in the store. (QueryBuilder)
  useEffect(() => { 
    if(dataSeries !== undefined) {
      let numberData = dataSeries.timeSeries.data
      if(numberData !== undefined) {
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

  //Query data based on the input prop. 
  useEffect(() => {
    let mounted = true;
    if (props.input !== undefined) {
      const input = props.input
      
      if(mounted){
      sendQuery(client, GET_TIME_SERIES, { input })
        .then((result) => setDataFromBackend(result.data.timeSeries.data))
        .catch((err) => console.log(err));
      }

      if(dataFromBackend.length > 0) {
      let dataFromStore = dataFromBackend.map((sensorData) => ({
        name: sensorData.name,
        data: sensorData.data,
        pointStart: sensorData.startTime / 10000,
        pointInterval: sensorData.interval / 10000,
        yAxis: determineAxis(sensorData.data[10])
      }))
      setGraphData(dataFromStore) 
      }
    }
    return () => mounted = false;
  }, [client, props.input, dataFromBackend]);


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

  //Customizing the graph
  const options = {
    chart: {
      type: 'line',
      zoomType: 'x',
      animation: false,
    },
    
    title: {
      text: userOptions.title
    },
    colors: ['#004299', '#94f700', '#c70076', '#f7e818'],
    xAxis: [{ 
      type: 'datetime',
      title: {
          text: 'Date'
      },
    }],
    rangeSelector: {
      buttons: [
      {
        type: 'hour',
        count: 1,
        text: '1h'
      }, {
        type: 'day',
        count: 1,
        text: '1d'
      }, {
        type: 'week',
        count: 1,
        text: '1w'
      }, {
        type: 'month',
        count: 1,
        text: '1m'
      }, {
        type: 'month',
        count: 6,
        text: '6m'
      }, {
        type: 'year',
        count: 1,
        text: '1y'
      }, {
        type: 'all',
        text: 'All'
      }]
    },

    // Primary yAxis
    yAxis: [{ 
      labels: {
        format: '{value}',
      },
      title: {
        text: userOptions.rYAxis,
      },
    },
    // Secondary yAxis
    { 
      title: {
        text: userOptions.lYAxis,
      },
      labels: {
        format: '{value}',
      },
      opposite: false,
    }],
      series: graphdata 
  };

  return (
  <div className="chart_wrapper">
    <div>
      <HighchartsReact highcharts={Highcharts} options={options} constructorType={"stockChart"} />
    </div>
  </div>
  );
}

export default DisplayHighcharts;
