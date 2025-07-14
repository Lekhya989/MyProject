import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TaxProfessional } from '../../models/taxprofessional';
import { Slot } from '../../models/slot';
import { UserBookingsComponent } from '../../admin/user-bookings/user-bookings.component';
import { BookingDetails } from '../../models/booking-details';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrl: string = '/api/User';
  constructor(private http: HttpClient) {}

  registerUser(user: any): Observable<any>{
    console.log("user..");
    return this.http.post(`${this.baseUrl}/Create`,user,{responseType:'text'});
  }

  loginUser(credentials:{email:string,password:string}){
    return this.http.post(`${this.baseUrl}/Login`,credentials);
  }

  getTaxProfessionals() {
  return this.http.get<TaxProfessional[]>('/api/TaxProfessional/getAll');
}
 
createTaxProfessional(taxPro: TaxProfessional) {
  return this.http.post('/api/TaxProfessional/create', taxPro, {responseType:'text'});
}

generateSlots(slotData: { taxProfessionalId: number; startTime: string; endTime: string}) {
  return this.http.post('/api/Slot/generate', slotData,{responseType:'text'});
}
 
updateTaxProfessional(id: number, taxPro: TaxProfessional) {
  return this.http.put(`/api/TaxProfessional/update/${id}`, taxPro,{responseType:'text'});
}
 
deleteTaxProfessional(id: number) {
  return this.http.delete(`/api/TaxProfessional/delete/${id}`,{responseType:'text'});
}

getSlotsByTaxProfessional(id: number): Observable<Slot[]> {
    return this.http.get<Slot[]>(`/api/Slot/byTaxPro/${id}`);
  }

updateSlot(slotId: number, slot: any) {
  return this.http.put(`/api/Slot/update/${slotId}`, slot,{responseType:'text'});
}

deleteSlot(slotId: number) {
  return this.http.delete(`/api/Slot/delete/${slotId}`,{responseType:'text'});
}

getUserBookings(): Observable<any[]> {
  const token = localStorage.getItem('token');
  const headers = new HttpHeaders({
    Authorization: `Bearer ${token}`
  });

  return this.http.get<any[]>(`/api/Booking/my`, { headers });
}

bookSlot(payload: { slotId: number }) {
  const token = localStorage.getItem("token");
  const headers = {
    Authorization: `Bearer ${token}`
  };

  return this.http.post('/api/Booking', payload, { headers });
}

deleteBooking(bookingId:number){
  const token = localStorage.getItem("token");
  const headers={
    Authorization: `Bearer ${token}`
  };
  return this.http.delete(`/api/Booking/${bookingId}`, { headers })
}

getAllBookings(): Observable<BookingDetails[]> {
  const token = localStorage.getItem('token');
  const headers = new HttpHeaders({
    Authorization: `Bearer ${token}`
  });

  return this.http.get<BookingDetails[]>(`/api/Booking/all`, { headers });
}

approveBooking(bookingId: number) {
  const token = localStorage.getItem('token');
  const headers = new HttpHeaders({
    Authorization: `Bearer ${token}`
  });

  return this.http.post(`/api/Booking/approve/${bookingId}`, {}, { headers });
}

getUpcomingBookings() {
  const token = localStorage.getItem("token");
  const headers = {
    Authorization: `Bearer ${token}`
  };
  return this.http.get<any[]>('/api/Booking/upcoming', { headers });
}

}



