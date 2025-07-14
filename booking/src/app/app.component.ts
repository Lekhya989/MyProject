import { Component, OnInit } from '@angular/core';
import { RouterOutlet,Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SharedModule } from './shared/shared.module';
import { AuthModule } from './auth/auth.module';
import { AdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './user/user-dashboard/user-dashboard.component';
import { PageSidenavComponent } from './shared/components/page-sidenav/page-sidenav.component';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, SharedModule, AuthModule,AdminDashboardComponent,UserDashboardComponent,PageSidenavComponent,PageNotFoundComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'booking';


  constructor(private router:Router){}

  isAuthRout():boolean{
    return this.router.url === '/login' || this.router.url === '/register';
  }

}
