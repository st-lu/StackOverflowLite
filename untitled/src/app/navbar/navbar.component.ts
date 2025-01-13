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
    this.isLoggedIn = await this.keycloakService.isLoggedIn(); // Check login status
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.url; // Update the current route
      }
    });
  }


  async login(): Promise<void> {
    await this.keycloakService.login(); // Redirects to Keycloak login
    this.isLoggedIn = await this.keycloakService.isLoggedIn(); // Update login status
    if (this.isLoggedIn) {
      await this.router.navigate(['/user']); // Navigate to the user page after login
    }
  }

  // Logout method
  async logout(): Promise<void> {
    await this.keycloakService.logout(); // Redirects to Keycloak logout
    this.isLoggedIn = await this.keycloakService.isLoggedIn(); // Update login status
  }
}
