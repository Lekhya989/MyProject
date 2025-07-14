import { Component } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { UpcomingBookingsComponent } from '../shared/upcoming-bookings/upcoming-bookings.component';

@Component({
  selector: 'user',
  imports: [SharedModule,UpcomingBookingsComponent],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent {

}
