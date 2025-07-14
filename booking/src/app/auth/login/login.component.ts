import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators,ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../../shared/services/api.service';
import { jwtDecode } from 'jwt-decode';
import { MaterialModule } from '../../material/material.module';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
@Component({
  selector: 'login',
  standalone: true,
  imports:[ReactiveFormsModule,MaterialModule, CommonModule,SharedModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {

  loginForm: FormGroup;
  hideContent: boolean = true;
  constructor(private fb: FormBuilder,
    private apiService:ApiService,
    private snackBar:MatSnackBar,
    private router:Router
  ){
    this.loginForm = fb.group({
      email: fb.control('', [Validators.required, Validators.email]),
      password: fb.control('', [Validators.required]),

    });
  }

    login() {
  if (this.loginForm.valid) {
    const loginInfo = this.loginForm.value;
 
    this.apiService.loginUser(loginInfo).subscribe({
      next: (res:any) => {
        localStorage.setItem('token', res.token);
        console.log(jwtDecode(res.token));
        const role = this.getUserRoleFromToken(res.token);
        this.snackBar.open('Login successful!', 'Close', { duration: 3000 });
        if (role === 'ADMIN'){
          this.router.navigate(['/admin']);
        }
        else if (role === 'USER'){
          this.router.navigate(['/dashboard']);
        }
        else{
          this.snackBar.open('Unknown role. Redirect to login.','Close',{duration: 3000});
        }
      },
      error: (err:any) => {
        this.snackBar.open('Invalid credentials. Please try again.', 'Close', { duration: 3000 });
        console.error(err);
      }
    });
  } else {
    this.snackBar.open('Please enter valid credentials.', 'Close', { duration: 3000 });
  }
}

getUserRoleFromToken(token: string): string {

  try {
    const decoded: any = jwtDecode(token);
    return decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  } catch (e) {
    console.error('Invalid token:', e);
    return '';
  }

}

  }
    



