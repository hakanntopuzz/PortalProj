import React, { useEffect, useState, Fragment } from "react";
import { useForm } from "react-hook-form";
import {
  addToFavourite,
  removeFromFavourite,
  checkPageIsFavourite,
} from "../../actions/addToFavouriteActions";
import { SuccessAlertPopup, ErrorAlertPopup } from "../shared/AlertPopup";

const AddToFavouriteButton = (props) => {
  const { handleSubmit } = useForm();
  const PageUrl = window.location.pathname + window.location.search;
  const [favourite, setFavourite] = useState(false);
  const [favouriteId, setFavouriteId] = useState(null);

  //#region check is favourite
  useEffect(() => {
    const checkPageIsFavourites = () => {
      checkPageIsFavourite(PageUrl).then((result) => {
        if (result.isSuccess === true) {
          setFavourite(true);
          setFavouriteId(result.data.id);
        }
      });
    };
    checkPageIsFavourites();
  }, [PageUrl]);
  //#endregion

  //#region add to favourite
  const handleAddToFavourite = () => {
    const data = {
      PageTitle: props.pageTitle,
      PageUrl: PageUrl,
    };

    addToFavourite(data).then((result) => {
      if (result.isSuccess) {
        checkPageIsFavourite(PageUrl).then((result) => {
          if (result.isSuccess === true) {
            setFavourite(true);
            setFavouriteId(result.data.id);
          }
        });
        SuccessAlertPopup(result.message);
      } else {
        ErrorAlertPopup();
      }
    });
  };
  //#endregion

  //#region remove from favourite
  const handleRemoveFromFavourite = () => {
    const data = {
      Id: parseInt(favouriteId),
    };

    removeFromFavourite(data).then((result) => {
      if (result.isSuccess) {
        setFavourite(false);
        setFavouriteId(0);
        SuccessAlertPopup(result.message);
      } else {
        ErrorAlertPopup();
      }
    });
  };
  //#endregion

  //#region render button
  let button = (
    <form onSubmit={handleSubmit(handleAddToFavourite)}>
      <button type="submit" className="btn btn-simple btn-sm hover-action">
        <i className="fa fa-star" aria-hidden="true"></i> Favorilerime Ekle
      </button>
    </form>
  );

  if (favourite) {
    button = (
      <form onSubmit={handleSubmit(handleRemoveFromFavourite)}>
        <button
          type="submit"
          className="btn btn-simple btn-sm hover-action active">
          <i className="fa fa-star" aria-hidden="true"></i> Favorilerimden Çıkar
        </button>
      </form>
    );
  }
  //#endregion

  return <Fragment>{button}</Fragment>;
};

export default AddToFavouriteButton;
