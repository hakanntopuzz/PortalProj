import React, { Fragment, useState } from "react";
import { Col, Form, FormGroup, Label, Input, Row } from "reactstrap";
import { useForm } from "react-hook-form";
import useFetch from "../../hooks/useFetch";
import {
  GET_BUILD_TYPES_URL,
  GET_APPLICATIONS_URL,
  GET_ENVIRONMENTS_URL,
  GET_JENKINS_JOB_TYPES_URL,
} from "../../config/urls";
import initialState from "../../config/initialState";
import Breadcrumb from "../shared/Breadcrumb";
import PageTitle from "../shared/PageTitle";
import SelectBox from "../shared/SelectBox";
import CustomButton from "../shared/CustomButton";
import CustomLoadingButton from "../shared/CustomLoadingButton";
import AlertMessage from "../shared/AlertMessage";
import Loader from "../shared/Loader";
import {
  createBuildScript,
  downloadBuildScript,
} from "../../actions/buildScriptsActions";
import AddToFavouriteButton from "../shared/AddToFavouriteButton";

const Index = () => {
  const breadcrumbItems = [
    { title: "Derleme Scripti Oluştur", url: "/create-build-script" },
  ];

  const jenkinsJobTypes = useFetch(
    GET_JENKINS_JOB_TYPES_URL,
    initialState.jobTypes
  );
  const environments = useFetch(
    GET_ENVIRONMENTS_URL,
    initialState.environments
  );
  const buildTypes = useFetch(GET_BUILD_TYPES_URL, initialState.buildTypes);
  const applications = useFetch(
    GET_APPLICATIONS_URL,
    initialState.applications
  );

  const [createdScript, setCreatedScript] = useState("");
  const [error, setError] = useState(false);
  const [loading, setLoading] = useState(false);
  const [selectedBuildTypeId, setSelectedBuildTypeId] = useState(null);
  const [showDownloadButton, setShowDownloadButton] = useState(false);

  const { handleSubmit, register, errors } = useForm();
  const handleCreateBuildScript = (data) => {
    setError(false);
    setLoading(true);

    createBuildScript(data).then((buildResult) => {
      if (buildResult === "") {
        setCreatedScript("");
        setShowDownloadButton(false);
        setLoading(false);
        setError(true);
      } else {
        setLoading(false);
        setCreatedScript(buildResult);
        setSelectedBuildTypeId(data.BuildTypeId);
        setShowDownloadButton(true);
      }
    });
  };

  const download = () => {
    if (selectedBuildTypeId && selectedBuildTypeId !== "") {
      downloadBuildScript(parseInt(selectedBuildTypeId));
    }
  };

  const isLoading =
    jenkinsJobTypes.loading ||
    buildTypes.loading ||
    applications.loading ||
    environments.loading;

  return (
    <Fragment>
      <Breadcrumb items={breadcrumbItems} />
      <div className="py-3 mb-5">
        <PageTitle text="Derleme Scripti Oluştur" />
        <div className="d-flex justify-content-end">
        <AddToFavouriteButton pageTitle="Derleme Scripti Oluştur" />
        </div>
        {isLoading && <Loader data-testid="loader-component" />}
        <Form onSubmit={handleSubmit(handleCreateBuildScript)}>
          <SelectBox
            id="jobTypeId"
            name="JobTypeId"
            label="Görev Türü"
            innerRef={register({ required: true })}
            error={errors.JobTypeId}
            errorMessage="Lütfen görev türü seçiniz!"
            data={jenkinsJobTypes.data}
          />
          <SelectBox
            id="environmentId"
            name="EnvironmentId"
            label="Ortam"
            innerRef={register({ required: true })}
            error={errors.EnvironmentId}
            errorMessage="Lütfen ortam seçiniz!"
            data={environments.data}
          />
          <SelectBox
            id="buildTypeId"
            name="BuildTypeId"
            label="Derleme Türü"
            innerRef={register({ required: true })}
            error={errors.BuildTypeId}
            errorMessage="Lütfen derleme türü seçiniz!"
            data={buildTypes.data}
          />
          <SelectBox
            id="applicationId"
            name="ApplicationId"
            label="Uygulama"
            innerRef={register({ required: true })}
            error={errors.ApplicationId}
            errorMessage="Lütfen uygulama seçiniz!"
            data={applications.data}
          />
          <FormGroup>
            {loading && (
              <CustomLoadingButton
                size="sm"
                cssClass="mr-2"
                text=" Oluşturuluyor..."
              />
            )}
            {!loading && <CustomButton color="primary" text="Oluştur" />}
          </FormGroup>
          {error && <AlertMessage type="danger" message="Bir hata oluştu!" />}
          <FormGroup className="mt-4">
            <Row>
              <Label for="exampleText" sm={6}>
                Oluşturulan Script
              </Label>
              <Col sm={6}>
                {showDownloadButton && (
                  <CustomButton
                    outline
                    color="success"
                    size="sm"
                    text="İndir"
                    cssClass="float-right"
                    onClick={download}
                  />
                )}
              </Col>
            </Row>
            <Input
              style={{ resize: "none" }}
              readOnly={true}
              type="textarea"
              name="text"
              id="text-created-script"
              rows="10"
              defaultValue={createdScript}
            />
          </FormGroup>
        </Form>
      </div>
    </Fragment>
  );
};

export default Index;
