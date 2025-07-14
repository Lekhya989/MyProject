import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../material/material.module';
import { ApiService } from '../../shared/services/api.service';
import { TaxProfessional } from '../../models/taxprofessional';
import { Slot } from '../../models/slot';

@Component({
  selector: 'slot-selection',
  standalone: true,
  imports: [CommonModule, MaterialModule, ReactiveFormsModule],
  templateUrl: './slot-selection.component.html',
  styleUrls: ['./slot-selection.component.scss']
})
export class SlotSelectionComponent implements OnInit {
  slotForm!: FormGroup;
  taxPros: TaxProfessional[] = [];
  availableSlots: Slot[] = [];
  selectedTaxPro: TaxProfessional | null = null; 

  constructor(private fb: FormBuilder, private api: ApiService) {}

  ngOnInit(): void {
  this.slotForm = this.fb.group({
    taxProfessionalId: ['']
  });

  this.api.getTaxProfessionals().subscribe({
    next: (res) => {
      this.taxPros = res.map((pro: any) => ({
        ...pro,
        SMB_certified: pro.SMB_certified === true || pro.SMB_certified === 'true' || pro.SMB_certified === 1 || pro.SMB_certified === '1',
        speaks_spanish: pro.speaks_spanish === true || pro.speaks_spanish === 'true' || pro.speaks_spanish === 1 || pro.speaks_spanish === '1'
      }));
    },
    error: () => console.error('Failed to load professionals')
  });
}



onTaxProSelect(): void {
  const id = this.slotForm.value.taxProfessionalId;
  if (id) {
    this.selectedTaxPro = this.taxPros.find(p => p.id === id) || null;

    if (this.selectedTaxPro) {
      const cert = this.selectedTaxPro['SMB_certified'] ?? this.selectedTaxPro['SMB_certified'];
      this.selectedTaxPro.SMB_certified = this.normalizeBoolean(cert).toString();

      const spanish = this.selectedTaxPro['speaks_spanish'] ?? this.selectedTaxPro['speaks_spanish'];
      this.selectedTaxPro.speaks_spanish = this.normalizeBoolean(spanish).toString();
    }

    this.api.getSlotsByTaxProfessional(id).subscribe({
      next: (slots) => {
        this.availableSlots = slots.filter(s => !s.isBooked);
      },
      error: () => console.error('Failed to fetch slots')
    });
  } else {
    this.selectedTaxPro = null;
    this.availableSlots = [];
  }
}

normalizeBoolean(value: any): boolean {
  return value === true || value === 'true' || value === 1 || value === '1';
}

  bookSlot(slotId: number) {
    const payload = { slotId };
    console.log('Booking payload:', payload);
    this.api.bookSlot(payload).subscribe({
      next: () => {
        alert('Booking request submitted! Awaiting admin approval.');
        this.onTaxProSelect(); 
      },
      error: () => alert('Failed to book slot.')
    });
  }
}
