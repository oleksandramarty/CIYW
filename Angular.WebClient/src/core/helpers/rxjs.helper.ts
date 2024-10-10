import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError, throwError} from "rxjs";
import {LocalizationService} from "../services/localization.service";

export function handleApiError(
  snackBar: MatSnackBar,
  localizationService: LocalizationService | undefined = undefined,
  message: string = 'ERROR.GENERIC') {
  return catchError((error: any) => {

    console.log(typeof error)
    console.log(JSON.stringify(error))
    console.log(JSON.parse(JSON.stringify(error)))
    console.log(error)
    console.log(error)

    console.error(error);
    if (!!localizationService) {
      snackBar.open(localizationService.getTranslation(error?.message ?? message) ?? message, 'Close', { duration: 3000 });
    } else {
      snackBar.open(error?.message ?? message, 'Close', { duration: 3000 });
    }
    return throwError(() => error);
  });
}
