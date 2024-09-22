import { Component } from '@angular/core';
import {AppCommonModule} from "../../common-app/app-common.module";

@Component({
  selector: 'app-in-development',
  standalone: true,
  imports: [
    AppCommonModule
  ],
  templateUrl: './in-development.component.html',
  styleUrl: '../../common-app/not-found/not-found.component.scss'
})
export class InDevelopmentComponent {

}
