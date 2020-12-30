import React, { useState, Fragment } from "react";
import { Collapse, Navbar, NavbarToggler, NavbarBrand, Nav } from "reactstrap";
import "../../../styles/header.css";
import useFetch from "../../../hooks/useFetch";
import { MENU_URL } from "../../../config/urls";
import initialState from "../../../config/initialState";
import images from "../../../config/images";
import ProfileMenu from "./ProfileMenu";
import NavItems from "./NavItems";

const Header = () => {
  const { data } = useFetch(MENU_URL, initialState.menu);
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  
  const toggle = () => setIsMenuOpen(!isMenuOpen);

  return (
    <header>
      <Navbar color="light" light expand="lg">
        <div className="container-middle d-xl-flex d-lg-flex justify-content-between align-items-center position-relative">
          <NavbarBrand href="/">
            <img src={images.DevPortalLogo} alt="dev portal logo" width="80" />
          </NavbarBrand>
          <NavbarToggler
            onClick={toggle}
            data-testid="toggleMenuButton"
            className="float-right"
          />
          {data && data.length > 0 && (
            <Fragment>
              <ProfileMenu menu={data} />
              <Collapse
                isOpen={isMenuOpen}
                navbar
                data-testid="collapse-menu"
                data-value={isMenuOpen}
                className="justify-content-end">
                <Nav className="nav" navbar data-testid="nav-menu">
                  <NavItems menu={data} />
                </Nav>
              </Collapse>
            </Fragment>
          )}
        </div>
      </Navbar>
    </header>
  );
};

export default Header;
