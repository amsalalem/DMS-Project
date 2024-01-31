import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "@core/security/auth.guard";
import { NgModule } from "@angular/core";
import { MyApprovedFileComponent } from "./my-approved-file.component";

const routes: Routes = [
  {
    path: '', component: MyApprovedFileComponent,
    data: { claimType: 'approval_file_request_view_my_approved_file' },
    canActivate: [AuthGuard]
  }
]
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyApprovedFileRoutingModule { }
