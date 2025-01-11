import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {Question} from "../models/question.model";

@Injectable()
export class QuestionService {
  private apiUrl = "http://localhost:8080/question"; // Backend URL
  constructor(private http: HttpClient) {}

  getQuestions(offset: number, size: number): Observable<Question[]> {
    return this.http.get<Question[]>(`${this.apiUrl}?offset=${offset}&size=${size}`);
  }

  questions$ = this.getQuestions2();
  getQuestions2(): Observable<Question[]> {
    return this.http.get<Question[]>(`${this.apiUrl}`);
  }
}
