import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthServiceService } from 'src/app/services/auth/auth-service.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit{

  registerUser = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(3)]],
    middleName: [''],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    userName: ['', [Validators.required, Validators.minLength(5)]],
    password: ['', [Validators.required, Validators.minLength(5)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(5)]]
  })

  constructor(private fb: FormBuilder, private authService: AuthServiceService) {}

  ngOnInit(): void {
    // this.registerUser = this.fb.group({
    //   firstName: ['', [Validators.required, Validators.minLength(3)]],
    //   middleName: [''],
    //   lastName: ['', Validators.required],
    //   email: ['', [Validators.required, Validators.email]],
    //   userName: ['', [Validators.required, Validators.minLength(5)]],
    //   password: ['', [Validators.required, Validators.minLength(5)]],
    //   confirmPassword: ['', [Validators.required, Validators.minLength(5)]]
    // }, {
    //   asyncValidator: this.ConfirmedValidator('password', 'confirmPassword')
    // })
  }

  onSubmit(){
    // console.log(this.registerUser.value)
    // console.log(delete this.registerUser.value['confirmPassword'])
    // console.log(this.registerUser.value)
    this.authService.registerNewUser(this.registerUser.value)
  }
  private ConfirmedValidator(controlName: string, matchingControlName: string){
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[controlName];
        const matchingControl = formGroup.controls[matchingControlName];
        if (matchingControl.errors && !matchingControl.errors['confirmedValidator']) {
            return;
        }
        if (control.value !== matchingControl.value) {
            matchingControl.setErrors({ confirmedValidator: true });
        } else {
            matchingControl.setErrors(null);
        }
    }
  }

  // private password(formGroup: FormGroup) {
  //   const { value: password } = formGroup.get('password');
  //   const { value: confirmPassword } = formGroup.get('confirmpassword');
  //   return password === confirmPassword ? null : { passwordNotMatch: true };
  // }

}
