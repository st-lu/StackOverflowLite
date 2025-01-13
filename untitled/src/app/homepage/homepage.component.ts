import { Component, HostListener, OnInit } from '@angular/core';
import { UserService } from "../service/user.service";
import { Question } from "../models/question.model";
import { QuestionService } from "../service/question.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent implements OnInit {
  questions: Question[] = [];
  isLoading: boolean = false;
  offset: number = 0;
  batchSize: number = 25;
  hasMoreQuestions: boolean = true; // To track if more questions are available

  // Filters
  filters = {
    searchText: '',
    viewsCountOrder: '',
    scoreOrder: ''
  };

  constructor(private userService: UserService, private questionService: QuestionService, private router: Router) {}

  ngOnInit(): void {
    this.loadQuestions();
  }

  loadQuestions(): void {
    if (this.isLoading || !this.hasMoreQuestions) return;

    this.isLoading = true;

    this.questionService
      .getQuestions(
        this.offset,
        this.batchSize,
        this.filters.searchText,
        this.filters.viewsCountOrder,
        this.filters.scoreOrder
      )
      .subscribe({
        next: (data: Question[]) => {
          if (data.length > 0) {
            this.questions = [...this.questions, ...data.reverse()];
            this.offset += data.length;
          } else {
            this.hasMoreQuestions = false;
          }
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
          alert('Failed to load questions.');
        }
      });
  }

  onFilterChange(): void {
    this.offset = 0;
    this.questions = [];
    this.hasMoreQuestions = true;
    this.loadQuestions();
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    const scrollPosition = window.innerHeight + window.scrollY;
    const bottomPosition = document.documentElement.scrollHeight - 100;

    console.log('Scroll position:', scrollPosition);
    console.log('Bottom position:', bottomPosition);

    if (this.isLoading || !this.hasMoreQuestions) return;

    if (scrollPosition >= bottomPosition) {
      console.log('Loading more questions...');
      this.loadQuestions();
    }
  }

  navigateToQuestion(questionId: string): void {
    this.router.navigate(['/question', questionId]);
  }
}
