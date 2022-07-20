import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Message } from '../models/message';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private accountService: AccountService) { }

  getMessages() {
    var currentUser = this.accountService.getCurrentUser().username;

    const response = fetch(this.baseUrl + 'messages/recipient/' + currentUser);

/*    console.log(response);*/

/*    const response = this.http.get(this.baseUrl + 'messages/recipient/' + currentUser).pipe(
        map((messages: Message[]) => {

            console.log(messages);

        }))*/

    return response;
    
  }
}
