import {Component, Input} from '@angular/core';
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";

@Component({
  selector: 'app-common-loader',
  standalone: true,
  imports: [
    MatProgressSpinnerModule
  ],
  templateUrl: './common-loader.component.html',
  styleUrl: './common-loader.component.scss'
})
export class CommonLoaderComponent {
  @Input() diameter: number | null = 50;
}
