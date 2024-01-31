import { HttpClient, HttpEvent, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { WorkFlowHistory } from '@core/domain-classes/work-flow-history';
import { Workflow } from '@core/domain-classes/workflow';
import { WorkFlowResource } from '@core/domain-classes/workflow-resource';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FileApprovalRequestService {

  constructor(private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) { }

  getFileApprovalRequest(resource: WorkFlowResource): Observable<HttpResponse<Workflow[]> | CommonError> {
    const url = `FileNeedMyApproval`;
    const customParams = new HttpParams()
      .set('Fields', resource.fields)
      .set('OrderBy', resource.orderBy)
      .set('PageSize', resource.pageSize.toString())
      .set('Skip', resource.skip.toString())
      .set('SearchQuery', resource.searchQuery)
      .set('categoryId', resource.categoryId)
      .set('name', resource.name)
      .set('metaTags', resource.metaTags)
      .set('id', resource.id.toString())

    return this.httpClient.get<Workflow[]>(url, {
      params: customParams,
      observe: 'response'
    }).pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getDocumentLibrary(id: string): Observable<Workflow> {
    return this.httpClient.get<Workflow>('document/' + id);
  }
  getDocumentViewLibrary(id: string): Observable<DocumentInfo> {
    return this.httpClient.get<Workflow>('document/view/' + id);
  }

  createFileNeedMyApproval(salesOrder: WorkFlowHistory): Observable<WorkFlowHistory | CommonError> {
    const url = `FileNeedMyApproval`;
    return this.httpClient.post<WorkFlowHistory>(url, salesOrder)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }


}
