import React from "react";
import { render } from "react-dom";
import { MemoryRouter } from "react-router-dom";
import FailedJobList from "../../../components/failedJobs/Index";

describe("FailedJobList", () => {
  it("renders failed job list on table when page loaded", () => {
    const root = document.createElement("div");
    document.body.appendChild(root);
    
    render(
      <MemoryRouter initialEntries={["/jenkins/failed-jobs"]}>
        <FailedJobList />
      </MemoryRouter>,
      root
    );
  });
});