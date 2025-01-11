import { Component } from '@angular/core';
import {KeycloakService} from "keycloak-angular";
import {AppConfigService} from "../service/AppConfigService";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  websiteDomain:string
  public isLoggedIn = false;
  constructor(private readonly keycloak: KeycloakService,
              private appConfig: AppConfigService) {
    this.websiteDomain = this.appConfig.websiteDomain;
  }
  public async ngOnInit() {
    this.isLoggedIn = this.keycloak.isLoggedIn();

  }
  public login() {
    this.keycloak.login({redirectUri: `${this.websiteDomain}/homepage`});
  }

  public logout(): void {
    this.keycloak.logout(`${this.websiteDomain}/register`).then(() => {
      console.log("Logout successful");
    }).catch((error: any) => {
      console.error("Logout failed:", error);
    });
  }
}
