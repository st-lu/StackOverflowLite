import { Component, OnInit } from '@angular/core';
import { UserService } from '../service/user.service';

@Component({
  selector: 'app-most-active-users',
  templateUrl: './most-active-users.component.html',
  styleUrls: ['./most-active-users.component.css']
})
export class MostActiveUsersComponent implements OnInit {
  mostActiveUsers: any[] = [];
  errorMessage: string | null = null;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadMostActiveUsers();
  }

  loadMostActiveUsers(): void {
    this.userService.getMostActiveUsers().subscribe({
      next: (users) => {
        this.mostActiveUsers = users;
        console.log(users);
      },
      error: (err) => {
        this.errorMessage = 'You must be logged in to view this page.';
        console.error(err);
      }
    });
  }
}
