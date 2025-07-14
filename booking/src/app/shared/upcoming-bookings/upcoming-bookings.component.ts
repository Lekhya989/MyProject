import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../material/material.module';

@Component({
  selector: 'app-upcoming-bookings',
  templateUrl: './upcoming-bookings.component.html',
  styleUrls: ['./upcoming-bookings.component.scss'],
  imports:[CommonModule, MaterialModule]
})
export class UpcomingBookingsComponent implements OnInit {

  upcomingBookings: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getUpcomingBookings().subscribe({
      next: (data) => {
        const now = new Date();
        this.upcomingBookings = data.filter((booking:any)=>{
          const endTime = new Date(booking.slot?.endTime);
          return endTime > now;
        });
      },
      error: (err) => {
        console.error("Failed to load upcoming bookings", err);
      }
    });
  }
}
