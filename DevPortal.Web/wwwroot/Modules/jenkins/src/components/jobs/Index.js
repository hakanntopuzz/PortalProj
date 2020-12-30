import React, { Fragment } from "react";
import useFetch from "../../hooks/useFetch";
import { JOBS_URL } from "../../config/urls";
import initialState from "../../config/initialState";
import Breadcrumb from "../shared/Breadcrumb";
import JobsTable from "./JobsTable";
import Loader from "../shared/Loader";
import PageTitle from "../shared/PageTitle";
import AlertMessage from "../shared/AlertMessage";

const Index = () => {
  const { error, loading, data } = useFetch(JOBS_URL, initialState.jobs);
  const breadcrumbItems = [{ title: "Görev Listesi", url: "/jobs" }];

  return (
    <Fragment>
      <Breadcrumb items={breadcrumbItems} />
      <div className="py-3">
        <PageTitle text="Görev Listesi" />
        {error && <AlertMessage type="danger" message=" Bir hata oluştu!" />}
        {loading && <Loader data-testid="loader-component" />}
        {!error && !loading && data && (
          <JobsTable data-testid="jobs-table-component" data={data} />
        )}
      </div>
    </Fragment>
  );
};

export default Index;
