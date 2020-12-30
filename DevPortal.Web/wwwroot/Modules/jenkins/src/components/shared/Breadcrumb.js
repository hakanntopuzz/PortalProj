import React from "react";
import { Link } from "react-router-dom";

const Breadcrumb = items => {
  const breadcrumbItems = items.items;
  return (
    <nav aria-label="breadcrumb" className="my-2">
      <ol className="breadcrumb">
        <li className="breadcrumb-item">Jenkins</li>
        {breadcrumbItems &&
          breadcrumbItems.length > 0 &&
          breadcrumbItems.map((item, index) => (
            <li className="breadcrumb-item" key={index}>
              <Link to={item.url}>{item.title}</Link>
            </li>
          ))}
      </ol>
    </nav>
  );
};

export default Breadcrumb;
