import React, { Fragment } from "react";
import { Switch, Route } from "react-router-dom";
import "../styles/site.css";
import "../styles/app.css";
import "../styles/material_icons.css";

import Header from "./shared/header/Header";
import Footer from "./shared/Footer";
import JobList from "./jobs/Index";
import JobDetail from "./jobDetail/Index";
import FailedJobList from "./failedJobs/Index";
import BuildScripts from "./buildScripts/Index";
import { UNAUTHORIZED_REDIRECT_URL } from "../config/urls";
import axios from "axios";

const IsAuth = () => {
  axios.interceptors.response.use(
    function (response) {
      return response;
    },
    function (error) {
      if (error && error.response && error.response.status === 401) {
        window.location.href = UNAUTHORIZED_REDIRECT_URL;
      }
    }
  );
}

const App = () => {
  IsAuth();
  return (
    <Fragment>
      <Header />
      <div className="container-middle">
        <Switch>
          <Route path="/create-build-script" component={BuildScripts} />
          <Route path="/failed-jobs" component={FailedJobList} />
          <Route path="/jobs/:name" component={JobDetail} />
          <Route path="/jobs" component={JobList} />
        </Switch>
      </div>
      <Footer />
    </Fragment>
  );
};

export default App;
