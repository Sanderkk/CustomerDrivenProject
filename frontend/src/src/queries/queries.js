import { gql } from "@apollo/client";

export const GET_TABLE_AND_COLUMNS = gql`
  query getTableAndColumns {
    tableAndColumns {
      key
      value
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
