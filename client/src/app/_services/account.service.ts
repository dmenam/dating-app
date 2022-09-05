import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = 'https://localhost:5001/api/'

  //Type of observable that returns the last value from the array
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post(this.baseUrl + 'account/login', model).pipe( //Filter the data received from the API

      //Using the map function to get the data
      map((response: User) => {

        const user = response;

        //Validate if the user was received or if it is null
        if (user) {
          //Saving the user into the local storage (Web browser)
          localStorage.setItem("user", JSON.stringify(user));

          //Adding the gotten user to our observable array (Which has a size of 1)
          this.currentUserSource.next(user);
        }
      })
    )
  }

  //Helper method
  setCurrentUser(user : User){
    this.currentUserSource.next(user);
  }

  //Login out method
  logout(){
    //Remove user from browser 
    localStorage.removeItem("user");

    //Remove the user from the observable
    this.currentUserSource.next(null);
  }
}
