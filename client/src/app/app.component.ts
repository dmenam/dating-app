import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Dating App';
  users: any;

  constructor(private http: HttpClient, private accountService : AccountService){}
  
  ngOnInit(){
    //Calling methods as the component is started
    this.getUsers();
    this.setCurrentUser();
  }

  setCurrentUser(){
    //Get the user saved in the local storage by the account service
    const user : User = JSON.parse(localStorage.getItem("user"));

    //Pass the user to the service
    this.accountService.setCurrentUser(user);
  }

  getUsers(){
    this.http.get("https://localhost:5001/api/users").subscribe({
      next: response => {
              this.users = response;
              return response;
            },
      error: error => console.log(error)
    })
  }
}
