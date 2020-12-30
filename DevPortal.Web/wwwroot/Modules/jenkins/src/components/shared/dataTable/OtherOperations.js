import React, { useState } from "react";
import { CSVLink } from "react-csv";
import {
  Dropdown,
  DropdownItem,
  DropdownToggle,
  DropdownMenu,
} from "reactstrap";

const OtherOperations = ({ headers, data, filename }) => {
  const [otherOptionsOpen, setOtherOptionsOpen] = useState(false);
  const toggleOtherOptions = () => setOtherOptionsOpen(!otherOptionsOpen);

  return (
    <div className="otherOperationsSection">
      <Dropdown isOpen={otherOptionsOpen} toggle={toggleOtherOptions}>
        <DropdownToggle className="btn btn-simple btn-sm hover-primary">
        <i className="fa fa-grip-vertical" aria-hidden="true"></i> Diğer işlemler
        </DropdownToggle>
        <DropdownMenu>
          <DropdownItem>
            <CSVLink
              data={data}
              headers={headers}
              filename={filename}>
              Dışa Aktar
            </CSVLink>
          </DropdownItem>
        </DropdownMenu>
      </Dropdown>
    </div>
  );
};

export default OtherOperations;
