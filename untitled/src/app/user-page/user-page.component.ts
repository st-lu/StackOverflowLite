import { Component, OnInit } from '@angular/core';
import { UserService } from "../service/user.service";
import { HttpErrorResponse } from '@angular/common/http';
import {Router} from "@angular/router";

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.css']
})
export class UserPageComponent implements OnInit {
  user: any = null;
  userQuestions: any[] = []; // Holds the list of user's questions
  isLoadingQuestions: boolean = true; // Indicates questions fetching status

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.fetchUser();
    this.fetchUserQuestions();
  }

  fetchUser(): void {
    this.userService.getCurrentUser().subscribe({
      next: (data) => {
        this.user = data; // Display user data if valid
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 404 || error.status === 401) {
          // If user is not valid, retry creating the user
          this.userService.createUser();
          this.fetchUser();
        } else {
          console.error('An unexpected error occurred:', error);
        }
      }
    });
  }

  fetchUserQuestions(): void {
    this.userService.getUserQuestions().subscribe({
      next: (data) => {
        this.userQuestions = data; // Store user's questions
        this.isLoadingQuestions = false; // Stop the loading spinner
      },
      error: (error: HttpErrorResponse) => {
        console.error('Failed to load user questions:', error);
        this.isLoadingQuestions = false; // Stop the spinner even if there's an error
      }
    });
  }

  navigateToQuestion(questionId: string): void {
    this.router.navigate(['/question', questionId]); // Navigate to the question page
  }
}
