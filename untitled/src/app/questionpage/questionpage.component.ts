import {Component, OnDestroy, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionService } from "../service/question.service";
import { Question } from '../models/question.model';

import {UserService} from "../service/user.service";
import {HttpErrorResponse} from "@angular/common/http";
import {AnswerService} from "../service/answer.service";
import {AnswerDto} from "../models/answer.model";
import {interval, Subscription, switchMap, takeWhile, tap} from "rxjs";


@Component({
  selector: 'app-questionpage',
  templateUrl: './questionpage.component.html',
  styleUrls: ['./questionpage.component.css']
})
export class QuestionpageComponent implements OnInit, OnDestroy {
  question: Question | null = null;
  questionId: string = '';
  answers: any[] = [];
  authorId: string | undefined = '';
  isLoading: boolean = true;
  errorMessage: string | null = null;
  isEditing: boolean = false;
  updatedQuestionContent: string = '';
  isOwner: boolean = false;
  isAddingAnswer: boolean = false;
  newAnswerContent: string = '';
  currentUsername: string = '';
  isAdmin: boolean = false;
  pollingSubscription: Subscription | null = null;


  constructor(
    private route: ActivatedRoute,
    private questionService: QuestionService,
    private userService: UserService,
    private answerService: AnswerService
  ) {}

  ngOnDestroy(): void {
    if (this.pollingSubscription) {
      this.pollingSubscription.unsubscribe();
    }
  }
  ngOnInit(): void {
    // @ts-ignore
    this.questionId = this.route.snapshot.paramMap.get('id');
    if (this.questionId) {
      this.checkQuestionStatus(this.questionId);
    } else {
      console.error('questionId lipsă sau invalid');
      this.handleError('Invalid question ID or missing authentication token.');
    }
  }

  private checkQuestionStatus(questionId: string): void {
    this.questionService.getQuestionStatus(questionId).subscribe({
      next: (status) => {
        console.log(`Status inițial: \${JSON.stringify(status)}`);
        if (status.processed) {
          console.log('Întrebarea este deja procesată');
          this.loadQuestion(questionId);
          this.loadCurrentUser();
        } else {
          console.log('Întrebarea nu este procesată, începe polling-ul');
          this.startPolling(questionId);
        }
      },
      error: (error) => {
        console.error('Eroare la verificarea stării inițiale:', error);
        this.handleError('Eroare la verificarea stării întrebării');
      }
    });
  }

  private startPolling(questionId: string): void {
    this.pollingSubscription = interval(2000)
      .pipe(
        tap(() => console.log('Polling interval declanșat')),
        switchMap(() => this.questionService.getQuestionStatus(questionId)),
        tap(status => console.log(`Status primit: \${JSON.stringify(status)}`))
      )
      .subscribe({
        next: (status) => {
          if (status.processed) {
            console.log('Întrebarea a fost procesată');
            this.pollingSubscription?.unsubscribe();

            setTimeout(() => {
              console.log('Începe încărcarea întrebării după 2 secunde');
              this.loadQuestion(questionId);
              this.loadCurrentUser();
            }, 2000);
          } else {
            console.log('Întrebarea încă nu este procesată');
          }
        },
        error: (error) => {
          console.error('Eroare în timpul polling-ului:', error);
          this.handleError('Eroare la verificarea stării întrebării');
        }
      });
  }

  loadCurrentUser(): void {
    this.userService.getCurrentUser().subscribe({
      next: (user) => {
        this.currentUsername = user.username;
      },
      error: (error) => {
        console.error('Failed to load current user:', error);
        alert('Failed to load user information.');
      },
    });

    this.userService.isAdmin().subscribe({
      next: (isAdmin:any) => {
        this.isAdmin = isAdmin;
      },
      error: (error) => {
        console.error('Failed to get role for the current user:', error);
      },
    });
  }

  loadQuestion(questionId: string): void {
    this.questionService.getQuestionById(questionId).subscribe({
      next: (response) => {
        this.question = response;
        this.questionId = questionId;
        this.answers = response.answers.map(answer => ({ ...answer, isEditing: false, editedContent: answer.content }));
        console.log(this.answers);
        this.isLoading = false;
        this.authorId = this.question?.userId;
        console.log(this.question);
        this.checkUserOwnership();
      },
      error: (error) => {
        if (error.status === 404) {
          this.errorMessage = 'Question rejected for violating terms of service.';
        } else {
          this.errorMessage = 'An unexpected error occurred while loading the question.';
        }
        this.question = null;
        this.isLoading = false;
      },
    });
  }

  enableAnswerEdit(answer: any): void {
    answer.isEditing = true;
  }

  cancelAnswerEdit(answer: any): void {
    answer.isEditing = false;
    answer.editedContent = answer.content;
  }

  saveAnswerEdit(answer: any): void {
    const updatedAnswer = { content: answer.editedContent };

    this.answerService.editAnswer(answer.id, updatedAnswer).subscribe({
      next: () => {
        answer.content = answer.editedContent;
        answer.isEditing = false;
      },
      error: (error :any) => {
        console.error('Failed to edit the answer:', error);
        alert('Failed to save the changes. Please try again.');
      }
    });
  }

  addAnswer(content: string): void {
    if (!content.trim()) {
      alert('Answer content cannot be empty.');
      return;
    }

    this.isAddingAnswer = true;

    const newAnswer = { content };

    this.answerService.createAnswer(this.questionId, newAnswer).subscribe({
      next: (response: AnswerDto) => {
        if (!response) {
          alert('Failed to post the answer. Please try again.');
          return;
        }

        const newAnswerObject = {
          id: response,
          content: newAnswer.content,
          user: {
            id: 'current-user-id',
            username: this.currentUsername,
          },
          score: 0,
          isEditing: false,
          editedContent: newAnswer.content,
        };

        this.answers.push(newAnswerObject);
        this.question?.answers?.push(newAnswerObject);

        this.newAnswerContent = '';
        this.isAddingAnswer = false;
      },
      error: (error) => {
        console.error('Failed to post the answer:', error);
        alert('An error occurred while posting your answer. Please try again.');
        this.isAddingAnswer = false;
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
        const answerIndex = this.answers.findIndex(answer => answer.id === answerId);
        if (answerIndex !== -1) {
          this.answers[answerIndex] = updatedAnswer;
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
      vote: voteType as number,
      id: this.questionId as string,
    };

    this.questionService.voteQuestion(questionVoteRequest).subscribe({
      next: (updatedQuestion) => {
        this.question = updatedQuestion;
      },
      error: (error) => {
        console.error('Failed to vote:', error);
        this.handleError('An error occurred while voting.');
      },
    });
  }

  toggleEditMode(): void {
    this.isEditing = !this.isEditing;
    this.updatedQuestionContent = this.question?.content || '';
  }

  checkUserOwnership(): void {
    this.userService.getCurrentUser().subscribe({
      next: (currentUser) => {
        console.log("this user ", currentUser);
        console.log("author ", this.authorId);
        this.isOwner = currentUser.id === this.authorId;
      },
      error: (error: HttpErrorResponse) => {
        console.error('Error retrieving current user:', error);
        this.isOwner = false;
      }
    });

  }

  saveChanges(): void {
    if (!this.question) {
      this.handleError('Unable to save changes. Question not found.');
      return;
    }

    const updatedQuestion = {
      content: this.updatedQuestionContent,
    };

    console.log(updatedQuestion);

    this.questionService.updateQuestion(this.questionId as string, updatedQuestion).subscribe({
      next: (response) => {
        this.question = response;
        this.isEditing = false;
        console.log('Question successfully updated');
      },
      error: (error) => {
        console.error('Failed to update the question:', error);
        this.handleError('An error occurred while updating the question.');
      },
    });
  }
  deleteAnswer(answerId: string): void {
    if (!confirm('Are you sure you want to delete this answer?')) return;

    this.answers = this.answers.filter(answer => answer.id !== answerId);

    this.answerService.deleteAnswer(answerId).subscribe({
      next: () => {
        console.log(`Answer with ID ${answerId} deleted successfully.`);
      },
      error: (error) => {
        console.error('Failed to delete the answer:', error);
        this.loadQuestion(this.questionId);
      }
    });
  }


  deleteAnswerAsAdmin(answerId: string): void {
    if (!confirm('Are you sure you want to delete this answer as an admin? This action cannot be undone.')) return;

    this.answers = this.answers.filter(answer => answer.id !== answerId);

    this.answerService.adminDeleteAnswer(answerId).subscribe({
      next: () => {
        alert('Answer successfully deleted by admin.');
      },
      error: (error) => {
        console.error('Failed to delete the answer as admin:', error);
        this.loadQuestion(this.questionId);
      },
    });
  }

  deleteQuestionAsAdmin(): void {
    if (!confirm('Are you sure you want to delete this question as an admin? This action cannot be undone.')) return;

    this.questionService.adminDeleteQuestion(this.questionId).subscribe({
      next: () => {
        alert('Question successfully deleted by admin.');
      },
      error: (error) => {
        console.error('Failed to delete the question as admin:', error);
        alert('Failed to delete the question. Please try again.');
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
    this.isLoading = false;
  }

}
