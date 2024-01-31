import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';

import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { SharedModule } from '@shared/shared.module';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatMenuModule } from '@angular/material/menu';
import { MyApprovedFileComponent } from './my-approved-file.component';
import { MyApprovedFileRoutingModule } from './my-approved-file-routing.module';

@NgModule({
  declarations: [
    MyApprovedFileComponent
  ],
  imports: [
    CommonModule,
    MyApprovedFileRoutingModule,
    ReactiveFormsModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatSlideToggleModule,
    SharedModule,
    MatDatepickerModule,
    MatSortModule,
    MatPaginatorModule,
    MatMenuModule
  ]
})
export class MyApprovedFileModule { }
