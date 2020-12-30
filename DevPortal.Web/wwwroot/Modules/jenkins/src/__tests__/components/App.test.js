import React from "react";
import { cleanup} from "@testing-library/react";
import { render } from "react-dom";
import { MemoryRouter } from "react-router-dom";
import App from "../../components/App";

afterEach(cleanup);

it("navigates home start app", () => {
  const root = document.createElement("div");
  document.body.appendChild(root);

  render(
    <MemoryRouter initialEntries={["/"]}>
      <App />
    </MemoryRouter>,
    root
  );

  expect(window.location.pathname).toBe("/");
});
