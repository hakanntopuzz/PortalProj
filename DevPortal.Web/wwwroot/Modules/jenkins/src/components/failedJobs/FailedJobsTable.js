import React, { Fragment } from "react";
import "@fortawesome/fontawesome-free/css/all.min.css";
import "bootstrap-css-only/css/bootstrap.min.css";
import { MDBDataTable, MDBIcon } from "mdbreact";
import "../../styles/dataTable.css";
import OtherOperations from "../shared/dataTable/OtherOperations";
import AddToFavouriteButton from "../shared/AddToFavouriteButton";

const renderStatusColumn = (color) => {
  return (
    <div data-testid="statusColumn" className={`mx-auto status ${color}`}></div>
  );
};

const renderDetailColumn = (url) => {
  return (
    <a
      href={url}
      target="_blank"
      rel="noopener noreferrer"
      data-testid="detail-link"
      className="btn-link mx-auto text-center">
      <MDBIcon icon="eye" />
    </a>
  );
};

const FailedJobsTable = ({ data }) => {
  const jobs = data.sort((a, b) => a - b);
  const columns = [
    {
      label: "",
      field: "color",
      sort: "disabled",
    },
    {
      label: "Ad",
      field: "name",
      sort: "asc",
      width: 150,
    },
    {
      label: "Url",
      field: "url",
      sort: "asc",
      width: 150,
    },
    {
      label: "",
      field: "detail",
      sort: "disabled",
    },
  ];
  const tableData = {
    columns: columns,
    rows:
      data &&
      jobs.length > 0 &&
      jobs.map((job, index) => ({
        color: renderStatusColumn(job.color),
        name: job.name,
        url: job.url,
        detail: renderDetailColumn(job.url),
      })),
  };

  const csvHeaders = [
    {
      label: "Ad",
      key: "name",
    },
    {
      label: "Url",
      key: "url",
    },
  ];

  return (
    <Fragment>
      <div className="d-flex justify-content-end">
        <AddToFavouriteButton pageTitle="Jenkins Başarısız Görevler" />
        <OtherOperations
          headers={csvHeaders}
          data={jobs}
          filename="jenkins-basarisiz-gorevler.csv"
        />
      </div>
      <MDBDataTable
        searchLabel="Ara"
        paginationLabel={["Geri", "İleri"]}
        entriesLabel="Göster"
        infoLabel={["Gösterilen içerik", "-", "/", ""]}
        striped
        bordered
        responsive
        data={tableData}
        className="dev-portal-jobs-table"
      />
    </Fragment>
  );
};

export default FailedJobsTable;
