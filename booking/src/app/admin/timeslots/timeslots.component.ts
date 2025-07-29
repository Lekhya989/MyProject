import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../../shared/services/api.service';
import { Slot } from '../../models/slot';
import { TaxProfessional } from '../../models/taxprofessional';
import { MaterialModule } from '../../material/material.module';
import { CommonModule } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  standalone: true,
  selector: 'timeslots',
  imports: [MaterialModule, CommonModule, ReactiveFormsModule, MatDatepickerModule, MatNativeDateModule],
  templateUrl: './timeslots.component.html',
  styleUrls: ['./timeslots.component.scss']
})
export class TimeslotsComponent implements OnInit {
  slotForm!: FormGroup;
  slots: Slot[] = [];
  taxPros: TaxProfessional[] = [];
  isEditMode: boolean = false;
  editSlotId: number | null = null;
  selectedProName: string = '';
  timeSlots: string[] = [];

  displayColumns: string[] = ['date', 'startTime', 'endTime', 'actions'];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.getTaxProfessionals();
    this.generateTimeSlots();
  }

  generateTimeSlots() {
  const start = 8;
  const end = 20;
  this.timeSlots = [];

  for (let h = start; h < end; h++) {
    const slot = h.toString().padStart(2, '0') + ':00';

    
    const tempDate = new Date();
    tempDate.setHours(h, 0, 0, 0);
    const formattedLabel = tempDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

    this.timeSlots.push(slot); 
  }
}


  initializeForm() {
    this.slotForm = this.fb.group({
      taxProfessionalId: ['', Validators.required],
      date: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
    });
  }

  getTaxProfessionals() {
    this.apiService.getTaxProfessionals().subscribe({
      next: (res: TaxProfessional[]) => {
        this.taxPros = res;
      },
      error: () => {
        this.snackBar.open('Failed to fetch tax professionals.', 'Close', {
          duration: 3000,
        });
      }
    });
  }

  onTaxProfessionalChange(taxProId: number | string) {
    const id = Number(taxProId);
    const pro = this.taxPros.find(p => p.id === id);
    this.selectedProName = pro ? `${pro.firstName} ${pro.lastName}` : '';
    if (id) {
      this.apiService.getSlotsByTaxProfessional(id).subscribe({
        next: (res: Slot[]) => {
          this.slots = res;
        },
        error: () => {
          this.snackBar.open('Failed to fetch slots.', 'Close', {
            duration: 3000,
          });
        }
      });
    }
  }

  submitSlot() {
  if (this.slotForm.invalid) return;

  const taxProfessionalId = this.slotForm.get('taxProfessionalId')?.value;
  const date = this.slotForm.get('date')?.value;
  const startTime = this.slotForm.get('startTime')?.value;
  const endTime = this.slotForm.get('endTime')?.value;

  if (!startTime || !endTime || !date) {
    this.snackBar.open('Please fill in all fields.', 'Close', { duration: 3000 });
    return;
  }

  const [sh] = startTime.split(':');
  const [eh] = endTime.split(':');

  const startHour = parseInt(sh, 10);
  const endHour = parseInt(eh, 10);

  if (startHour >= endHour) {
    this.snackBar.open('End time must be after start time.', 'Close', { duration: 3000 });
    return;
  }

  const slotRequests = [];

  for (let h = startHour; h < endHour; h++) {
    const startSlot = new Date(date);
    const endSlot = new Date(date);
    startSlot.setHours(h, 0, 0, 0);
    endSlot.setHours(h + 1, 0, 0, 0);

    // Format to ISO string in local time (YYYY-MM-DDTHH:mm:ss)
    const formatToLocalISOString = (d: Date) => {
      const pad = (n: number) => n.toString().padStart(2, '0');
      return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}:${pad(d.getSeconds())}`;
    };

    slotRequests.push({
      taxProfessionalId,
      startTime: formatToLocalISOString(startSlot),
      endTime: formatToLocalISOString(endSlot)
    });
  }

  let createdCount = 0;

  slotRequests.forEach((slot, index) => {
    this.apiService.generateSlots(slot).subscribe({
      next: () => {
        createdCount++;
        if (createdCount === slotRequests.length) {
          this.snackBar.open('Slots generated successfully!', 'Close', { duration: 3000 });
          this.resetSlotForm();
          this.onTaxProfessionalChange(taxProfessionalId);
        }
      },
      error: () => {
        this.snackBar.open(`Failed to generate slot starting at ${slot.startTime}`, 'Close', { duration: 3000 });
      }
    });
  });
}



  editSlot(slot: Slot) {
    this.isEditMode = true;
    this.editSlotId = slot.id;
    const date = new Date(slot.startTime);
    const formatTime = (d: Date) => `${d.getHours().toString().padStart(2, '0')}:${d.getMinutes().toString().padStart(2, '0')}`;

    this.slotForm.patchValue({
      taxProfessionalId: slot.taxProfessionalId,
      date: date,
      startTime: formatTime(new Date(slot.startTime)),
      endTime: formatTime(new Date(slot.endTime))
    });
  }

  deleteSlot(id: number) {
    this.apiService.deleteSlot(id).subscribe({
      next: () => {
        this.snackBar.open('Slot deleted successfully!', 'Close', { duration: 3000 });
        const taxProId = this.slotForm.get('taxProfessionalId')?.value;
        if (taxProId) {
          this.onTaxProfessionalChange(taxProId);
        }
      },
      error: () => {
        this.snackBar.open('Failed to delete slot.', 'Close', { duration: 3000 });
      }
    });
  }

  resetSlotForm() {
    const selectedTaxPro = this.slotForm.get('taxProfessionalId')?.value;
    this.slotForm.reset({ taxProfessionalId: selectedTaxPro });
    this.isEditMode = false;
    this.editSlotId = null;
  }

  trackById(index: number, item: any) {
    return item.id;
  }
}
