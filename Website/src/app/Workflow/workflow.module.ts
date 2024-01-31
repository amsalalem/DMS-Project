import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkflowListComponent } from './workflow-list/workflow-list.component';
import { WorkflowRoutingModule } from './workflow-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { WorkflowEditComponent } from './workflow-edit/workflow-edit.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';

import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { SharedModule } from '@shared/shared.module';
import { WorkflowEditPresentationComponent } from './workflow-edit-presentation/workflow-edit-presentation.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatMenuModule } from '@angular/material/menu';
import { WorkFlowParticipantComponent } from './workflow-list/work-flow-participant/work-flow-participant.component';

@NgModule({
  declarations: [
    WorkflowListComponent,
    WorkflowEditComponent,
    WorkflowEditPresentationComponent,
    WorkFlowParticipantComponent,
  ],
  imports: [
    CommonModule,
    WorkflowRoutingModule,
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
    MatMenuModule,
  ]
})
export class WorkflowModule { }
