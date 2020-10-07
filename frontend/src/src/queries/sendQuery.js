import createApolloLink from "./createApolloLink";

async function sendQuery(client, query, variables) {
  //Function used to send queries to back end.
  //Requires client, query and variables as input, is variables is nothing then input null for the variables parameter

  client.setLink(await createApolloLink());

  return await client.query({
    query: query,
    variables: variables,
  });
}

export default sendQuery;
