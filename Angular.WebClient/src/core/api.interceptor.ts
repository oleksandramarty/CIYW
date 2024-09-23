import {Injectable} from "@angular/core";
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Router} from "@angular/router";
import {catchError, Observable, switchMap, take, throwError} from "rxjs";
import {Store} from "@ngrx/store";
import {selectToken} from "./store/selectors/auth.selectors";
import {environment} from "./environments/environment";

export const HTTP_METHODS = {
  GET: 'GET',
  HEAD: 'HEAD',
  POST: 'POST',
  PUT: 'PUT',
  PATCH: 'PATCH',
  DELETE: 'DELETE',
  CONNECT: 'CONNECT',
  OPTIONS: 'OPTIONS',
  TRACE: 'TRACE',
};

@Injectable()
export class BaseUrlInterceptor implements HttpInterceptor {
  private sso_req = null;

  constructor(
    private router: Router,
    private readonly store: Store) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return this.store.select(selectToken)
      .pipe(
        take(1),
        switchMap((token) => {
          if (token?.token) {
            request = request.clone({
              setHeaders: {
                Authorization: `${environment.authSchema} ${token?.token}`
              }
            });
          }
          return next.handle(request);
        }),
        catchError((error: HttpErrorCustomResponse) => {
          if (typeof error === 'string') {
            error = JSON.parse(error);
          }
          return throwError(error);
        })
      );
  }
}

export declare class HttpErrorCustomResponse extends HttpErrorResponse {
  fields: any;
}
