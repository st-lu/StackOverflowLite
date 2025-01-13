import { Component, OnInit } from '@angular/core';
import { KeycloakService } from "keycloak-angular";
import {NavigationEnd, Router} from "@angular/router";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  isLoggedIn = false;
  currentRoute: string = '';

  constructor(private readonly keycloakService: KeycloakService, private router: Router) {}

  async ngOnInit(): Promise<void> {
    this.isLoggedIn = await this.keycloakService.isLoggedIn();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.url;
      }
    });
  }


  async login(): Promise<void> {
    await this.keycloakService.login();
    this.isLoggedIn = await this.keycloakService.isLoggedIn();
    if (this.isLoggedIn) {
      await this.router.navigate(['/user']);
    }
  }

  // Logout method
  async logout(): Promise<void> {
    await this.keycloakService.logout();
    this.isLoggedIn = await this.keycloakService.isLoggedIn();
  }
}
