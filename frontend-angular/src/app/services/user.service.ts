import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

export interface User {
  id: number;
  fullName: string;
  email: string;

}
@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly url = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.url}/get-users`);
  }

  createUser(request: {}): Observable<any>{
    return this.http.post(`${this.url}/create-user`, request)
  }
}
