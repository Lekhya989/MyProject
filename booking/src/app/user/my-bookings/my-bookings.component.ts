import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../material/material.module';
import { ApiService } from '../../shared/services/api.service';
import { BookingDetails } from '../../models/booking-details';
 
@Component({
  selector: 'my-bookings',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './my-bookings.component.html',
  styleUrls: ['./my-bookings.component.scss']
})
export class MyBookingsComponent implements OnInit {
  bookings: BookingDetails[] = [];
 
  constructor(private apiService: ApiService) {}
 
  ngOnInit(): void {
    this.apiService.getUserBookings().subscribe({
      next: (res) => {
        console.log(" Bookings received:", res);
        this.bookings = res;
      },
      error: () => {
        console.error('Failed to fetch bookings');
      }
    });
  }

  cancelBooking(bookingId:number):void{
    if(confirm('Are you sure you want to cancel this booking?')){
      this.apiService.deleteBooking(bookingId).subscribe({
        next:()=>{
          this.bookings=this.bookings.filter(b=> b.id!==bookingId);
        },
        error:()=>console.error('Failed to delete booking')

      });
    }
  }
}