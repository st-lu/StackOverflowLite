import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AnswerDto } from '../models/answer.model';
import {AppConfigService} from "./AppConfigService";

@Injectable({
  providedIn: 'root'
})
export class AnswerService {
  private apiUrl = this.appConfigService.apiBaseUrl + "/answer"; // Replace with your backend URL

  constructor(private http: HttpClient, private appConfigService: AppConfigService) {}

  createAnswer(questionId: string, answerRequest: { content: string }): Observable<AnswerDto> {
    return this.http.post<AnswerDto>(`${this.apiUrl}?questionId=${questionId}`, answerRequest);
  }

  editAnswer(answerId: string, updatedContent: { content: string }): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${answerId}`, updatedContent);
  }

  deleteAnswer(answerId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${answerId}`);
  }

  voteAnswer(voteRequest: { vote: number; id: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/vote`, voteRequest);
  }

  adminDeleteAnswer(answerId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/admin/${answerId}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`, // Include admin authentication token
      },
    });
  }
}
