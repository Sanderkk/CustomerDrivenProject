import { gql } from "@apollo/client";

export const GET_SENSORS = gql`
  query getSensors {
    sensors {
      sensorColumns
      sensorTypeName
      sensorIds
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
      metadataID
      number
      actualDisposal
      altitude
      cableLength
      checkOnInspectionRound
      company
      coordinate
      createdAt
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
      outdatedFrom
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
  query dashboard($userId: String!, $dashboardId: Int!) {
    userDashboard(userId: $userId, dashboardId: $dashboardId) {
      id
      name
      description
    }
  }
`;

export const GET_DASHBOARDS = gql`
  query dashboards($userId: String!) {
    userDashboard(userId: $userId) {
      id
      name
      description
    }
  }
`;

export const GET_DASHBOARD_CELL = gql`
  query cell($userId: String!, $dashboardId: Int!, $cellId: Int!) {
    cells(userId: $userId, dashboardId: $dashboardId, cellId: $cellId) {
      id
      options
      input
    }
  }
`;

export const GET_DASHBOARD_CELLS = gql`
  query cells($userId: String!, $dashboardId: Int!) {
    cells(userId: $userId, dashboardId: $dashboardId) {
      id
      options
      input
    }
  }
`;

export const UPDATE_DASHBOARD = gql`
  mutation updateDashboard($input: DashboardInput!) {
    updateDashboard(input: $input)
  }
`;
export const UPDATE_CELL = gql`
  mutation updateCell($input: CellDataInput!) {
    updateCell(input: $input)
  }
`;

export const DELETE_DASHBOARD = gql`
  mutation deleteDashboard($userId: String!, $dashboardId: Int!) {
    deleteDashboard(userId: $userId, dashboardId: $dashboardId)
  }
`;

export const DELETE_CELL = gql`
  mutation deleteCell($userId: String!, $dashboardId: Int!, $cellId: Int!) {
    deleteCell(userId: $userId, dashboardId: $dashboardId, cellId: $cellId)
  }
`;
