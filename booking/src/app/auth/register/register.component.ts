import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../../shared/services/api.service';
import { Subject, takeUntil } from 'rxjs';
import { Router } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../../material/material.module';

@Component({
  selector: 'register',
  standalone: true,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  imports: [SharedModule,CommonModule,ReactiveFormsModule,MaterialModule]
})
export class RegisterComponent implements OnDestroy {
  hideContent: boolean=true;
  hiderContent: boolean=true;
  registerForm: FormGroup;
  destroyed = new Subject()

  constructor(private fb: FormBuilder, private route: Router,
    private apiService: ApiService, private snackBar: MatSnackBar){
    this.registerForm = fb.group({
      firstName: fb.control('', [Validators.required]),
      lastName: fb.control('', [Validators.required]),
      email: fb.control('', [Validators.required, Validators.email]),
      mobileNumber: fb.control('', [Validators.required]),
      userType: fb.control('ADMIN',[Validators.required]),
      password: fb.control('', [Validators.required]),
      cpassword: fb.control('', [Validators.required]),
    });
  }

  registerUser() {
  if (this.registerForm.valid) {
    if (this.registerForm.value.password !== this.registerForm.value.cpassword){
      this.snackBar.open('Password do not match.','close',{duration:3000});
      return;
    }
    let user = this.registerForm.value;
    this.apiService.registerUser(user)
      .pipe(takeUntil(this.destroyed))
      .subscribe({
        next: (res) => {
          console.log("res...", res);
          this.snackBar.open('Registered Successfully', 'Close', { duration: 3000 });
          this.route.navigateByUrl('/login');
        },
        
      });
  } else {
    this.snackBar.open('Please fill all required fields.', 'Close', { duration: 3000 });
  }
}

  ngOnDestroy(): void {
    //this.destroyed.next();
    this.destroyed.complete();
  }

}
