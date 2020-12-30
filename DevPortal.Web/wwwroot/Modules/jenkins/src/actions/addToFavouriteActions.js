import axios from "axios";
import {
  ADD_TO_FAVOURITES_URL,
  REMOVE_FROM_FAVOURITES_URL,
  CHECK_PAGE_IS_FAVOURITES_URL,
} from "../config/urls";

export const addToFavourite = (data) => {
  const request = axios
    .post(ADD_TO_FAVOURITES_URL, data)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });

  return request;
};

export const removeFromFavourite = (data) => {
  const request = axios
    .post(REMOVE_FROM_FAVOURITES_URL, data)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });

  return request;
};

export const checkPageIsFavourite = (pageUrl) => {
  const request = axios
    .get(`${CHECK_PAGE_IS_FAVOURITES_URL}?pageUrl=${pageUrl}`)
    .then((response) => {
      if (response.data.isSuccess) {
        return response.data;
      } else {
        return false;
      }
    })
    .catch((error) => {
      return error;
    });

  return request;
};
