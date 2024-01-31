import { ResourceParameter } from './resource-parameter';

export class WorkFlowResource extends ResourceParameter {
  id?: string = '';
  createdBy?: string = '';
  categoryId?: string = '';
  createDate?: Date;
}
