import { ApprovalStatus } from "src/app/Workflow/workflow-list/work-flow-participant/approval-status.enum";
import { User } from "./user";

export class WorkFlowParticipant {
  statusCode: number;
  messages: string[];
  id?: string;
  workflowId?: string;
  roleId?: string;
  userId: string;
  aproverName: string;
  orderNumber: number;
  approvalStatus: ApprovalStatus.Pending;
}
