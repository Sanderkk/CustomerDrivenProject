import { gql } from "@apollo/client";

export const GET_SENSORS = gql`
  query getSensors {
    sensors {
      sensorColumns
      sensorTypeName,
      sensorIds,
    }
  }
`;

export const GET_TIME_SERIES = gql`
  query TimeSeries($input: TimeSeriesRequestInput!) {
    timeSeries(input: $input) {
      table,
      startDate,
      endDate,
      data {
        name,
        startTime,
        interval,
        data
      },
      time
    }
  }
`;

export const GET_LAST_METADATA = gql`
  query lastMetadata($sensorID: Int!) {
    lastMetadata(sensorID: $sensorID) {
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

export const GET_USER_DASHBOARDS = gql`
  query userDashboards($userId: String!) {
    userDashboards(userId: $userId) {
        id,
        name,
        description,
        data
    }
  }
`;

export const UPDATE_DASHBOARD = gql`
  mutation updateDashboard($input: DashboardInput!) {
    updateDashboard(input: $input)
  }  
`