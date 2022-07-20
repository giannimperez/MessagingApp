import { Output, Type } from '@angular/core';
import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      this.cancel();
    }, error => {

      if (error.error.errors) {
        console.log(1);
        const modalStateErrors = [];
        console.log(2);
        for (const key in error.error.errors) {
          console.log(3);
          if (error.error.errors[key]) {
            console.log(4);
            modalStateErrors.push(error.error.errors[key])
            console.log(5);
          }
          this.toastr.error(error.error.errors[key]);
        }
        throw modalStateErrors;
      } else {
        console.log(error);
        this.toastr.error(error.error);
      }
    })
  }

  cancel() {
    console.log('Cancelled');
  }

}
