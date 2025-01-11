import {Component, HostListener, OnInit} from '@angular/core';
import {UserService} from "../service/user.service";
import {Question} from "../models/question.model";
import {QuestionService} from "../service/question.service";

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent implements OnInit {
  questions: Question[] = [];
  isLoading: boolean = false;
  offset: number = 0;
  batchSize: number = 5000;

  constructor(private UserService:UserService, private questionService:QuestionService) {
  }
  ngOnInit(): void {
    this.UserService.createUser();
    this.loadQuestions();
  }

  loadQuestions(): void {
    if (this.isLoading) return;
    this.isLoading = true;
    this.questionService.getQuestions(this.offset, this.batchSize).subscribe({
      next: (data: Question[]) => {
        this.questions = [...this.questions, ...data]; // Add the new questions to the existing ones
        this.offset += this.batchSize; // Update the offset for the next batch
        this.isLoading = false;  // Set loading to false after the request completes
      },
      error: () => {
        this.isLoading = false;
        alert('Failed to load questions.');
      }
    });
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    if (this.isLoading) return; // Prevent multiple API calls while loading
    const bottom = window.innerHeight + window.scrollY >= document.documentElement.scrollHeight;
    if (bottom) {
      this.loadQuestions(); // Load more questions when the user reaches the bottom of the page
    }
  }

}
