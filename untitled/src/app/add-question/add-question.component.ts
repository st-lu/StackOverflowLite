import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { QuestionService } from "../service/question.service";

@Component({
  selector: 'app-add-question',
  templateUrl: './add-question.component.html',
  styleUrls: ['./add-question.component.css'],
})
export class AddQuestionComponent {
  questionContent: string = '';
  isSubmitting: boolean = false;
  errorMessage: string | null = null;

  constructor(
    private questionService: QuestionService,
    private router: Router
  ) {}

  submitQuestion(): void {
    if (!this.questionContent.trim()) {
      alert('Question content cannot be empty!');
      return;
    }

    this.isSubmitting = true;

    const newQuestion = {
      content: this.questionContent,
    };

    console.log(newQuestion);

    // Submit the question to the backend
    this.questionService.addQuestion(newQuestion).subscribe({
      next: (questionId:string) => {
        console.log('Question created successfully with ID:', questionId);
        this.router.navigate(['/question', questionId]);
      },
      error: (error: any) => {
        console.error('Failed to create the question:', error);
        if (error.status === 401) {
          alert('Unauthorized: Please log in before creating a question.');
        } else if (error.status === 400) {
          alert('Validation error: Please ensure your question content is valid.');
        } else {
          alert('An error occurred while creating the question. Please try again.');
        }
        this.isSubmitting = false;
      },
    });
  }
}
