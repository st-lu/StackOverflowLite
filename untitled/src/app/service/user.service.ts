import {Injectable} from "@angular/core";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {catchError, concatMap, Observable, of, tap} from "rxjs";


import {AppConfigService} from "./AppConfigService";

@Injectable()
export class UserService {
  //TODO add retry mechanism

  private userUrl: string;

  constructor(private http: HttpClient,
              private appConfigService: AppConfigService) {
    this.userUrl = this.appConfigService.apiBaseUrl;
  }

  public createUser(): void {
    this.http.post(this.userUrl + "/create-mapping", {}).subscribe({
      next: () => console.log('User created successfully'),
      error: (err) => console.error('An error occurred while creating the user.', err)
    });
  }

  public getCurrentUser(): Observable<any> {
    return this.http.get(this.userUrl + "/current");
  }

  public isAdmin(): Observable<any> {
    return this.http.get(this.userUrl + "/is-admin"); // Call the backend to retrieve the current user
  }

  public getUserQuestions(): Observable<any[]> {
    return this.http.get<any[]>(`${this.userUrl}/me/questions`);
  }

  public getMostActiveUsers(): Observable<any> {
    return this.http.get<any[]>(`${this.userUrl}/most-active-users`).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('Error fetching most active users', error);
        return of([]);
      })
    );
  }
}
