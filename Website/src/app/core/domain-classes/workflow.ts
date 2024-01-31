import { WorkFlowParticipant } from "./workflow-participant";

export class Workflow {
  id: string;
  url?: string;
  documentId: string;
  documentName?:string
  isAllowDownload?:boolean
  workflowMethod: number;
  workFlowType: number;
  approvalStatus: number;
  workFlowParticipant: WorkFlowParticipant[];
}
