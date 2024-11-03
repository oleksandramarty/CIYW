import { Directive, ElementRef, Input, OnInit, OnDestroy } from '@angular/core';
import { LocalizationService } from '../services/localization.service';
import { Subject, takeUntil, tap } from 'rxjs';
import {BaseUnsubscribeComponent} from "../base-components/base-unsubscribe.compoinent";

@Directive({
  selector: '[translation]'
})
export class TranslateDirective extends BaseUnsubscribeComponent {
  @Input('translation') key: string | undefined;
  @Input() translationAttr: 'innerText' | 'placeholder' = 'innerText';

  constructor(
    private el: ElementRef,
    private localizationService: LocalizationService
  ) {
    super();
  }

  override ngOnInit() {
    if (!this.key) {
      return;
    }

    this.updateTranslation();

    this.localizationService.localeChangedSub
      .pipe(
        takeUntil(this.ngUnsubscribe),
        tap((state) => {
          if (!!state) {
            this.updateTranslation();
          }
        })
      )
      .subscribe();
  }

  private updateTranslation() {
    if (this.key) {
      const translation = this.localizationService.getTranslation(this.key);
      if (this.translationAttr === 'innerText') {
        this.el.nativeElement.innerText = translation || this.key;
      } else if (this.translationAttr === 'placeholder') {
        this.el.nativeElement.placeholder = translation || this.key;
      }
    }
  }
}
