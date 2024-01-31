import { Role } from "./role";

export class WorkFlowRoleParticipant {
    id?: string;
    documentId: string;
    roleId: string;
    startDate: Date;
    endDate: Date;
    role?: Role
}
