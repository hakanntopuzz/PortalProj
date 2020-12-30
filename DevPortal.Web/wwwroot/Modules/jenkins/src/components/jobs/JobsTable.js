import React, { Fragment } from "react";
import { Link } from "react-router-dom";
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

const renderDetailColumn = (name) => {
  return (
    <Link to={`/jobs/${name}`} className="btn-link mx-auto text-center">
      <MDBIcon icon="eye" />
    </Link>
  );
};

const renderApplicationNameColumn = (id, name) => {
  if (id !== 0) {
    return (
      <a
        href={`/application/detail/${id}`}
        className="btn-link mx-auto text-center"
        target="_blank"
        rel="noopener noreferrer">
        {name}
      </a>
    );
  } else {
    return <span></span>;
  }
};

const JobsTable = ({ data }) => {
  const jobs = data.sort((a, b) => a - b);
  const columns = [
    {
      label: "",
      field: "color",
      sort: "disabled",
    },
    {
      label: "Uygulama Adı",
      field: "applicationName",
      sort: "disabled",
      width: 150,
    },
    {
      label: "Görev Adı",
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
      jobs &&
      jobs.length > 0 &&
      jobs.map((job, index) => ({
        color: renderStatusColumn(job.color),
        applicationName: renderApplicationNameColumn(
          job.applicationId,
          job.applicationName
        ),
        name: job.name,
        url: job.url,
        detail: renderDetailColumn(job.name),
      })),
  };

  const csvHeaders = [
    {
      label: "Durum",
      key: "color",
    },
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
        <AddToFavouriteButton pageTitle="Jenkins Görevleri"/>
        <OtherOperations
        headers={csvHeaders}
        data={jobs}
        filename="jenkins-gorevleri.csv"
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

export default JobsTable;
