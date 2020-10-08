import React, { useEffect, useState } from 'react';
import Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import './componentStyles/LineGraph.css'

import { useSelector } from "react-redux";

//This component creates the graphs. The component listens to changes in the redux store, and
//whenever the response of queryData is changed, the graph is updated with these values. 
const DisplayHighcharts = () => {
    const dataSeries = useSelector(state => state.queryData.response)
    const [graphdata, setGraphData] = useState([])
    
    //TODO: remove this function when the data structure is changed in backend. 
    const zip = function(ar1, ar2, zipper) {
        return zipper
            ? ar1.map((value, index) => zipper(value, ar2[index]))
            : ar1.map((value, index) => [value, ar2[index]])
            ;
    }

    //Update the series displayed in the graph when state is updated in the store.
    useEffect(() => { 
        if(dataSeries !== undefined) {
            let numberData = dataSeries.timeSeries.numberData
                let dataFromStore = numberData.map((sensorData) => ({
                    name: sensorData.key,
                    data: sensorData.value,
                    yAxis: determineAxis(sensorData.value[10])
                }))
                //TODO: use this line instead of the ones underneath when the data structure is changed in backend.
                //setGraphData(dataFromStore)
                
                //TODO: remove these lines when the data structure is changed in backend. 
                const t = parseTimes()
                const d = transformData(dataFromStore, t)
                setGraphData(d)
            }
     }, [dataSeries]);


     //TODO: remove this function when the data structure is changed in backend. 
     const parseTimes = () => {
        let hours = []
        for(let i = 0; i < dataSeries.timeSeries.time.length; i++) {
            let dateObject = new Date(dataSeries.timeSeries.time[i]).getTime()
            hours.push(dateObject)
        }
        return hours
    }


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

    
    //TODO: remove this function when the data structure is changed in backend. 
    const transformData = (sensors, time) => {
        var listOfObjects = [];
        sensors.map(sensor => {
            var singleObj = {};
            singleObj['data'] = zip(time, sensor.data);
            singleObj['name'] = sensor.name;
            listOfObjects.push(singleObj);
        });
        return listOfObjects
    }

    //Customizing the graph
    const options = {
        chart: {
            type: 'line',
            zoomType: 'x',
        },
        title: {
            text: "Title"
        },
        colors: ['#004299', '#94f700', '#c70076', '#f7e818'],
        xAxis: [{ 
            type: 'datetime',
            title: {
                text: 'Date'
            },
        }],
        // Primary yAxis
        yAxis: [{ 
            labels: {
                format: '{value}',
            },
            title: {
                text: 'Primary y-axis',

            },
            opposite: true
        },
        // Secondary yAxis
         { 
            title: {
                text: 'Secondary y-axis',
            },
            labels: {
                format: '{value}',
            }
        }
    ],
        // Data from the query builder.
        series: graphdata 
    };

    return (
    <div className="chart_wrapper">
        <div>
            <HighchartsReact highcharts={Highcharts} options={options} />
        </div>
    </div>
    );
}

export default DisplayHighcharts;
