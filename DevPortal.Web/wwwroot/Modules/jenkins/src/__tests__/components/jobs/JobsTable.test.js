import React from "react";
import { render, cleanup } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import JobsTable from "../../../components/jobs/JobsTable";

afterEach(cleanup);

test("renders jobs table", () => {
  const data = [{ id: 1, color: "blue", name: "job name", url: "job-url" }];

  const { getByRole, getByTestId, asFragment } = render(
    <MemoryRouter initialEntries={["/jenkins/jobs"]}>
      <JobsTable data={data} />
    </MemoryRouter>
  );  
  
  const table = getByRole("table");
  const statusColumn = getByTestId("statusColumn");

  expect(asFragment).toMatchSnapshot();
  expect(table).toBeInTheDocument();
  expect(table.classList.contains("table")).toBe(true);
  expect(statusColumn).toBeInTheDocument();
});
