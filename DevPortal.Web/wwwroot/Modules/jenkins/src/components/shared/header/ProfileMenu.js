import React, { Fragment } from "react";
import { UncontrolledDropdown, DropdownToggle, DropdownMenu } from "reactstrap";
import useFetch from "../../../hooks/useFetch";
import { GET_USER_IDENTITY_URL } from "../../../config/urls";

const ProfileMenu = ({ menu }) => {
  let { error, data } = useFetch(GET_USER_IDENTITY_URL, null);
  const profileMenuItems = menu.find((x) => x.group === 2);

  return (
    <Fragment>
      {!error && data && (
        <UncontrolledDropdown
          className="nav-item-profile float-right"
          data-testid="profile-menu">
          <DropdownToggle nav caret data-testid="profileName">
            {data}
          </DropdownToggle>
          {profileMenuItems.children.length > 0 && (
            <DropdownMenu>
              {profileMenuItems.children.map((child, childIndex) => (
                <a
                  key={childIndex}
                  href={`${child.link}`}
                  className="dropdown-item">
                  <i className={`fa ${child.icon}`} aria-hidden="true"></i>
                  {child.name}
                </a>
              ))}
            </DropdownMenu>
          )}
        </UncontrolledDropdown>
      )}
    </Fragment>
  );
};

export default ProfileMenu;
