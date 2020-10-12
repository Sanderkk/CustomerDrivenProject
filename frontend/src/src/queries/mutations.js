import { gql } from "@apollo/client";

export const EDIT_METADATA = gql`
  mutation editMetadata($input: MetadataInput!) {
    addMetadata(newMetadata: $input) {
      sensorID
    }
  }
`;

export const UPDATE_DASHBOARD = gql`
  mutation updateDashboard($input: DashboardInput!) {
    updateDashboard(input: $input)
  }  
`
export const UPDATE_CELL = gql`
    mutation updateCell($input: CellDataInput!) {
        updateCell(input: $input)
    }
`

export const DELETE_DASHBOARD = gql`
    mutation deleteDashboard($userId: String!, $dashboardId: Int!) {
        deleteDashboard(userId: $userId, dashboardId: $dashboardId)
    }
`

export const DELETE_CELL = gql`
    mutation deleteCell($userId: String!, $dashboardId: Int!, $cellId: Int!) {
        deleteCell(userId: $userId, dashboardId: $dashboardId, cellId: $cellId)
    }
`