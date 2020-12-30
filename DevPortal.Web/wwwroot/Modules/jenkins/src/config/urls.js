export const BASE_URL = process.env.REACT_APP_PUBLIC_URL;
export const MENU_URL = `${BASE_URL}/get-menu`;
export const JOBS_URL = `${BASE_URL}/get-jenkins-jobs`;
export const JOB_DETAIL_URL = `${BASE_URL}/get-jenkins-job-detail`;
export const FAILED_JOBS_URL = `${BASE_URL}/get-all-failed-jenkins-jobs`;
export const GET_USER_IDENTITY_URL = `${BASE_URL}/account/get-user-identity-name`;
export const LOGIN_PAGE_URL = `${BASE_URL}/account/login`;
export const LOGOUT_URL = `${BASE_URL}/account/logout`;
export const PROFILE_PAGE_URL = `${BASE_URL}/account/profile`;
export const ACCOUNT_PASSWORD_PAGE_URL = `${BASE_URL}/account/password`;
export const UNAUTHORIZED_REDIRECT_URL = `${BASE_URL}/account/login?returnUrl=/jenkins/jobs`;
export const GET_BUILD_TYPES_URL = `${BASE_URL}/get-build-types`;
export const GET_APPLICATIONS_URL = `${BASE_URL}/get-applications`;
export const CREATE_NEW_BUILD_SCRIPT_URL = `${BASE_URL}/create-new-build-script`;
export const GET_ENVIRONMENTS_URL = `${BASE_URL}/get-environments`;
export const GET_JENKINS_JOB_TYPES_URL = `${BASE_URL}/get-jenkins-job-types`;
export const ADD_TO_FAVOURITES_URL = `${BASE_URL}/add-to-favourites`;
export const REMOVE_FROM_FAVOURITES_URL = `${BASE_URL}/remove-from-favourites`;
export const CHECK_PAGE_IS_FAVOURITES_URL = `${BASE_URL}/check-page-is-favourites`;