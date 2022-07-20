import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Member } from '../models/member';


  const httpOptions = {
    headers: new HttpHeaders({
      Authorization: 'Bearer' + JSON.parse(localStorage.getItem('user'))?.token
    })
  }


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }


  searchMembers(query: string) {
    return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);
  }

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);
  }

  getMembersByPartialUsername(username: string) {
    if (username == "") {
      return;
    }
    return this.http.get<Member[]>(this.baseUrl + 'users/partialusername/' + username, httpOptions);
  }

}
