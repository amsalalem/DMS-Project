import { Component, Inject, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { WorkFlowHistory } from '@core/domain-classes/work-flow-history';
import { FileApprovalRequestService } from '../file-approval-request.service';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { TranslationService } from '@core/services/translation.service';
import { BaseComponent } from 'src/app/base.component';
import { ApprovalStatus } from '../../workflow-list/work-flow-participant/approval-status.enum';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { CommonService } from '@core/services/common.service';

@Component({
  selector: 'app-file-approval-request-presentation',
  templateUrl: './file-approval-request-presentation.component.html',
  styleUrls: ['./file-approval-request-presentation.component.css']
})
export class FileApprovalRequestPresentationComponent extends BaseComponent implements OnInit {
  workFlowHistoryForm: UntypedFormGroup;
  workFlowHistory: WorkFlowHistory[] = [];
  approvalStatuses = Object.values(ApprovalStatus);
  constructor(  private fb: UntypedFormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fileApprovalRequestService: FileApprovalRequestService,
    private dialogRef: MatDialogRef<FileApprovalRequestPresentationComponent>,
    private commonDialogService: CommonDialogService,
    private translationService: TranslationService,
    private toastrService: ToastrService,
    private commonService: CommonService,
    private router: Router) {
    super(); }

  ngOnInit(): void {
    this.createForm();
    //this.getDocumentComment();
  }
  closeDialog() {
    this.dialogRef.close();
  }

  createForm() {
    this.workFlowHistoryForm = this.fb.group({
      approvalStatus: ['', [Validators.required]],
      comment : ['',[Validators.required]]
    });
  }

  addFileAproval() {
    const documentComment: WorkFlowHistory = {
      workflowId: this.data.id,
      approvalStatus: 2,
      comment: this.workFlowHistoryForm.get('comment').value,
      email:''
    };
    this.sub$.sink = this.fileApprovalRequestService.createFileNeedMyApproval(documentComment)
      .subscribe((c: WorkFlowHistory) => {
        this.workFlowHistoryForm.markAsUntouched();
        this.toastrService.success(
          this.translationService.getValue('FILE_APPROVED_SUCCESSFULLY')
        );
        this.dialogRef.close("loaded");
        this.addDocumentTrail();
        this.router.navigate(['/FileNeedMyApproval']);
      });
  }
  addFileRejection() {
    const documentComment: WorkFlowHistory = {
      workflowId: this.data.id,
      approvalStatus: 3,
      comment: this.workFlowHistoryForm.get('comment').value,
      email:''
    };
    this.sub$.sink = this.fileApprovalRequestService.createFileNeedMyApproval(documentComment)
      .subscribe((c: WorkFlowHistory) => {
        this.workFlowHistoryForm.markAsUntouched();
        this.toastrService.success(
          this.translationService.getValue('FILE_REJECTED_SUCCESSFULLY')
        );
        this.dialogRef.close("loaded");
        this.rejectDocumentTrail();
        this.router.navigate(['/FileNeedMyApproval']);
      });
  }
  addDocumentTrail() {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: this.data.document.documentId,
      operationName: DocumentOperation.Workflow_Approved.toString()
    }
    this.sub$.sink = this.commonService.addDocumentAuditTrail(objDocumentAuditTrail)
      .subscribe(c => {
      })
  }
  rejectDocumentTrail() {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: this.data.document.documentId,
      operationName: DocumentOperation.Workflow_Rejected.toString()
    }
    this.sub$.sink = this.commonService.addDocumentAuditTrail(objDocumentAuditTrail)
      .subscribe(c => {
      })
  }
}
