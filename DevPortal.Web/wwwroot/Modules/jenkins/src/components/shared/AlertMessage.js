import React from "react";
import { Alert } from "reactstrap";

const AlertMessage = ({type, message}) => {
  return(
    <Alert data-testid="alert-component" color={type}>{message}</Alert>
  );
}

export default AlertMessage;