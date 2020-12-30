import React from 'react';
import { render } from '@testing-library/react';
import BuildTable from '../../../components/jobDetail/BuildTable';

test('renders jenkins jobs build table', () => {
  const data = {
    lastBuild: 1,
    lastSuccessfulBuild:2,
    lastFailedBuild:3
  };

  const { getByRole } = render(<BuildTable data={data} />);
  const table = getByRole("table");
  
  expect(table).toBeInTheDocument();
  expect(table.classList.contains("table")).toBe(true);
});