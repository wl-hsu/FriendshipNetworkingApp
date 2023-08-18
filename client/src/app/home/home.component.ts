import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';
import { UserParams } from '../_models/userParams';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  users: any;
  model: any = {}
  router: any;
  constructor(public accountService: AccountService) { 
  }

  ngOnInit(): void {
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }


  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
