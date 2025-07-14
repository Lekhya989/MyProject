import { Component, OnInit } from '@angular/core';
import { BookingDetails } from '../../models/booking-details';
import { ApiService } from '../../shared/services/api.service';
import { MaterialModule } from '../../material/material.module';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'user-bookings',
  imports: [MaterialModule,DatePipe],
  templateUrl: './user-bookings.component.html',
  styleUrl: './user-bookings.component.scss'
})
export class UserBookingsComponent implements OnInit {
  bookings:BookingDetails[]=[];
  displayedColumns: string[] = ['userName', 'taxProfessionalName', 'startTime', 'endTime', 'bookedOn'];

  constructor(private apiService: ApiService){}

  ngOnInit(): void {
      this.apiService.getAllBookings().subscribe({
        next: (res)=> this.bookings=res,
        error:()=>console.error("Could not load bookings")
      });
  }

}
