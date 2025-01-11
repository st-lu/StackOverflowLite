import { BrowserModule } from '@angular/platform-browser';
import {APP_INITIALIZER, NgModule} from '@angular/core';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpClientModule} from "@angular/common/http";

import {KeycloakAngularModule, KeycloakService} from "keycloak-angular";
import {AppConfigService} from "./service/AppConfigService";
import { RegisterComponent } from './register/register.component';

function initializeApp(appConfigService: AppConfigService) {
  return () => appConfigService.loadAppConfig();
}

function initializeKeycloak(keycloak: KeycloakService, appConfigService: AppConfigService) {
  return () =>
    appConfigService.loadAppConfig().then(() => {
      return keycloak.init({
        config: {
          realm: 'Stackoverflow-Lite',
          url: appConfigService.keycloakUrl,
          clientId: 'LiteClientID'
        },
        initOptions: {
          checkLoginIframe: false,
          onLoad: 'check-sso',
          silentCheckSsoRedirectUri:
            window.location.origin + '/assets/silent-check-sso.html'
        }
      });
    });
}

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    KeycloakAngularModule,

  ],
  providers: [{
    provide: APP_INITIALIZER,
    useFactory: initializeApp,
    multi: true,
    deps: [AppConfigService]
  },
    {
      provide: APP_INITIALIZER,
      useFactory: initializeKeycloak,
      multi: true,
      deps: [KeycloakService, AppConfigService]
    }],
  bootstrap: [AppComponent]
})



export class AppModule { }
