import { Component, OnInit } from '@angular/core';
import { Member } from '../../models/member';
import { MembersService } from '../../services/members.service';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {

  members: Array<Member> = [];

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    
  }

/*  loadMembers() {

    this.memberService.getMembers().subscribe(members => {
      //this.members = members;
    })
  }*/

  sendQuery(event: any) {
    let query: string = event.target.value;

    if (query == "") {
      this.members = [];
    }

    this.memberService.getMembersByPartialUsername(query.trim()).subscribe(results => {
      this.members = results;
    })
  }

}
