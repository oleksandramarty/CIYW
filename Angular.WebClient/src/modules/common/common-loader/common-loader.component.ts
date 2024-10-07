import {Component, Input, OnInit} from '@angular/core';
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {LoaderService} from "../../../core/services/loader.service";
import {tap} from "rxjs";
import {CommonModule} from "@angular/common";

@Component({
    selector: 'app-common-loader',
    standalone: true,
    imports: [
        MatProgressSpinnerModule,
        CommonModule
    ],
    templateUrl: './common-loader.component.html',
    styleUrls: ['./common-loader.component.scss'] // Corrected property name
})
export class CommonLoaderComponent implements OnInit {
    @Input() diameter: number | null = 50;

    public isBusy: boolean | undefined;

    constructor(
        private readonly loaderService: LoaderService
    ) {
    }

    ngOnInit(): void {
        this.loaderService.loaderIsBusyChanged$.pipe(
            tap((value) => {
                this.isBusy = value;
            })
        ).subscribe();
    }
}