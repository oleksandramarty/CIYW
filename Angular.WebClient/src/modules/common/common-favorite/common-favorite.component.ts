import {Component, Input} from '@angular/core';
import {SharedModule} from '../../../core/shared.module';
import {CommonModule} from "@angular/common";

@Component({
    selector: 'app-common-favorite',
    standalone: true,
    imports: [
        CommonModule,
        SharedModule
    ],
    templateUrl: './common-favorite.component.html',
    styleUrls: ['./common-favorite.component.scss']
})
export class CommonFavoriteComponent {
    @Input() title: string | undefined;
    @Input() favorite: boolean | undefined;
    isHovered: boolean = false;

    onMouseEnter() {
        this.isHovered = true;
    }

    onMouseLeave() {
        this.isHovered = false;
    }
}