import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject, takeUntil, tap} from "rxjs";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MatSnackBar} from "@angular/material/snack-bar";
import {CustomValidators} from "../../../../core/helpers/validator.helper";
import {selectUser} from "../../../../core/store/selectors/auth.selectors";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {Store} from "@ngrx/store";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {DataItem} from "../../../../core/models/common/data-item.model";
import {UserResponse} from "../../../../core/api-models/common.models";

@Component({
    selector: 'app-user-settings',
    templateUrl: './user-settings.component.html',
    styleUrl: './user-settings.component.scss'
})
export class UserSettingsComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();

    public currentUser: UserResponse | undefined;
    public userSettingsForm: FormGroup | undefined;

    get locales(): DataItem[] | undefined {
        return this.dictionaryService.dataItems?.locales;
    }

    constructor(
        private readonly fb: FormBuilder,
        private readonly snackBar: MatSnackBar,
        private readonly store: Store,
        private readonly dictionaryService: DictionaryService,
    ) {
    }

    ngOnInit(): void {
        this.store.select(selectUser)
            .pipe(
                takeUntil(this.ngUnsubscribe),
                tap((user) => {
                    this.currentUser = user;
                    this.createUserSettingsForm();
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public updateUserSettings(): void {
        console.log(this.userSettingsForm)
    }

    private createUserSettingsForm(): void {
        this.userSettingsForm = this.fb.group({
            defaultLocale: [this.currentUser?.userSetting?.defaultLocale, Validators.required],
            timeZone: [this.currentUser?.userSetting?.timeZone],
            currencyId: [this.currentUser?.userSetting?.currencyId],
            countryId: [this.currentUser?.userSetting?.countryId],
            defaultUserProject: [this.currentUser?.userSetting?.defaultUserProject],

            currentPassword: [null, Validators.required],
            newPassword: [null, Validators.minLength(6)],
            confirmNewPassword: [null],
        }, {validator: CustomValidators.PasswordMatchValidator});
    }
}