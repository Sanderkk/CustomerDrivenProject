import rootReducer from "../globalState/reducers/reducer";
import {
  setQueryData,
  setQueryDataInputAction,
  setQueryDataResponseAction,
  removeQueryDataAction,
} from "../globalState/actions/queryDataActions";

// Tests for testing the userReducer
describe("Test queryDataReducer", () => {
  // Dummy action to reducer should return initial state
  test("Initial state", () => {
    const action = { type: "dummy_action" };
    const userState = rootReducer(undefined, action).queryData;
    expect(userState).toEqual({});
  });

  // Test setting input
  test("setQueryDataInputAction", () => {
    const input = {
      tableName: "tension",
      columnNames: ["time", "analog_channel_3_rv010_133149"],
      from: "2020-08-25T22:40:00.000Z",
      to: "2020-08-25T22:43:00.000Z",
    };
    const queryDataState = rootReducer(
      undefined,
      setQueryDataInputAction(input)
    ).queryData;
    expect(queryDataState.input).toEqual(input);
  });

  // Test setting response
  test("setQueryDataResponseAction", () => {
    const response = {
      timescale: {
        data: [],
        time: [],
      },
    };
    const queryDataState = rootReducer(
      undefined,
      setQueryDataResponseAction(response)
    ).queryData;
    expect(queryDataState.response).toEqual(response);
  });

  // Test setting both input and response
  test("setQueryDataInputAction", () => {
    const input = {
      tableName: "",
      columnNames: ["time"],
    };
    const response = {
      response: {
        data: [1, 2, 3],
      },
    };
    const queryDataState = rootReducer(undefined, setQueryData(input, response))
      .queryData;
    expect(queryDataState).toEqual({ input, response });
  });

  // Test removing queryData
  test("removeUserAction", () => {
    const queryDataState = rootReducer(undefined, removeQueryDataAction())
      .queryData;
    expect(queryDataState).toEqual({});
  });
});
