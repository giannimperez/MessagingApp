import { Component, OnInit } from '@angular/core';
import { FriendsComponent } from '../friends/friends.component';
import { Message } from '../models/message';
import { AccountService } from '../services/account.service';
import { MessageService } from '../services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'
  ]
})
export class MessagesComponent implements OnInit {
  messages: Message[];

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {

    this.messageService.getMessages();
  }





  selectFriend() {
    console.log("Clicked friend.");
  }

}
