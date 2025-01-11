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

/*  getUserProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>($
    {
      this.userUrl
    }
    /profile).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 404) {
        return this.createUser();
      }
      throw (error);
    })
  )
    ;
  }*/

  public createUser(): void {
    this.http.post(this.userUrl + "/create-mapping", {})
  }
}
