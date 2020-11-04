import createApolloLink from "./createApolloLink";

async function sendMutation(client, mutation, variables) {
  //Function used to send mutations to back end.
  //Requires client, query and variables as input, is variables is nothing then input null for the variables parameter

  client.setLink(await createApolloLink());

  return await client.mutate({
    mutation: mutation,
    variables: variables,
  });
}

export default sendMutation;
