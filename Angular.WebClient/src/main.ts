import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './modules/areas/app/app.config';
import { AppComponent } from './modules/areas/app/app.component';

if (environment.production) {
    enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
    .catch(err => console.error(err));
