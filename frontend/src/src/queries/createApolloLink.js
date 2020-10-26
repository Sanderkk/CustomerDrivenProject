import { createHttpLink } from "@apollo/client";
import { setContext } from "@apollo/client/link/context";
import { authProvider } from "../authProvider";
import store from "../globalState/store";

async function createApolloLink() {
  //Function used by sendQuery and sendMutation
  //returns the link to be used by the apollo client, with the correct access token

  // Get the logged in user's access token. Either from redux store or by requesting it from Azure AD
  var token = store.getState().user.accessToken;
  if(store.getState().user.accessToken == null || token.expiresOn.getTime() < Date.now()){
    // request new accessToken from Azure AD if redux store does not have an already existing accessToken 
    // or if the existing access token is expired 
    token = await authProvider.getAccessToken();
  }

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
    uri: "http://customerdriven.sanderkk.com:5000/",
  });

  return authLink.concat(httpLink);
}

export default createApolloLink;
