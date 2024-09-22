import { Directive, HostListener, Input } from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';
import { MatSnackBar } from '@angular/material/snack-bar';

@Directive({
  selector: '[appCopyToClipboard]'
})
export class CopyToClipboardDirective {
  @Input('appCopyToClipboard') textToCopy: string | undefined;

  constructor(private clipboard: Clipboard, private snackBar: MatSnackBar) {}

  @HostListener('click')
  copyText() {
    if (this.textToCopy) {
      this.clipboard.copy(this.textToCopy);
      this.snackBar.open('Copied to clipboard!', 'Close', {
        duration: 2000,
      });
    }
  }
}
