import React from "react";
import { render } from "@testing-library/react";
import FailedJobsTable from "../../../components/failedJobs/FailedJobsTable";

test("renders failed jobs table", () => {
  const data = [
    { Name: "mxkobi", Url: "mx-kobi" },
    { Name: "mxsocial", Url: "mx-social" },
  ];

  const { getByRole, getAllByTestId } = render(<FailedJobsTable data={data} />);
  const table = getByRole("table");
  const detailLink = getAllByTestId("detail-link");

  expect(table).toBeInTheDocument();
  expect(table.classList.contains("table")).toBe(true);

  expect(detailLink[0].getAttribute("rel")).toBe("noopener noreferrer");
});
