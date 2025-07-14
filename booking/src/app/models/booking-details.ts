export interface BookingDetails {
  id: number;
  userId: number;
  slotId: number;
  userName: string; 
  bookedOn: string;
  startTime?: string;
  endTime: string;
  taxProfessionalName: string;
  isApproved?: boolean; 
}
