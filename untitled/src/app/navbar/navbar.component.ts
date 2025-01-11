import {Component, OnInit} from '@angular/core';
import {KeycloakService} from "keycloak-angular";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{
  isLoggedIn = false;
  constructor(private readonly keycloakService: KeycloakService) {}

  ngOnInit(): void {
    this.isLoggedIn = this.keycloakService.isLoggedIn();
  }
}
