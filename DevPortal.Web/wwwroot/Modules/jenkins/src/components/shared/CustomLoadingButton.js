import React from "react";
import { Button, Spinner } from "reactstrap";

const CustomLoadingButton = ({ size, cssClass, text }) => {
  return (
    <Button variant="primary" disabled>
      <Spinner
        as="span"
        animation="grow"
        size={size ? size : "md"}
        role="status"
        aria-hidden="true"
        className={cssClass}
      />
      {text}
    </Button>
  );
};

export default CustomLoadingButton;
