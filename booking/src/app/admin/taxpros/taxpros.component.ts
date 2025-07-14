import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../../shared/services/api.service';
import { MaterialModule } from '../../material/material.module';
import { CommonModule } from '@angular/common';
import { Slot } from '../../models/slot';

@Component({
  selector: 'taxpros',
  standalone: true,
  imports: [ReactiveFormsModule,MaterialModule,CommonModule],
  templateUrl: './taxpros.component.html',
  styleUrl: './taxpros.component.scss',
})
export class TaxprosComponent implements OnInit{

  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'phoneNumber', 'speaks_spanish','SMB_certified', 'actions'];

  taxProForm!: FormGroup;
  taxPros:any = [];
  showForm = false;
  isEditMode = false;
  editId: number | null = null;
  selectedTaxProSlots: Slot[] = [];
  selectedTaxProName: string = '';


  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private snackBar: MatSnackBar
  ){}

  ngOnInit(): void {
    this.initializeForm();
    this.fetchTaxProfessionals();
  }

   initializeForm() {
    this.taxProForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      speaks_spanish: [false, Validators.required],
      SMB_certified:[false,Validators.required]
    });
  }

  fetchTaxProfessionals() {
    this.apiService.getTaxProfessionals().subscribe({
      next: (data) => {
        this.taxPros = data;
         console.log('Fetched tax professionals:', this.taxPros);
      },
      error: () => {
        this.snackBar.open('Failed to load tax professionals', 'Close', { duration: 3000 });
      }
    });
  }

  submitTaxPro() {
    if (this.taxProForm.invalid) {
      this.snackBar.open('Please fill all fields correctly.', 'Close', { duration: 3000 });
      return;
    }

    const formValue = this.taxProForm.value;
    console.log('Form Value:', formValue);

    if (this.isEditMode && this.editId !== null) {
      this.apiService.updateTaxProfessional(this.editId, formValue).subscribe({
        next: () => {
          this.snackBar.open('Updated successfully', 'Close', { duration: 3000 });
          this.resetForm();
          this.fetchTaxProfessionals();
        },
        error: () => {
          this.snackBar.open('Update failed', 'Close', { duration: 3000 });
        }
      });
    } else {
      this.apiService.createTaxProfessional(formValue).subscribe({
        next: () => {
          this.snackBar.open('Added successfully', 'Close', { duration: 3000 });
          this.resetForm();
          this.fetchTaxProfessionals();
        },
        error: () => {
          this.snackBar.open('Add failed', 'Close', { duration: 3000 });
        }
      });
    }
  }

  editTaxPro(taxPro: any) {
    this.taxProForm.patchValue(taxPro);
    this.isEditMode = true;
    this.editId = taxPro.id ?? null;
    this.showForm = true;
  }

  deleteTaxPro(id: number) {
    if (confirm('Are you sure you want to delete this tax professional?')) {
      this.apiService.deleteTaxProfessional(id).subscribe({
        next: () => {
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
          this.fetchTaxProfessionals();
        },
        error: (err) => {
          console.error('Delete error:', err);
          this.snackBar.open('Delete failed', 'Close', { duration: 3000 });
        }
      });
    }
  }

  resetForm() {
  this.taxProForm.reset({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    speaks_spanish: false,
    SMB_certified: false 
  });
  this.isEditMode = false;
  this.editId = null;
  this.showForm = false;
}

  trackById(index: number, item: any) {
    return item.id;
  }


}

