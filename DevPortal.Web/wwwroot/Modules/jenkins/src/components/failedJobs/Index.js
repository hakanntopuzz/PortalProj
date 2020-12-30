import React, { Fragment } from "react";
import { FAILED_JOBS_URL } from "../../config/urls";
import initialState from "../../config/initialState";
import Breadcrumb from "../shared/Breadcrumb";
import useFetch from "../../hooks/useFetch";
import FailedJobsTable from "./FailedJobsTable";
import Loader from "../shared/Loader";
import PageTitle from "../shared/PageTitle";
import AlertMessage from "../shared/AlertMessage";

const Index = () => {
  const { error, loading, data } = useFetch(FAILED_JOBS_URL, initialState.jobs);
  const breadcrumbItems = [
    { title: "Başarısız Görev Listesi", url: "/failed-jobs" },
  ];

  return (
    <Fragment>
      <Breadcrumb items={breadcrumbItems} />
      <div className="py-3">
        <PageTitle text="Başarısız Görev Listesi" />
        {error && <AlertMessage type="danger" message=" Bir hata oluştu!" />}
        {loading && <Loader />}
        {!error && !loading && data && <FailedJobsTable data={data} />}
      </div>
    </Fragment>
  );
};

export default Index;
