import React from "react";
import { render } from '@testing-library/react';
import Footer from "../../../components/shared/Footer";

describe("Footer", () => {
  it('renders without crashing and includes `DevPortal`', () => {
    const { getByText } = render(<Footer />);
    const linkElement = getByText(/DevPortal/i);
    expect(linkElement).toBeInTheDocument();
  });
});