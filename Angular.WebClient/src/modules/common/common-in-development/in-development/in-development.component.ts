import { Component } from '@angular/core';
import {SharedModule} from "../../../../core/shared.module";

@Component({
  selector: 'app-in-development',
  standalone: true,
  imports: [
    SharedModule
  ],
  templateUrl: './in-development.component.html',
  styleUrl: '../../common-app/not-found/not-found.component.scss'
})
export class InDevelopmentComponent {

}
