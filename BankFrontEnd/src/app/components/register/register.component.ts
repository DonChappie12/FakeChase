import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {

  registerUser = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(3)]],
    middleName: [''],
    // lastName: ['', Validators.required],
    // email: ['', [Validators.required, Validators.email]],
    // userName: ['', Validators.required]
  })

  constructor(private fb: FormBuilder) {}

  onSubmit(){
    console.log(this.registerUser.value)
    console.log(this.registerUser.errors)
  }

}
