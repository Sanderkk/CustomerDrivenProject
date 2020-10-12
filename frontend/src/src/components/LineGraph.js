import React, { useEffect, useState } from 'react';
import Highcharts from 'highcharts/highstock';
import HighchartsReact from 'highcharts-react-official';
import './componentStyles/LineGraph.css'
import { useSelector } from "react-redux";


const DisplayHighcharts = ({userOptions}) => {

   /*
    Component for creating the actual graph. The component listent to changes in redux store,
    and updates the graph whenever the data is changed in store. 

    :param Object(String) userOptions: object containing the title and y-axis names. 
    e.g: {title: 'Title', primaryAxis: 'Value', secondaryAxis: 'Value'}
  */

  const dataSeries = useSelector(state => state.queryData.response)
  const [graphdata, setGraphData] = useState([])

  useEffect(() => { 
    //Update the series displayed in the graph when state is updated in the store.
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
        text: userOptions.primaryAxis,
      },
    },
    // Secondary yAxis
    { 
      title: {
        text: userOptions.secondaryAxis,
      },
      labels: {
        format: '{value}',
      },
      opposite: false,
    }],
      // Data from the query builder.
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
