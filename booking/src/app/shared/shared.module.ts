import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material/material.module';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { PageFooterComponent } from './components/page-footer/page-footer.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    PageHeaderComponent,
    PageFooterComponent,
 
  ],
  imports: [CommonModule ,MaterialModule, RouterModule, ReactiveFormsModule],
  exports: [
    CommonModule, 
    PageHeaderComponent, 
    PageFooterComponent, 
    RouterModule,
    ReactiveFormsModule,
    MaterialModule
  ],
})
export class SharedModule { }
