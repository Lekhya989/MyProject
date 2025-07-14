import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../../shared/services/api.service';
import { Slot } from '../../models/slot';
import { TaxProfessional } from '../../models/taxprofessional';
import { MaterialModule } from '../../material/material.module';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'timeslots',
  imports: [MaterialModule, CommonModule, ReactiveFormsModule],
  templateUrl: './timeslots.component.html',
  styleUrls: ['./timeslots.component.scss']
})
export class TimeslotsComponent implements OnInit {
  slotForm!: FormGroup;
  slots: Slot[] = [];
  taxPros: TaxProfessional[] = [];
  isEditMode: boolean = false;
  editSlotId: number | null = null;
  selectedProName: string='';

  displayColumns:string[]=['date', 'startTime', 'endTime', 'actions']

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.getTaxProfessionals();
  }

  initializeForm() {
    this.slotForm = this.fb.group({
  taxProfessionalId: ['', Validators.required],
  startTime: ['', Validators.required],
  endTime: ['', Validators.required],
});

  }

  getTaxProfessionals() {
    this.apiService.getTaxProfessionals().subscribe({
      next:(res: TaxProfessional[]) => {
        this.taxPros = res;
      },
      error:() => {
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
  console.log('Selected Professional:', this.selectedProName);
    if (id) {
      this.apiService.getSlotsByTaxProfessional(id).subscribe({
        next:(res: Slot[]) => {
          this.slots = res;
          console.log('Fetched slots for TaxPro ID:', id, res);
        },
        error:() => {
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
  const startTime = this.slotForm.get('startTime')?.value;
  const endTime = this.slotForm.get('endTime')?.value;

  if (!startTime || !endTime) {
    this.snackBar.open('Please fill in all fields.', 'Close', { duration: 3000 });
    return;
  }


  const payload = {
    taxProfessionalId,
    startTime,
    endTime
  };

  if (this.isEditMode && this.editSlotId !== null) {
    this.apiService.updateSlot(this.editSlotId, payload).subscribe({
      next: () => {
        this.snackBar.open('Slot updated successfully!', 'Close', { duration: 3000 });
        this.resetSlotForm();
        this.onTaxProfessionalChange(taxProfessionalId);
      },
      error: () => {
        this.snackBar.open('Failed to update slot.', 'Close', { duration: 3000 });
      }
    });
  } else {
    this.apiService.generateSlots(payload).subscribe({
      next: () => {
        this.snackBar.open('Slot(s) generated successfully!', 'Close', { duration: 3000 });
        this.resetSlotForm();
        this.onTaxProfessionalChange(taxProfessionalId);
      },
      error: () => {
        this.snackBar.open('Failed to generate slots.', 'Close', { duration: 3000 });
      }
    });
  }
}
  editSlot(slot: Slot) {
  this.isEditMode = true;
  this.editSlotId = slot.id;

  const formatDateTimeLocal = (dateStr: string) => {
    const date = new Date(dateStr);
    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
  };

  this.slotForm.patchValue({
    taxProfessionalId: slot.taxProfessionalId,
    startTime: formatDateTimeLocal(slot.startTime), // ✅ formatted properly
    endTime: formatDateTimeLocal(slot.endTime)
  });
}


  deleteSlot(id: number) {
    this.apiService.deleteSlot(id).subscribe({
      next:() => {
        this.snackBar.open('Slot deleted successfully!', 'Close', { duration: 3000 });
        const taxProId = this.slotForm.get('taxProfessionalId')?.value;
        if (taxProId) {
          this.onTaxProfessionalChange(taxProId);
        }
      },
      error:() => {
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
