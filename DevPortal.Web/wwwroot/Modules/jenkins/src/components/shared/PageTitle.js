import React from "react";

const PageTitle = ({ text }) => {
  return <h1 className="title-primary display-4" data-testid="page-title">{text}</h1>;
};

export default PageTitle;
