import React from "react";

const BuildTable = ({ data }) => {
  return (
    <table className="table table-bordered table-responsive-cols mt-4 mb-5">
      <thead className="thead-light">
        <tr>
          <th scope="col">Derleme Tipi</th>
          <th scope="col" className="text-center">
            Derleme Numarası
          </th>
          <th scope="col">
            Url
          </th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>Son Derleme</td>
          <td className="text-center">{data.lastBuild.number}</td>
          <td>
            <a
              href={data.lastBuild.url}
              target="_blank"
              rel="noopener noreferrer">
              {data.lastBuild.url}
            </a>
          </td>
        </tr>
        <tr>
          <td>Son Başarılı Derleme</td>
          <td className="text-center">{data.lastSuccessfulBuild.number}</td>
          <td>
            <a
              href={data.lastSuccessfulBuild.url}
              target="_blank"
              rel="noopener noreferrer">
              {data.lastSuccessfulBuild.url}
            </a>
          </td>
        </tr>
        <tr>
          <td>Son Başarısız Derleme</td>
          <td className="text-center">{data.lastFailedBuild.number}</td>
          <td>
            <a
              href={data.lastFailedBuild.url}
              target="_blank"
              rel="noopener noreferrer">
              {data.lastFailedBuild.url}
            </a>
          </td>
        </tr>
      </tbody>
    </table>
  );
};

export default BuildTable;
