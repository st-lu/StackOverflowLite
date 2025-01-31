import { BrowserModule } from '@angular/platform-browser';
import {APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpClientModule} from "@angular/common/http";

import {KeycloakAngularModule, KeycloakService} from "keycloak-angular";
import {AppConfigService} from "./service/AppConfigService";
import { RegisterComponent } from './register/register.component';
import { NavbarComponent } from './navbar/navbar.component';
import { HomepageComponent } from './homepage/homepage.component';
import {UserService} from "./service/user.service";
import {QuestionService} from "./service/question.service";
import { QuestionpageComponent } from './questionpage/questionpage.component';
import {FormsModule} from "@angular/forms";
import { AddQuestionComponent } from './add-question/add-question.component';
import { UserPageComponent } from './user-page/user-page.component';
import { MostActiveUsersComponent } from './most-active-users/most-active-users.component';

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
    RegisterComponent,
    NavbarComponent,
    HomepageComponent,
    QuestionpageComponent,
    AddQuestionComponent,
    UserPageComponent,
    MostActiveUsersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    KeycloakAngularModule,
    FormsModule,

  ],
  providers: [UserService, QuestionService, {
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
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})



export class AppModule { }
