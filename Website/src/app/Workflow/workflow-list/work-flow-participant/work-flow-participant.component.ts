import { Component, Inject, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Workflow } from '@core/domain-classes/workflow';
import { WorkFlowParticipant } from '@core/domain-classes/workflow-participant';
import { BaseComponent } from 'src/app/base.component';
import { WorkflowService } from '../../workflow.service';
import { TranslationService } from '@core/services/translation.service';
import { ActivatedRoute } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { ApprovalStatus } from './approval-status.enum';

@Component({
  selector: 'app-work-flow-participant',
  templateUrl: './work-flow-participant.component.html',
  styleUrls: ['./work-flow-participant.component.css']
})
export class WorkFlowParticipantComponent extends BaseComponent implements OnInit {
  @Input() workflow: Workflow;
  workFlowParticipant: WorkFlowParticipant[] = [];
  isLoading = false;
  displayedColumns: string[] = ['approver','orderNumber', 'approvalStatus'];
  ParticipantDataSource: MatTableDataSource<WorkFlowParticipant>;
  @ViewChild('workFlowParticipantPaginator') workFlowParticipantPaginator: MatPaginator;
   constructor(private workflowService: WorkflowService,
    public translationService:TranslationService,
    private route: ActivatedRoute,
    @Inject(MAT_DIALOG_DATA) public data: Workflow,
    private dialogRef: MatDialogRef<WorkFlowParticipantComponent>) {
    super();
    this.workflow = data;
  }

  ngOnInit(): void {
    this.sub$.sink = this.route.params.subscribe(params => {
      this.getWorkFlowParticipants();
    });
  }

  getWorkFlowParticipants() {
    this.sub$.sink = this.workflowService.getWorkFlowParticipants(this.workflow.id)
      .subscribe((data: WorkFlowParticipant[]) => {
        this.workFlowParticipant = data;
        this.ParticipantDataSource = new MatTableDataSource(this.workFlowParticipant);
        this.ParticipantDataSource.paginator = this.workFlowParticipantPaginator;
      });
  }

  closeDialog() {
    this.dialogRef.close();
  }
  getApprovalStatusText(status: ApprovalStatus): string {
    switch (status) {
      case ApprovalStatus.New:
        return 'New';
      case ApprovalStatus.Pending:
        return 'Pending';
      case ApprovalStatus.Approved:
        return 'Approved';
      case ApprovalStatus.Rejected:
        return 'Rejected';
      default:
        return 'Unknown';
    }
  }
}
