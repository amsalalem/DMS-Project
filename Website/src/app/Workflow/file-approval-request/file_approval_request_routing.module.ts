import { RouterModule, Routes } from "@angular/router";
import { FileApprovalRequestComponent } from "./file-approval-request.component";
import { AuthGuard } from "@core/security/auth.guard";
import { NgModule } from "@angular/core";

const routes: Routes = [
  {
    path: '', component: FileApprovalRequestComponent,
    data: { claimType: 'approval_file_request_view_approval_file_request' },
    canActivate: [AuthGuard]
  }
]
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FileApprovalRequestRoutingModule { }
