import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import {Question} from "../models/question.model";
import {AppConfigService} from "./AppConfigService";
import {QuestionStatus} from "../models/questionStatus.model";

@Injectable()
export class QuestionService {
  private apiUrl =  this.appConfigService.apiBaseUrl + "/question";
  constructor(private http: HttpClient, private appConfigService: AppConfigService) {}

  getQuestions(offset: number, size: number, searchText?: string, viewsCountOrder?: string, scoreOrder?: string): Observable<Question[]> {
    let params = `offset=${offset}&size=${size}`;

    if (searchText) {
      params += `&searchText=${encodeURIComponent(searchText)}`;
    }
    if (viewsCountOrder) {
      params += `&viewsCountOrder=${viewsCountOrder}`;
    }
    if (scoreOrder) {
      params += `&scoreOrder=${scoreOrder}`;
    }
    console.log(`Request URL: ${this.apiUrl}?${params}`);
    return this.http.get<Question[]>(`${this.apiUrl}?${params}`);
  }

  getQuestionById(questionId: string/*, token: string*/): Observable<Question> {
    console.log(`${this.apiUrl}/${questionId}`);
    return this.http.get<Question>(`${this.apiUrl}/${questionId}`);
  }
  getQuestionStatus(questionId: string/*, token: string*/): Observable<QuestionStatus> {
    console.log(`${this.apiUrl}/${questionId}`);
    return this.http.get<QuestionStatus>(`${this.apiUrl}/status/${questionId}`);
  }

  voteQuestion(questionVoteRequest: { vote: number; id: string }): Observable<Question> {
    return this.http.post<Question>(`${this.apiUrl}/vote`, questionVoteRequest);
  }

  updateQuestion(questionId: string, updatedQuestion: { content: string }): Observable<Question> {
    return this.http.put<Question>(`${this.apiUrl}/${questionId}`, updatedQuestion);
  }

  addQuestion(question: { content: string }): Observable<any> {
    return this.http.post<any>(this.apiUrl, question, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`, // Ensure token is passed for authentication
      },
    });
  }

  deleteQuestion(questionId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${questionId}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`, // Add auth token if required
      },
    });
  }

  adminDeleteQuestion(questionId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/admin/${questionId}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`, // Include admin authentication token
      },
    });
  }

}
