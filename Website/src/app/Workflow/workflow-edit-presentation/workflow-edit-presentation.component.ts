import { HttpClient, HttpEventType, HttpRequest } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';

import { Role } from '@core/domain-classes/role';
import { User } from '@core/domain-classes/user';
import { Workflow } from '@core/domain-classes/workflow';
import { CommonService } from '@core/services/common.service';
import { BaseComponent } from 'src/app/base.component';
import { UntypedFormArray, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentService } from 'src/app/document/document.service';
import { WorkflowUserParticipant } from '@core/domain-classes/workflow-user-participant';
import { WorkflowService } from '../workflow.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { TranslationService } from '@core/services/translation.service';
import { WorkFlowParticipant } from '@core/domain-classes/workflow-participant';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'app-workflow-edit-presentation',
  templateUrl: './workflow-edit-presentation.component.html',
  styleUrls: ['./workflow-edit-presentation.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WorkflowEditPresentationComponent
  extends BaseComponent
  implements OnInit
{
  workflow: Workflow;
  workflowForm: UntypedFormGroup;
  participantForm: UntypedFormGroup;
  documentInfos: DocumentInfo[] = [];
  allDocumentInfos: DocumentInfo[] = [];
  @Input() loading: boolean;
  @Output() onSaveWorkflow: EventEmitter<Workflow> =
    new EventEmitter<Workflow>();
  filterUsersMap: { [key: string]: User[] } = {};
  progress = 0;
  message = '';
  users: User[];
  roles: Role[];
  selectedRoles: Role[] = [];
  selectedUsers: User[] = [];
  workFlowParticipant: WorkFlowParticipant[];

  get workflowParticipantArray(): UntypedFormArray {
    return <UntypedFormArray>this.workflowForm.get('workFlowParticipant');
  }
  constructor(
    private route: ActivatedRoute,
    private fb: UntypedFormBuilder,
    private commonService: CommonService,
    private documentService: DocumentService,
    private workflowService: WorkflowService,
    private toastrService: ToastrService,
    public translationService: TranslationService,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.createWorkflowForm();
    this.getDocuments();
    this.getUsers();
    this.getRoles();
  }
  getDocuments() {
    this.documentService.getAllDocumentsForDropDown().subscribe((c) => {
      this.documentInfos = c;
    });
  }
  getUsers() {
    this.sub$.sink = this.commonService
      .getUsers()
      .subscribe((users: User[]) => (this.users = users));
  }

  getRoles() {
    this.sub$.sink = this.commonService
      .getRoles()
      .subscribe((roles: Role[]) => (this.roles = roles));
  }
  createWorkflowForm() {
    this.workflowForm = this.fb.group({
      documentId: ['', [Validators.required]],
      workflowMethod: 1,
      workFlowType: 1,
      approvalStatus: 1,
      workFlowParticipant: this.fb.array([]),
    });
    this.workflowParticipantArray.push(
      this.createWorkflowParticipant(
        this.workflowParticipantArray.length
      )
    );
  }

  onAddUserParticipant() {
    this.workflowParticipantArray.push(
      this.createWorkflowParticipant(
        this.workflowParticipantArray.length
      )
    );
  }

  createWorkflowParticipant(index: number) {
    const formGroup = this.fb.group({
      userId: ['', [Validators.required]],
      approvalStatus:[1],
      orderNumber: [1, [Validators.required, Validators.min(1)]],
    });
    return formGroup;
  }
  onRAddUserParticipant(index: number) {
    this.workflowParticipantArray.removeAt(index);
  }
  onWorkflowSubmit() {
    const workflow = this.buildWorkflow();
    this.workflowService.addWorkflow(workflow).subscribe(
      (c: Workflow) => {
        this.toastrService.success(
          this.translationService.getValue('PURCHASE_ORDER_ADDED_SUCCESSFULLY')
        );
        this.router.navigate(['/Workflow']);
      },
      (err) => {
        //this.isLoading = false;
      }
    );
  }
  buildWorkflow() {
    const workflow: Workflow = {
      id: '',
      documentId: this.workflowForm.get('documentId').value,
      workflowMethod: this.workflowForm.get('workflowMethod').value,
      workFlowType: this.workflowForm.get('workFlowType').value,
      approvalStatus :this.workflowForm.get('approvalStatus').value,
      workFlowParticipant: [],
    };
    const workFlowParticipant = this.workflowForm.get('workFlowParticipant').value;
    if (workFlowParticipant && workFlowParticipant.length > 0) {
      workFlowParticipant.forEach((po) => {
        workflow.workFlowParticipant.push({
          statusCode: po.statusCode,
          messages: po.message,
          id:po.id,
          workflowId: po.workflowId,
          roleId: po.roleId,
          userId: po.userId,
          orderNumber: po.orderNumber,
          approvalStatus : po.approvalStatus,
          aproverName :po.aproverName
        });
      });
    }
    return workflow;
  }
}
