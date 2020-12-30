import React from "react";
import { render } from "react-dom";
import { MemoryRouter } from "react-router-dom";
import JobDetail from "../../../components/jobDetail/Index";

describe("JobDetail", () => {
  jest.mock("react-router-dom", () => ({
    ...jest.requireActual("react-router-dom"),
    useParams: () => ({
      name: "job-name"
    }),
    useRouteMatch: () => ({ url: "/jenkins/jobs/job-name" })
  }))

  it("renders job detail when page loaded", () => {
    const root = document.createElement("div");
    document.body.appendChild(root);

    render(
      <MemoryRouter initialEntries={["/jenkins/jobs/job-name"]}>
        <JobDetail />
      </MemoryRouter>,
      root
    );
  });
});
