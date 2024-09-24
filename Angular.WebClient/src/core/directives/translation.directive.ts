import { Directive, ElementRef, Input, OnInit, OnDestroy } from '@angular/core';
import { LocalizationService } from '../services/localization.service';
import { Subject, takeUntil, tap } from 'rxjs';

@Directive({
  selector: '[translation]'
})
export class TranslateDirective implements OnInit, OnDestroy {
  protected ngUnsubscribe: Subject<void> = new Subject<void>();
  @Input('translation') key: string | undefined;
  @Input() translationAttr: 'innerText' | 'placeholder' = 'innerText';

  constructor(
    private el: ElementRef,
    private localizationService: LocalizationService
  ) {}

  ngOnInit() {
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

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
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
