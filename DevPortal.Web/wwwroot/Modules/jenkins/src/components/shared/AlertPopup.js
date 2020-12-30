import Swal from "sweetalert2";

export const SuccessAlertPopup = (message) => {
  Swal.fire({
    text: message,
    icon: "success",
    showCloseButton: true,
    showCancelButton: false,
    showConfirmButton: false,
  });
};

export const ErrorAlertPopup = () => {
  Swal.fire({
    text: "Bir hata oluştu! Lütfen daha sonra tekrar deneyiniz.",
    icon: "warning",
    showCloseButton: true,
    showCancelButton: false,
    showConfirmButton: false,
  });
};
