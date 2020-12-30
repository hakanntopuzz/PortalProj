import React from "react";
import { render, cleanup, fireEvent } from "@testing-library/react";
import Header from "../../../components/shared/header/Header";
import ShallowRenderer from "react-test-renderer/shallow";
import DevPortalLogo from "../../../img/devportal-logo.png";

describe("Header", () => {
  afterEach(cleanup);

  test("renders without crashing", () => {
    const renderer = new ShallowRenderer();
    renderer.render(<Header />);

    const result = renderer.getRenderOutput();

    expect(result).toMatchSnapshot();
  });

  test("header component has dev portal logo", () => {
    const { getByAltText } = render(<Header />);
    const logo = getByAltText("dev portal logo");

    expect(logo).toBeInTheDocument();
    expect(logo.getAttribute("src")).toBe(DevPortalLogo);
  });

  // test("when clicking toggle menu button set the menu open", () => {
  //   const menu = [
  //     {
  //       name: "Uygulama",
  //       link: null,
  //       group: 1,
  //       children: [
  //         {
  //           name: "Uygulamalar",
  //           link: "/application",
  //           children: [],
  //           hasChildren: false,
  //         },
  //         {
  //           name: "Uygulama Grupları",
  //           link: "/applicationgroup",
  //           children: [],
  //           hasChildren: false,
  //         },
  //       ],
  //       hasChildren: true,
  //     },
  //   ];
    
  //   const mockData = { error: false, loading: false, data: menu };

  //   const { getByTestId } = render(<Header />);

  //   jest.mock("../../../hooks/useFetch", () => {
  //     const fetchData = jest.fn();
  //     return jest
  //       .fn()
  //       .mockImplementation(() => {
  //         return { fetchData: fetchData };
  //       })
  //       .mockReturnValue(Promise.resolve(mockData));
  //   });

  //   const toggleMenuButton = getByTestId("toggleMenuButton");
  //   const collapseMenu = getByTestId("collapse-menu");

  //   fireEvent.click(toggleMenuButton);
  //   expect(collapseMenu.getAttribute("data-value")).toBe("true");
  // });

  // test("header component has a navigation menu", () => {
  //   const menu = [
  //     {
  //       name: "Uygulama",
  //       link: null,
  //       group: 1,
  //       children: [
  //         {
  //           name: "Uygulamalar",
  //           link: "/application",
  //           children: [],
  //           hasChildren: false,
  //         },
  //         {
  //           name: "Uygulama Grupları",
  //           link: "/applicationgroup",
  //           children: [],
  //           hasChildren: false,
  //         },
  //       ],
  //       hasChildren: true,
  //     },
  //   ];
    
  //   const mockData = { error: false, loading: false, data: menu };

  //   const { getByTestId } = render(<Header />);

  //   jest.mock("../../../hooks/useFetch", () => {
  //     const fetchData = jest.fn();
  //     return jest
  //       .fn()
  //       .mockImplementation(() => {
  //         return { fetchData: fetchData };
  //       })
  //       .mockReturnValue(Promise.resolve(mockData));
  //   });

  //   const { getByTestId } = render(<Header />);
  //   const navMenu = getByTestId("nav-menu");

  //   expect(navMenu).toBeInTheDocument();
  // });
});
