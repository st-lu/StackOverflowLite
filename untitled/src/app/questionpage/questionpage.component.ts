import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionService } from "../service/question.service";
import { Question } from '../models/question.model';
import {KeycloakService} from "keycloak-angular";
import {UserService} from "../service/user.service";
import {HttpErrorResponse} from "@angular/common/http";
import {AnswerService} from "../service/answer.service";
import {AnswerDto} from "../models/answer.model";

@Component({
  selector: 'app-questionpage',
  templateUrl: './questionpage.component.html',
  styleUrls: ['./questionpage.component.css']
})
export class QuestionpageComponent implements OnInit {
  question: Question | null = null; // Holds the question data
  questionId: string = '';
  answers: any[] = []; // Holds the list of answers
  authorId: string | undefined = '';
  isLoading: boolean = true; // Spinner visibility
  errorMessage: string | null = null; // Error message placeholder
  isEditing: boolean = false; // Track if the user is editing
  updatedQuestionContent: string = ''; // Hold the updated question input
  isOwner: boolean = false; // Track ownership status in the component
  isAddingAnswer: boolean = false; // Track if the user is adding an answer
  newAnswerContent: string = ''; // For the new answer input
  currentUsername: string = ''; // Holds the current user's username
  isAdmin: boolean = false; // Track admin status

  constructor(
    private route: ActivatedRoute,
    private questionService: QuestionService,
    private userService: UserService,
    private answerService: AnswerService
  ) {}

  ngOnInit(): void {
    // Get questionId from the route parameters
    const questionId = this.route.snapshot.paramMap.get('id');
    //const token = this.authService.getToken(); // Fetch the user's token from AuthService
    //console.log(localStorage);
    //this.loggedInUserId = localStorage.getItem('userId') || '';

    if (questionId /*&& token*/) {
      // Call the service to fetch the question
      this.loadQuestion(questionId/*, token*/);
      this.loadCurrentUser();
    } else {
      this.handleError('Invalid question ID or missing authentication token.');
    }
  }

  loadCurrentUser(): void {
    this.userService.getCurrentUser().subscribe({
      next: (user) => {
        this.currentUsername = user.username; // Assuming the backend sends { username: '...' }
        this.isAdmin = user.roles.includes('admin'); // Determine if the user has an admin role
      },
      error: (error) => {
        console.error('Failed to load current user:', error);
        alert('Failed to load user information.');
      },
    });
  }

  loadQuestion(questionId: string/*, token: string*/): void {
    this.questionService.getQuestionById(questionId/*, token*/).subscribe({
      next: (response) => {
        this.question = response; // Set the question data
        this.questionId = questionId;
        this.answers = response.answers.map(answer => ({ ...answer, isEditing: false, editedContent: answer.content }));
        console.log(this.answers);
        this.isLoading = false;
        this.authorId = this.question?.userId;
        this.isLoading = false; // Stop the spinner
        console.log(this.question);
        this.checkUserOwnership(); // Check ownership
      },
      error: (error) => {
        if (error.status === 404) {
          this.errorMessage = 'Question rejected for violating terms of service.'; // Handle 404 specifically
        } else {
          this.errorMessage = 'An unexpected error occurred while loading the question.';
        }
        this.question = null; // Ensure no question data is available
        this.isLoading = false; // Stop spinner
      },
    });
  }

  enableAnswerEdit(answer: any): void {
    // Open the inline edit form
    answer.isEditing = true;
  }

  cancelAnswerEdit(answer: any): void {
    // Restore the original content and close the form
    answer.isEditing = false;
    answer.editedContent = answer.content;
  }

  saveAnswerEdit(answer: any): void {
    // API call to update answer
    const updatedAnswer = { content: answer.editedContent };

    this.answerService.editAnswer(answer.id, updatedAnswer).subscribe({
      next: () => {
        answer.content = answer.editedContent; // Update the content locally
        answer.isEditing = false; // Exit editing mode
      },
      error: (error :any) => {
        console.error('Failed to edit the answer:', error);
        alert('Failed to save the changes. Please try again.');
      }
    });
  }

  deleteQuestionAsAdmin(): void {
    if (!confirm('Are you sure you want to delete this question as an admin? This action cannot be undone.')) return;

    this.questionService.adminDeleteQuestion(this.questionId).subscribe({
      next: () => {
        alert('Question successfully deleted by admin.');
        // Redirect to another page
        // this.router.navigate(['/homepage']);
      },
      error: (error) => {
        console.error('Failed to delete the question as admin:', error);
        alert('Failed to delete the question. Please try again.');
      },
    });
  }

  deleteAnswer(answerId: string): void {
    if (!confirm('Are you sure you want to delete this answer?')) return;

    // Optimistically remove the answer from the UI
    this.answers = this.answers.filter(answer => answer.id !== answerId);

    // Call the backend API to delete the answer
    this.answerService.deleteAnswer(answerId).subscribe({
      next: () => {
        console.log(`Answer with ID ${answerId} deleted successfully.`);
        // The answer is already removed from the UI, no further action required.
      },
      error: (error) => {
        console.error('Failed to delete the answer:', error);
        /*alert('Failed to delete the answer. Please try again.');*/

        // Rollback: Reload the question to restore the answers if delete fails
        this.loadQuestion(this.questionId);
      }
    });
  }

  addAnswer(content: string): void {
    if (!content.trim()) {
      alert('Answer content cannot be empty.');
      return;
    }

    this.isAddingAnswer = true; // Indicate submission is in progress

    const newAnswer = { content }; // Prepare payload for the backend

    this.answerService.createAnswer(this.questionId, newAnswer).subscribe({
      next: (response: AnswerDto) => {
        if (!response) {
          alert('Failed to post the answer. Please try again.');
          return;
        }

        // Build the new `AnswerDto` object, including properties required for edit/delete
        const newAnswerObject = {
          id: response,                         // Backend returns answer ID as string
          content: newAnswer.content,           // Submitted content
          user: {                               // Replace with actual logged-in user data
            id: 'current-user-id',              // Example: Dynamic ID
            username: this.currentUsername,       // Example: Dynamic Username
          },
          score: 0,                             // Default starting value (can be backend-driven)
          isEditing: false,                     // Editing is initially set to false
          editedContent: newAnswer.content,     // Initialize the editable buffer
        };

        // Push the new answer into the `answers` array (for live rendering)
        this.answers.push(newAnswerObject);

        // Sync with `question.answers` if needed for consistency elsewhere
        this.question?.answers?.push(newAnswerObject);

        // Clear the input box for the new answer
        this.newAnswerContent = '';
        this.isAddingAnswer = false; // Exit submission mode
      },
      error: (error) => {
        console.error('Failed to post the answer:', error);
        alert('An error occurred while posting your answer. Please try again.');
        this.isAddingAnswer = false; // Reset state in case of an error
      },
    });
  }

  voteAnswer(answerId: string, voteType: number): void {
    const voteRequest = {
      vote: voteType,
      id: answerId,
    };

    this.answerService.voteAnswer(voteRequest).subscribe({
      next: (updatedAnswer) => {
        // Update the score of the voted answer in the UI
        const answerIndex = this.answers.findIndex(answer => answer.id === answerId);
        if (answerIndex !== -1) {
          this.answers[answerIndex] = updatedAnswer; // Replace with updated answer
        }
      },
      error: (error) => {
        console.error('Failed to vote on the answer:', error);
        alert('An error occurred while voting. Please try again.');
      }
    });
  }

  voteQuestion(voteType:number): void {
    if (!this.question) {
      this.handleError('Unable to vote on this question.');
      return;
    }

    const questionVoteRequest = {
      vote: voteType as number, // Ensure this matches the `Vote` enum (e.g., 'UPVOTE' or 'DOWNVOTE')
      id: this.questionId as string, // Ensure this matches the 'Id' field in the backend
    };

    // Call the vote API
    this.questionService.voteQuestion(questionVoteRequest).subscribe({
      next: (updatedQuestion) => {
        this.question = updatedQuestion; // Update question object with new score
      },
      error: (error) => {
        console.error('Failed to vote:', error);
        this.handleError('An error occurred while voting.');
      },
    });
  }

  // Toggle between edit and view modes
  toggleEditMode(): void {
    this.isEditing = !this.isEditing;
    this.updatedQuestionContent = this.question?.content || ''; // Set the current content for editing
  }

  // Check if the logged-in user is the author of the question
  checkUserOwnership(): void {
    this.userService.getCurrentUser().subscribe({
      next: (currentUser) => {
        console.log("this user ", currentUser);
        console.log("author ", this.authorId);
        this.isOwner = currentUser.id === this.authorId; // Set ownership status
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error retrieving current user:', error);
        this.isOwner = false; // Assume not owner in case of an error
      }
    });

  }

  saveChanges(): void {
    if (!this.question) {
      this.handleError('Unable to save changes. Question not found.');
      return;
    }

    const updatedQuestion = {
      content: this.updatedQuestionContent, // Send the modified question content
    };

    console.log(updatedQuestion);

    this.questionService.updateQuestion(this.questionId as string, updatedQuestion).subscribe({
      next: (response) => {
        this.question = response; // Update the question object with the backend response
        this.isEditing = false; // Exit edit mode
        console.log('Question successfully updated');
      },
      error: (error) => {
        console.error('Failed to update the question:', error);
        this.handleError('An error occurred while updating the question.');
      },
    });
  }

  // Delete an answer as admin
  deleteAnswerAsAdmin(answerId: string): void {
    if (!confirm('Are you sure you want to delete this answer as an admin? This action cannot be undone.')) return;

    this.answerService.adminDeleteAnswer(answerId).subscribe({
      next: () => {
        // Optimistically update the UI to remove the answer
        this.answers = this.answers.filter((answer) => answer.id !== answerId);
        alert('Answer successfully deleted by admin.');
      },
      error: (error) => {
        console.error('Failed to delete the answer as admin:', error);
        alert('Failed to delete the answer. Please try again.');
      },
    });
  }

  deleteQuestion(): void {
    if (!this.questionId) {
      this.handleError('No question ID to delete.');
      return;
    }

    if (confirm('Are you sure you want to delete this question? This action cannot be undone.')) {
      this.questionService.deleteQuestion(this.questionId).subscribe({
        next: () => {
          alert('Question successfully deleted.');
          // Redirect to another page, for example, the question list page
          // e.g., this.router.navigate(['/questions']);
        },
        error: (error) => {
          console.error('Failed to delete the question:', error);
          this.handleError('An error occurred while deleting the question.');
        },
      });
    }
  }

  handleError(message: string): void {
    this.errorMessage = message;
    this.isLoading = false; // Stop spinner on error
  }
}
