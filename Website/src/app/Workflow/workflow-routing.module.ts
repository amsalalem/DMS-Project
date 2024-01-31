import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@core/security/auth.guard';
import {WorkflowListComponent} from './workflow-list/workflow-list.component';
import { WorkflowEditComponent } from './workflow-edit/workflow-edit.component';

const routes: Routes = [
  {
    path: '', component: WorkflowListComponent,
    data: { claimType: 'Workflow_view_Workflow' },
    canActivate: [AuthGuard]
  },
  {
    path: 'add',
    component: WorkflowEditComponent,
    data: { claimType: 'workflow_create_workflow' },
    canActivate: [AuthGuard]
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkflowRoutingModule { }
