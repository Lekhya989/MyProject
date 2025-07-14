import { Component } from '@angular/core';
import { ApiService } from '../../shared/services/api.service';
import { BookingDetails } from '../../models/booking-details';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { Route } from '@angular/router';

@Component({
  selector: 'booking-confirmations',
  imports: [SharedModule],
  templateUrl: './booking-confirmations.component.html',
  styleUrl: './booking-confirmations.component.scss'
})
export class BookingConfirmationsComponent {

  bookings: BookingDetails[] = [];

  constructor(private api: ApiService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.api.getAllBookings().subscribe({
      next: (res) => this.bookings = res,
      error: () => this.snackBar.open('Failed to load bookings', 'Close', { duration: 3000 })
    });
  }

  approveBooking(id: number) {
  const booking = this.bookings.find(b => b.id === id);
  if (booking) booking.isApproved = true;

  this.api.approveBooking(id).subscribe({
    next: () => {
      alert('Booking approved successfully!');
      this.getBookings();
    },
    error: () => {
      alert('Already approved or approval failed.');
      this.getBookings();
    }
  });
}


getBookings() {
  this.api.getAllBookings().subscribe({
    next: (res) => {
      this.bookings = res;
    },
    error: () => {
      console.error('Failed to load bookings');
    }
  });
}


}
