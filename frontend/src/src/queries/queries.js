import { gql } from "@apollo/client";

export const GET_SENSORS = gql`
  query getSensors {
    sensors {
      sensorColumns
      sensorId
      sensorTabel
    }
  }
`;

export const GET_TIME_SERIES = gql`
  query TimeSeries($input: TimeSeriesRequestInput!) {
    timeSeries(input: $input) {
      table
      startDate
      endDate
      time
      data {
        key
        value
      }
      numberData {
        key
        value
      }
    }
  }
`;
