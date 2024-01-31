import { CollectionViewer } from '@angular/cdk/collections';
import { DataSource } from '@angular/cdk/table';
import { HttpResponse } from '@angular/common/http';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { Workflow } from '@core/domain-classes/workflow';
import { WorkFlowResource } from '@core/domain-classes/workflow-resource';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { WorkflowService } from 'src/app/Workflow/workflow.service';
export class WorkFlowDataSource implements DataSource<Workflow> {

  private workflowSubject = new BehaviorSubject<Workflow[]>([]);
  private responseHeaderSubject = new BehaviorSubject<ResponseHeader>(null);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();

  public responseHeaderSubject$= this.responseHeaderSubject.asObservable();

  private _data: Workflow[];
  public get data(): Workflow[] {
    return this._data;
  }
  public set data(v: Workflow[]) {
    this._data = v;
  }

  constructor(private workflowService: WorkflowService) { }

  connect(collectionViewer: CollectionViewer): Observable<Workflow[]> {
    return this.workflowSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.workflowSubject.complete();
    this.loadingSubject.complete();
  }

  loadDocuments(workFlowResource: WorkFlowResource) {

    this.loadingSubject.next(true);

    this.workflowService.getApprovalWorkFlows(workFlowResource).pipe(
      catchError(() => of([])),
      finalize(() => this.loadingSubject.next(false))
    )
    .subscribe(
      (resp: HttpResponse<Workflow[]>) => {
        const paginationParam = JSON.parse(
          resp.headers.get('X-Pagination')
        ) as ResponseHeader;
        this.responseHeaderSubject.next(paginationParam);
        this._data = [...resp.body];
        this.workflowSubject.next(this._data);
      }

    );
  }
}
