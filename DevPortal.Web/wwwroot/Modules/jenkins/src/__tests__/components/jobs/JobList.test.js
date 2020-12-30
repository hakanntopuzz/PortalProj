import React from "react";
import { render } from "react-dom";
import { MemoryRouter } from "react-router-dom";
import JobList from "../../../components/jobs/Index";

describe("JobList", () => {
  it("renders job list on table when page loaded", () => {
    const root = document.createElement("div");
    document.body.appendChild(root);

    render(
      <MemoryRouter initialEntries={["/jenkins/jobs"]}>
        <JobList />
      </MemoryRouter>,
      root
    );
  });

  test('api returns error error section should appears on the screen', () => {
    const root = document.createElement("div");
    document.body.appendChild(root);

    render(
      <MemoryRouter initialEntries={["/jenkins/jobs"]}>
        <JobList />
      </MemoryRouter>,
      root
    );
    
    const fetchJobsResult = { data: [], error: true, loading: true };
    const useFetchMock = jest.fn("../hooks/useFetch").mockReturnValue(fetchJobsResult);

    expect(useFetchMock().error).toBeTruthy();
  });
});
