
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  model: any = {}
  
  //loggedIn: boolean; //DEPRECATED - Using the async pipe methond on angular 
  currentUser$: Observable<User>;


  constructor(private accountService : AccountService) {}

  ngOnInit(): void {
    //Excecuted when the component is created
    //this.getCurrentUser(); //DEPRECATED - Using the async pipe methond on angular 
    this.currentUser$ = this.accountService.currentUser$;
  }

  login(){
    this.accountService.login(this.model).subscribe({
      
      next: response => {
        console.log(response)
        //this.loggedIn = true; DEPRECATED - Using the async pipe methond on angular 
      },
      error: e => console.log(e)
    })
  }

  logout(){
    this.accountService.logout();
    //this.loggedIn = false; DEPRECATED - Using the async pipe methond on angular 
  }

/*  DEPRECATED - Using the async pipe methond on angular 
    getCurrentUser(){
    //Look for the user in the observable
    this.acountService.currentUser$.subscribe(
    {
      //Two exclamation marks will "convert" the object into a boolean
      //first check if the object is not null, will return false
      //if it is not null, it will return true
      next : user => this.loggedIn = !!user,
      error : e => console.log(e)
    });
  } */
}
