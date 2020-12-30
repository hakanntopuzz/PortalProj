import React from "react";
import { UncontrolledDropdown, DropdownToggle, DropdownMenu } from "reactstrap";

const NavItems = ({ menu }) => {
  return (
    menu &&
    menu.length > 0 &&
    menu.map((item, index) => (
      <UncontrolledDropdown nav inNavbar key={index}>
        {item.group === 1 && (
          <DropdownToggle
            nav
            caret
            data-testid="nav-item"
            className={item.name === "Jenkins" ? "active" : ""}>
            {item.name}
          </DropdownToggle>
        )}
        {item.children.length > 0 && (
          <DropdownMenu>
            {item.children.map((child, childIndex) => (
              <a
                key={childIndex}
                href={`${child.link}`}
                className="dropdown-item">
                {child.name}
              </a>
            ))}
          </DropdownMenu>
        )}
      </UncontrolledDropdown>
    ))
  );
};

export default NavItems;
