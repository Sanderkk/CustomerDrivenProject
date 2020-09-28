const initialState = {
  aadResponse: null,
};

const userReducer = (state = initialState, action) => {
  switch (action.type) {
    case "AAD_LOGIN_SUCCESS":
      return { ...state, aadResponse: action.payload };
    case "AAD_LOGOUT_SUCCESS":
      return { ...state, aadResponse: null };
    default:
      return state;
  }
};

export default userReducer;
