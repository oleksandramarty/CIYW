import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './modules/areas/app/app.component';
import { config } from './modules/areas/app/app.config.server';

const bootstrap = () => bootstrapApplication(AppComponent, config);

export default bootstrap;
