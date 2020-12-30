import { renderHook, cleanup } from "@testing-library/react-hooks";
import axios from "axios";
import MockAdapter from "axios-mock-adapter";
import useFetch from "../hooks/useFetch";

afterEach(cleanup);

test("useFetch performs GET request", async () => {
  const initialValue = [];
  const mock = new MockAdapter(axios);

  const mockData = "response";
  const url = "api-url";
  mock.onGet(url).reply(200, mockData);

  const { result, waitForNextUpdate } = renderHook(() =>
    useFetch(url, initialValue)
  );

  expect(result.current.data).toEqual([]);
  expect(result.current.loading).toBeTruthy();

  await waitForNextUpdate();

  expect(result.current.data).toEqual("response");
  expect(result.current.loading).toBeFalsy();
});

test("useFetch sets loading to false and returns inital value on network error", async () => {
  const mock = new MockAdapter(axios);

  const initialValue = [];
  const url = "http://mock";

  mock.onGet(url).networkError();

  const { result, waitForNextUpdate } = renderHook(() =>
    useFetch(url, initialValue)
  );

  expect(result.current.data).toEqual([]);
  expect(result.current.loading).toBeTruthy();

  await waitForNextUpdate();

  expect(result.current.loading).toBeFalsy();
  expect(result.current.data).toEqual([]);
});
