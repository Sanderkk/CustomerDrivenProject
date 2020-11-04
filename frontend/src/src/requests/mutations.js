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
`;
export const UPDATE_CELL = gql`
  mutation updateCell($input: CellInput) {
    updateCell(input: $input)
  }
`;

export const DELETE_DASHBOARD = gql`
  mutation deleteDashboard($dashboardId: Int!) {
    deleteDashboard(dashboardId: $dashboardId)
  }
`;

export const DELETE_CELL = gql`
  mutation deleteCell($dashboardId: Int!, $cellId: Int!) {
    deleteCell(dashboardId: $dashboardId, cellId: $cellId)
  }
`;

export const UPLOAD_DATA = gql`
  mutation uploadData($input: UploadDataInput!) {
    uploadData(input: $input)
  }
`;
