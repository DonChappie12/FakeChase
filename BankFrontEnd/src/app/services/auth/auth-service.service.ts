import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {

  constructor(private _http: HttpClient) { }

  registerNewUser(newUser: {}){
    console.log(newUser)
  }
}
