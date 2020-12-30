import React from "react";
import images from "../../config/images";

const HealthReport = ({ data }) => {
  const getScoreStatusIcon = (iconUrl) => {
    switch (iconUrl) {
      case "health-00to19.png":
        return images.HealthRainy;
      case "health-20to39.png":
        return images.HealthMostlyCloudy;
      case "health-40to59.png":
        return images.HealthCloudy;
      case "health-60to79.png":
        return images.HealthPartlySunny;
      case "health-80plus.png":
        return images.HealthSunny;
      default:
        return "";
    }
  };
  return (
    <div className="card mt-4" data-testid="card">
      <div className="card-header" data-testid="card-header">
        Özet Bilgi
      </div>
      <div className="card-body">
        <ul
          className="list-group list-group-flush"
          data-testid="health-report-list">
          <li className="list-group-item d-flex pl-0 pr-0">
            <span className="mr-2">Uygulama : </span>
            <a
              href={`/application/detail/${data.applicationId}`}
              target="_blank"
              rel="noopener noreferrer">
              {data.applicationName}
            </a>
          </li>
          <li
            className="list-group-item d-flex pl-0 pr-0"
            data-testid="data-description">
            {data.description}
          </li>
          {data.healthReport &&
            data.healthReport.length > 0 &&
            data.healthReport.map((reportItem, index) => (
              <li
                key={index}
                className="list-group-item d-flex justify-content-between align-items-center pl-0 pr-0"
                data-testid="health-report-list-item">
                {reportItem.description}
                <span className="badge badge-pill">
                  <img
                    src={getScoreStatusIcon(reportItem.iconUrl)}
                    alt="score-status"
                    data-testid="health-report-status"
                  />
                </span>
              </li>
            ))}
        </ul>
      </div>
    </div>
  );
};

export default HealthReport;
