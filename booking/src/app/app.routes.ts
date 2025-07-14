import { Routes } from '@angular/router';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import { AuthGuard } from './auth/auth.guard';
import { AdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './user/user-dashboard/user-dashboard.component';
import { PageSidenavComponent } from './shared/components/page-sidenav/page-sidenav.component';
import { TimeslotsComponent } from './admin/timeslots/timeslots.component';
import { TaxprosComponent } from './admin/taxpros/taxpros.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { MyBookingsComponent } from './user/my-bookings/my-bookings.component';
import { SlotSelectionComponent } from './user/slot-selection/slot-selection.component';
import { UserBookingsComponent } from './admin/user-bookings/user-bookings.component';
import { BookingConfirmationsComponent } from './admin/booking-confirmations/booking-confirmations.component';
import { UserComponent } from './user/user.component';

 
export const routes: Routes = [

  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: '',
    component: PageSidenavComponent,
    canActivate: [AuthGuard],
    children: [
        {
      path: 'dashboard',
      component: UserDashboardComponent,
      data: { roles: ['USER'] },
      children: [
        { path: 'user-dashboard', component: UserComponent},
        { path: 'my-bookings', component: MyBookingsComponent },
        { path: 'slot-selection', component: SlotSelectionComponent },
        { path: '', redirectTo: 'my-bookings', pathMatch: 'full' },
        
      ]
    },

      {
        path: 'admin',
        component: AdminDashboardComponent,
        data: { roles: ['ADMIN'] },
        
      },
      {
        path: 'admin/time-slot',
        component:TimeslotsComponent,
        canActivate: [AuthGuard],
        data:{roles: ['ADMIN']}

      },
      {
        path: 'admin/manage-tax-pros',
        component:TaxprosComponent,
        canActivate: [AuthGuard],
        data:{roles: ['ADMIN']}

      },

      {
        path: 'admin/user-bookings',
        component:UserBookingsComponent,
        canActivate:[AuthGuard],
        data:{roles:['ADMIN']}
      },

      {
        path: 'admin/booking-confirmation',
        component:BookingConfirmationsComponent,
        canActivate:[AuthGuard],
        data:{roles:['ADMIN']}
      },
      {
        path: 'admin/admin-dashboard',
        component:AdminDashboardComponent,
        canActivate:[AuthGuard],
        data:{roles:['ADMIN']}
      },
     
    ]
  },

  {path:'', redirectTo:'login', pathMatch:'full'},
 
  { path: '**', redirectTo:'login' }

];

 