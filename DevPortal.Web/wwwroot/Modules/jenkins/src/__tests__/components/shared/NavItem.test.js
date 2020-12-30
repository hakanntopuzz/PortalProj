import React from "react";
import { render, cleanup } from "@testing-library/react";
import NavItems from "../../../components/shared/header/NavItems";

describe("Header", () => {
  afterEach(cleanup);

  test("header component has a navigation menu", () => {
    const menu = [
      {
        name: "Uygulama",
        link: null,
        group: 1,
        children: [
          {
            name: "Uygulamalar",
            link: "/application",
            children: [],
            hasChildren: false,
          },
          {
            name: "Uygulama Grupları",
            link: "/applicationgroup",
            children: [],
            hasChildren: false,
          },
        ],
        hasChildren: true,
      },
    ];
    const { getAllByTestId } = render(<NavItems menu={menu} />);
    const navItem = getAllByTestId("nav-item");

    expect(menu).toHaveLength(1);
    expect(navItem[0]).toHaveTextContent(menu[0].name);
  });
});
