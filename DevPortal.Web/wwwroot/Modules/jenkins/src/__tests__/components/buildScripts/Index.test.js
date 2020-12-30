import React from "react";
import { render, cleanup } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import Index from "../../../components/buildScripts/Index";
import 'mutationobserver-shim';

global.MutationObserver = window.MutationObserver;

describe("Index", () => {
  afterEach(cleanup);
  it("renders when page loaded", () => {
    const root = document.createElement("div");
    document.body.appendChild(root);
    
    const { getByTestId } = render(
      <MemoryRouter initialEntries={["/jenkins/create-build-script"]}>
        <Index />
      </MemoryRouter>,
      root
    );

    const pageTitle = getByTestId("page-title");

    expect(pageTitle).toBeInTheDocument();
  });
});