import { ApprovalStatus } from "src/app/Workflow/workflow-list/work-flow-participant/approval-status.enum";
export class WorkFlowHistory {
  id?: string;
  workflowId?: string;
  approvalStatus: ApprovalStatus;
  comment: string;
  email :string;
}
