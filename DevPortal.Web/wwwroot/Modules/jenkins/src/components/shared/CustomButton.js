import React from "react";
import { Button } from "reactstrap";

const CustomButton = ({
  type,
  outline,
  color,
  size,
  cssClass,
  text,
  onClick,
}) => {
  return (
    <Button
      type={type}
      outline={outline ? true : false}
      color={color}
      size={size ? size : "md"}
      className={cssClass}
      onClick={onClick}>
      {text}
    </Button>
  );
};

export default CustomButton;
