import { CollectionViewer } from '@angular/cdk/collections';
import { DataSource } from '@angular/cdk/table';
import { HttpResponse } from '@angular/common/http';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { Workflow } from '@core/domain-classes/workflow';
import { WorkFlowResource } from '@core/domain-classes/workflow-resource';
import { FileApprovalRequestService } from './file-approval-request.service';

export class FileApprovalDataSource implements DataSource<Workflow> {

  private documentsSubject = new BehaviorSubject<Workflow[]>([]);
  private responseHeaderSubject = new BehaviorSubject<ResponseHeader>(null);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  private _data: Workflow[];
  public get data(): Workflow[] {
    return this._data;
  }
  public set data(v: Workflow[]) {
    this._data = v;
  }

  public loading$ = this.loadingSubject.asObservable();

  public responseHeaderSubject$ = this.responseHeaderSubject.asObservable();

  constructor(private documentService: FileApprovalRequestService) { }

  connect(collectionViewer: CollectionViewer): Observable<Workflow[]> {
    return this.documentsSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.documentsSubject.complete();
    this.loadingSubject.complete();
  }

  loadDocuments(documentResource: WorkFlowResource) {

    this.loadingSubject.next(true);

    this.documentService.getFileApprovalRequest(documentResource).pipe(
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
          this.documentsSubject.next(this._data);
        }

      );
  }
}
