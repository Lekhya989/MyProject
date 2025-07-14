import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../material/material.module';
import { TaxprosComponent } from '../taxpros/taxpros.component';
import { TimeslotsComponent } from '../timeslots/timeslots.component';
import { SharedModule } from '../../shared/shared.module';
import { UpcomingBookingsComponent } from '../../shared/upcoming-bookings/upcoming-bookings.component';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, MaterialModule, TaxprosComponent, TimeslotsComponent, SharedModule,UpcomingBookingsComponent],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent {}
