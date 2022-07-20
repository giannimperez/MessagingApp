import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-friends-menu',
  templateUrl: './friends-menu.component.html',
  styleUrls: ['./friends-menu.component.css']
})
export class FriendsMenuComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  selectFriend() {
    console.log("Clicked friend.");
  }
}
