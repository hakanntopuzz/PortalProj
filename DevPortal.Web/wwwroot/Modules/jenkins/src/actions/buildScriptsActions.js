import axios from "axios";
import { CREATE_NEW_BUILD_SCRIPT_URL } from "../config/urls";

export const createBuildScript = (data) => {
  const requestData = {
    ApplicationId: parseInt(data.ApplicationId),
    BuildTypeId: parseInt(data.BuildTypeId),
    JobTypeId: parseInt(data.JobTypeId),
    EnvironmentId: parseInt(data.EnvironmentId),
  };

  const request = axios
    .post(CREATE_NEW_BUILD_SCRIPT_URL, requestData)
    .then((response) => {
      if (response.data.isSuccess) {
        return response.data.value;
      } else {
        return "";
      }
    })
    .catch((error) => {
      return error;
    });

  return request;
};

export const downloadBuildScript = (selectedBuildTypeId) => {
  const element = document.createElement("a");
  const file = new Blob(
    [document.getElementById("text-created-script").value],
    {
      type: "application/bat",
    }
  );
  element.href = URL.createObjectURL(file);
  element.download = `${generateDownloadFileName(selectedBuildTypeId)}.bat`;
  document.body.appendChild(element);
  element.click();
};

const generateDownloadFileName = (selectedBuildTypeId) => {
  if (selectedBuildTypeId === 1) {
    return "build";
  } else if (selectedBuildTypeId === 2) {
    return "package-restore";
  } else if (selectedBuildTypeId === 3) {
    return "run-unit-test";
  } else if (selectedBuildTypeId === 4) {
    return "deploy";
  } else if (selectedBuildTypeId === 5) {
    return "run-test-coverage";
  } else {
    return "";
  }
};
