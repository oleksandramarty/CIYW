import {environment} from "../environments/environment";

const API_PREFIX = 'api';
const V1 = 'v1';

export const API_ROUTES = {
  DATA_API: {
    LOCALIZATIONS: {
      GET: `${environment.apiDataUrl}/${API_PREFIX}/${V1}/localizations`,
      LOCALES: `${environment.apiDataUrl}/${API_PREFIX}/${V1}/localizations/locales`,
    },
    SITE_SETTINGS : {
      GET: `${environment.apiDataUrl}/${API_PREFIX}/${V1}/siteSettings`
    }
  },
  USER_API: {
    ACCOUNT: {
      REGISTER: `${environment.apiUsersUrl}/${API_PREFIX}/${V1}/accounts/register`,
      LOGIN: `${environment.apiUsersUrl}/${API_PREFIX}/${V1}/accounts/login`,
      CURRENT: `${environment.apiUsersUrl}/${API_PREFIX}/${V1}/accounts/current`,
      LOGOUT: `${environment.apiUsersUrl}/${API_PREFIX}/${V1}/accounts/logout`,
    },
    USER: {
      CURRENT: `${environment.apiUsersUrl}/${API_PREFIX}/${V1}/users/current`,
    },
  },
  KARMA_API: {
    KARMA_TAG: {
      KARMA: `${environment.apiKarmaUrl}/${API_PREFIX}/${V1}/tags/karma`,
    }
  },
  VACANCY_API: {
    VACANCY: {
      SUGGEST: `${environment.apiVacanciesUrl}/${API_PREFIX}/${V1}/vacancies/suggest`,
    }
  }
}
