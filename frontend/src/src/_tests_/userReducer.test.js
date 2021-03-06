import rootReducer from "../globalState/reducers/reducer";
import {
  setUserAction,
  removeUserAction,
} from "../globalState/actions/userActions";

// Tests for testing the userReducer
describe("Test userReducer", () => {
  // Dummy action to reducer should return initial state
  test("Initial state", () => {
    const action = { type: "dummy_action" };
    const userState = rootReducer(undefined, action).user;
    expect(userState).toEqual({ aadResponse: null });
  });

  // Test setting a user
  test("setUserAction", () => {
    const user = { name: "John Doe", userName: "johndoe@sintef.no", age: 21 };
    const userState = rootReducer(undefined, setUserAction(user)).user;
    expect(userState).toEqual({
      aadResponse: {
        user: { name: "John Doe", userName: "johndoe@sintef.no", age: 21 },
      },
    });
  });

  // Test removing a user
  test("removeUserAction", () => {
    const userState = rootReducer(undefined, removeUserAction()).user;
    expect(userState).toEqual({ aadResponse: null });
  });
});
