import { gql } from "@apollo/client";

export const GET_SENSORS = gql`
  query getSensors {
    sensors {
      sensorColumns
      sensorTypeName
      sensorIds
      sensorNumbers
    }
  }
`;

export const GET_TIME_SERIES = gql`
  query TimeSeries($input: TimeSeriesRequestInput!) {
    timeSeries(input: $input) {
      table
      startDate
      endDate
      data {
        name
        startTime
        interval
        data
      }
      time
    }
  }
`;

export const GET_METADATA = gql`
  query metadata($sensorID: Int, $onlyLast: Boolean, $sensorNumber: String) {
    metadata(
      sensorID: $sensorID
      onlyLast: $onlyLast
      sensorNumber: $sensorNumber
    ) {
      sensorID
      number
      actualDisposal
      altitude
      cableLength
      checkOnInspectionRound
      company
      coordinate
      department
      identificator
      inspectionRound
      lending
      lendingPrice
      locationDescription
      measureArea
      modelNumber
      name
      nextService
      ownerID
      picture
      plannedDisposal
      purchaseDate
      serialNumber
      servicePartner
      signal
      tag1
      tag2
      tag3
      timeless
      tollerance
      updatedAt
      voltage
      warrantyDate
      website
    }
  }
`;

export const GET_METADATA_SENSOR_NUMBERS = gql`
  query getMetadataSensors {
    metadata(onlyLast: true) {
      number
      sensorID
    }
  }
`;

export const GET_DASHBOARD = gql`
  query dashboard($dashboardId: Int!) {
    dashboard(dashboardId: $dashboardId) {
      dashboardId
      name
      description
    }
  }
`;

export const GET_DASHBOARDS = gql`
  query dashboards {
    dashboards {
      dashboardId
      name
      description
    }
  }
`;

export const GET_DASHBOARD_CELL = gql`
  query cell($dashboardId: Int!, $cellId: Int!) {
    cells(dashboardId: $dashboardId, cellId: $cellId) {
      cellId
      input {
        from
        to
        specifiedTimePeriod
        sensors
      }
      options {
        lYAxis
        rYAxis
        title
      }
    }
  }
`;

export const GET_DASHBOARD_CELLS = gql`
  query cells($dashboardId: Int!) {
    cells(dashboardId: $dashboardId) {
      cellId
      input {
        from
        to
        specifiedTimePeriod
        sensors
      }
      options {
        lYAxis
        rYAxis
        title
      }
    }
  }
`;
