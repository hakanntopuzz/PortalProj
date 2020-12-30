import React from "react";
import { render } from "@testing-library/react";
import HealthReport from "../../../components/jobDetail/HealthReport";
import HealthRainy from "../../../img/health-00to19.png";
import HealthMostlyCloudy from "../../../img/health-20to39.png";
import HealthCloudy from "../../../img/health-40to59.png";
import HealthPartlySunny from "../../../img/health-60to79.png";
import HealthSunny from "../../../img/health-80plus.png";

test("renders jenkins jobs health report", () => {
  const data = {
    description: "description",
    healthReport: [
      {
        description: "",
        iconUrl: "",
      },
    ],
  };

  const { getByTestId } = render(<HealthReport data={data} />);

  const card = getByTestId("card");
  const cardHeader = getByTestId("card-header");
  const dataDescription = getByTestId("data-description");
  const healthReportList = getByTestId("health-report-list");
  const healthReportListItem = getByTestId("health-report-list-item");

  expect(card).toBeInTheDocument();
  expect(cardHeader).toBeInTheDocument();
  expect(dataDescription).toBeInTheDocument();
  expect(dataDescription.innerHTML).toBe(data.description);
  expect(healthReportList).toBeInTheDocument();
  expect(healthReportListItem).toBeInTheDocument();
  expect(healthReportListItem.innerHTML).toContain(
    data.healthReport[0].description
  );
});

test("health report icon url is `health-00to19.png` and return image src is `images.HealthRainy`", () => {
  const data = {
    description: "description",
    healthReport: [
      {
        description: "health-description",
        iconUrl: "health-00to19.png",
      },
    ],
  };

  const { getByTestId } = render(<HealthReport data={data} />);
  const healthReportStatus = getByTestId("health-report-status");
  expect(healthReportStatus.getAttribute("src")).toBe(HealthRainy);
});

test("health report icon url is `health-20to39.png` and return image src is `images.HealthMostlyCloudy`", () => {
  const data = {
    description: "description",
    healthReport: [
      {
        description: "health-description",
        iconUrl: "health-20to39.png",
      },
    ],
  };

  const { getByTestId } = render(<HealthReport data={data} />);
  const healthReportStatus = getByTestId("health-report-status");
  expect(healthReportStatus.getAttribute("src")).toBe(HealthMostlyCloudy);
});

test("health report icon url is `health-40to59.png` and return image src is `images.HealthCloudy`", () => {
  const data = {
    description: "description",
    healthReport: [
      {
        description: "health-description",
        iconUrl: "health-40to59.png",
      },
    ],
  };

  const { getByTestId } = render(<HealthReport data={data} />);
  const healthReportStatus = getByTestId("health-report-status");
  expect(healthReportStatus.getAttribute("src")).toBe(HealthCloudy);
});

test("health report icon url is `health-60to79.png` and return image src is `images.HealthPartlySunny`", () => {
  const data = {
    description: "description",
    healthReport: [
      {
        description: "health-description",
        iconUrl: "health-60to79.png",
      },
    ],
  };

  const { getByTestId } = render(<HealthReport data={data} />);
  const healthReportStatus = getByTestId("health-report-status");
  expect(healthReportStatus.getAttribute("src")).toBe(HealthPartlySunny);
});

test("health report icon url is `health-80plus.png` and return image src is `images.HealthSunny`", () => {
  const data = {
    description: "description",
    healthReport: [
      {
        description: "health-description",
        iconUrl: "health-80plus.png",
      },
    ],
  };

  const { getByTestId } = render(<HealthReport data={data} />);
  const healthReportStatus = getByTestId("health-report-status");
  expect(healthReportStatus.getAttribute("src")).toBe(HealthSunny);
});
