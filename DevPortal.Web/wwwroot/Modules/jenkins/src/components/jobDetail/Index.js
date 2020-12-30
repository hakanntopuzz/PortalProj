import React, { Fragment } from "react";
import { useParams } from "react-router-dom";
import Breadcrumb from "../shared/Breadcrumb";
import useFetch from "../../hooks/useFetch";
import { JOB_DETAIL_URL } from "../../config/urls";
import initialState from "../../config/initialState";
import { Alert } from "reactstrap";
import Loader from "../shared/Loader";
import PageTitle from "../shared/PageTitle";
import HealthReport from "./HealthReport";
import BuildTable from "./BuildTable";
import AddToFavouriteButton from "../shared/AddToFavouriteButton";

const Index = () => {
  let { name } = useParams();
  const { error, loading, data } = useFetch(
    `${JOB_DETAIL_URL}/${name}`,
    initialState.job
  );

  const breadcrumbItems = [
    { title: "Görev Listesi", url: "/jobs" },
    { title: `${name}`, url: `/jobs/${name}` },
  ];

  const getAddToFavoritePageTitle = (jobTitle) => {
    return `${jobTitle}-Jenkins Görev Detayı`;
  };

  return (
    <Fragment>
      <Breadcrumb items={breadcrumbItems} />
      <div className="py-3">
        <PageTitle text="Jenkins Görev Detayı" />
        {error && <Alert color="danger">Bir hata oluştu!</Alert>}
        {loading && <Loader />}
        {!error && !loading && data && (
          <Fragment>
            <div className="d-flex justify-content-end">
              <AddToFavouriteButton pageTitle={getAddToFavoritePageTitle(data.fullDisplayName)} />
            </div>
            <h2>{data.fullDisplayName}</h2>
            <a href={data.url} target="_blank" rel="noopener noreferrer">
              <span className="text-muted">{data.url}</span>
            </a>
            <HealthReport data={data} />
            <BuildTable data={data} />
          </Fragment>
        )}
      </div>
    </Fragment>
  );
};

export default Index;
