import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError, throwError} from "rxjs";
import {LocalizationService} from "../services/localization.service";
import {ApolloError} from "@apollo/client";

export function handleApiError(
    snackBar: MatSnackBar,
    localizationService: LocalizationService | undefined = undefined,
    message: string = 'ERROR.GENERIC') {
    return catchError((error: any) => {
        let errorMessage = message;
        let apolloCustomError = new ApolloCustomError(error);

        if (apolloCustomError?.networkError?.error?.message) {
            errorMessage = apolloCustomError.networkError.error.message;
        } else {
          if (error instanceof ApolloError) {
            const apolloError = error as ApolloError;
            if (apolloError.graphQLErrors.length > 0) {
              errorMessage = apolloError.graphQLErrors[0].message;
            } else if (apolloError.networkError) {
              errorMessage = apolloError.networkError.message;
            }
          } else if (error.error) {
            errorMessage = error.error.message;
          } else if (error.message) {
            errorMessage = error.message;
          } else if (error.statusText) {
            errorMessage = error.statusText;
          }
        }

        if (!!localizationService) {
            snackBar.open(localizationService.getTranslation(errorMessage) ?? errorMessage, 'Close', {duration: 3000});
        } else {
            snackBar.open(errorMessage, 'Close', {duration: 3000});
        }
        return throwError(() => error);
    });
}

export interface IApolloCustomError {
  name: string;
  graphQLErrors: any[];
  protocolErrors: any[];
  clientErrors: any[];
  networkError: HttpErrorResponse;
  message: string;
  cause: HttpErrorResponse;
}

export interface HttpErrorResponse {
  headers: HttpHeaders;
  status: number;
  statusText: string;
  url: string;
  ok: boolean;
  name: string;
  message: string;
  error: ErrorDetails;
}

export interface HttpHeaders {
  normalizedNames: Record<string, any>;
  lazyUpdate: any;
}

export interface ErrorDetails {
  message: string;
  statusCode: number;
}

export class ApolloCustomError implements IApolloCustomError {
  name: string;
  graphQLErrors: any[];
  protocolErrors: any[];
  clientErrors: any[];
  networkError: HttpErrorResponseModel;
  message: string;
  cause: HttpErrorResponseModel;

  constructor(data: IApolloCustomError) {
    this.name = data.name;
    this.graphQLErrors = data.graphQLErrors;
    this.protocolErrors = data.protocolErrors;
    this.clientErrors = data.clientErrors;
    this.networkError = new HttpErrorResponseModel(data.networkError);
    this.message = data.message;
    this.cause = new HttpErrorResponseModel(data.cause);
  }
}

export class HttpErrorResponseModel implements HttpErrorResponse {
  headers: HttpHeadersModel;
  status: number;
  statusText: string;
  url: string;
  ok: boolean;
  name: string;
  message: string;
  error: ErrorDetailsModel;

  constructor(data: HttpErrorResponse) {
    this.headers = new HttpHeadersModel(data.headers);
    this.status = data.status;
    this.statusText = data.statusText;
    this.url = data.url;
    this.ok = data.ok;
    this.name = data.name;
    this.message = data.message;
    this.error = new ErrorDetailsModel(data.error);
  }
}

export class HttpHeadersModel implements HttpHeaders {
  normalizedNames: Record<string, any>;
  lazyUpdate: any;

  constructor(data: HttpHeaders) {
    this.normalizedNames = data.normalizedNames;
    this.lazyUpdate = data.lazyUpdate;
  }
}

export class ErrorDetailsModel implements ErrorDetails {
  message: string;
  statusCode: number;

  constructor(data: ErrorDetails) {
    this.message = data.message;
    this.statusCode = data.statusCode;
  }
}