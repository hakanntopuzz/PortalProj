import React from "react";
import { cleanup } from "@testing-library/react";
import ShallowRenderer from "react-test-renderer/shallow";
import ProfileMenu from "../../../components/shared/header/ProfileMenu";

describe("Profile Menu", () => {
  afterEach(cleanup);

  test("renders without crashing", () => {
    const menu = [
      {
        name: "Hesap",
        link: null,
        icon: null,
        group: 2,
        children: [
          {
            name: "Hesap Bilgileri",
            link: "/account/profile",
            icon: "person_outline",
            group: 2,
            children: [],
            hasChildren: false,
          },
          {
            name: "Parola",
            link: "/account/password",
            icon: "lock_outline",
            group: 2,
            children: [],
            hasChildren: false,
          },
          {
            name: "Çıkış Yap",
            link: "/account/logout",
            icon: "exit_to_app",
            group: 2,
            children: [],
            hasChildren: false,
          },
        ],
        hasChildren: true,
      },
    ];
    
    const renderer = new ShallowRenderer();
    renderer.render(<ProfileMenu menu={menu} />);

    const result = renderer.getRenderOutput();

    expect(result).toMatchSnapshot();
  });
});
