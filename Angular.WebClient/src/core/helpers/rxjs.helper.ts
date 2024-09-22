import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError, throwError} from "rxjs";
import {DictionaryService} from "../services/dictionary.service";

export function handleApiError(
  snackBar: MatSnackBar,
  dictionaryService: DictionaryService | undefined = undefined,
  message: string = 'ERROR.GENERIC') {
  return catchError((error: any) => {
    console.error(error);
    if (!!dictionaryService) {
      snackBar.open(dictionaryService.getTranslation(error?.message ?? message) ?? message, 'Close', { duration: 3000 });
    } else {
      snackBar.open(error?.message ?? message, 'Close', { duration: 3000 });
    }
    return throwError(() => error);
  });
}
