import { createHttpLink } from "@apollo/client";
import { setContext } from "@apollo/client/link/context";
import { authProvider } from "../authProvider";

async function createApolloLink() {
  //Function used by sendQuery and sendMutation
  //returns the link to be used by the apollo client, with the correct access token

  // Get the logged in user's access token. Either from cache or by requesting it from Azure AD
  const token = await authProvider.getAccessToken();

  const authLink = setContext((_, { headers }) => {
    // return the headers to the context so httpLink can read them
    return {
      headers: {
        ...headers,
        authorization: token ? `Bearer ${token}` : "",
      },
    };
  });

  const httpLink = createHttpLink({
    uri: "http://api-customerdriven.sanderkk.com/playground/..",
  });

  return authLink.concat(httpLink);
}

export default createApolloLink;
