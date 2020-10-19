import { gql } from "@apollo/client";

export const EDIT_METADATA = gql`
  mutation editMetadata($input: MetadataInput!) {
    addMetadata(newMetadata: $input) {
      sensorID
    }
  }
`;
