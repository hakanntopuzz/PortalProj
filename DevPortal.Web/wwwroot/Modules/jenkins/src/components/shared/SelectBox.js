import React from "react";
import { FormGroup, Label, Input } from "reactstrap";

const SelectBox = ({ id,name, label, innerRef, error, errorMessage, data }) => {
  return (
    <FormGroup className="mb-4">
      <Label for="exampleSelect">{label}</Label>
      <Input
        type="select"
        id={id}
        name={name}
        defaultValue=""
        innerRef={innerRef}
        className={error ? "mb-0 is-invalid" : ""}>
        <option value="">Seçiniz</option>
        {data &&
          data.length > 0 &&
          data.map((option) => {
            return (
              <option key={option.id} value={option.id}>
                {option.name}
              </option>
            );
          })}
      </Input>
      {error && (
        <div className="invalid-feedback">{errorMessage}</div>
      )}
    </FormGroup>
  );
};

export default SelectBox;
