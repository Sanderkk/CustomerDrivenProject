import createApolloLink from "./createApolloLink";

async function sendQuery(
  client,
  query,
  variables,
  fetchPolicy = "network-only"
) {
  //Function used to send queries to back end.
  //Requires client, query and variables as input, is variables is nothing then input null for the variables parameter

  client.setLink(await createApolloLink());

  return await client.query({
    query: query,
    variables: variables,
    fetchPolicy: fetchPolicy,
  });
}

export default sendQuery;
