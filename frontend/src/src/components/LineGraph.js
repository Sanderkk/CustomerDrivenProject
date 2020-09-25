
import React, {useState} from 'react';
import Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import './componentStyles/LineGraph.css'

import { useSelector } from "react-redux";

const DisplayHighcharts = () => {
    const store = useSelector((store) => store);
    const [seriesArray, setSeriesArray] = useState([])
    const [time, setTime] = useState([])
    
    //returns the dates for the x-axis.
    const parseTimes = () => {
        let hours = []
        for(let i = 0; i < store.queryData.response.timeSeries.time.length; i++) {
            let dateObject = new Date(store.queryData.response.timeSeries.time[i])
            hours.push(dateObject)
            //hours.push(dateObject.toLocaleString())
        }
        setTime(hours)
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

    const getDataFromStore = () => {
        let numberData = store.queryData.response.timeSeries.numberData
        if (store.queryData.response !== undefined) {
            if(numberData.length > 0) {
            
            let dataFromStore = []
            for (let i = 0; i < numberData.length; i++) {
                dataFromStore.push({
                    name: numberData[i].key,
                    data: numberData[i].value,
                    yAxis: determineAxis(numberData[i].value[10])
                })
            }
            setSeriesArray(dataFromStore)

            //Converting the dates from string to date objects
            parseTimes() 
        }
    }


    }

    //Customizing the graph
    const options = {
        chart: {
            type: 'line',
            zoomType: 'x'
        },
        title: {
            text: "Metocean"
        },
        colors: ['#fcba03', '#03a1fc', '#d13b40', '#006600'],
        xAxis: [{ 
            type: "datetime",
            categories: time, //array of dates
            crosshair: true,
        }],
        // Primary yAxis
        yAxis: [{ 
            min: 0,
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
            min: 0,
            title: {
                text: 'Secondary y-axis',
            },
            labels: {
                format: '{value}',
            }
        }
    ],
        series: seriesArray // Data from the query builder.
    };

    return (
    <div className="chart_wrapper">
        <button type="button" onClick={getDataFromStore}>Create graph</button>
        <HighchartsReact highcharts={Highcharts} options={options}/>
    </div>
    );
    
}

export default DisplayHighcharts;
