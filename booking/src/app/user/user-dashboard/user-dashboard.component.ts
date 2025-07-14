import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '../../material/material.module';
import { CommonModule } from '@angular/common';
import { UpcomingBookingsComponent } from '../../shared/upcoming-bookings/upcoming-bookings.component';

 
@Component({
  selector: 'user-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, MaterialModule,UpcomingBookingsComponent],
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss']
})
export class UserDashboardComponent {}