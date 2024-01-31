import { Injectable } from '@angular/core';
import { Workflow } from '@core/domain-classes/workflow';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { HttpClient, HttpEvent, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { WorkFlowParticipant } from '@core/domain-classes/workflow-participant';
import { WorkFlowResource } from '@core/domain-classes/workflow-resource';

@Injectable({
  providedIn: 'root'
})
export class WorkflowService {

  constructor( private http: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) { }

    addWorkflow(salesOrder: Workflow): Observable<Workflow | CommonError> {
      const url = `WorkFlow`;
      return this.http.post<Workflow>(url, salesOrder)
        .pipe(catchError(this.commonHttpErrorService.handleError));
    }

    getApprovalDocuments(
      resource: DocumentResource
    ): Observable<HttpResponse<DocumentInfo[]> | CommonError> {
      const url = `WorkFlow`;
      const customParams = new HttpParams()
        .set('Fields', resource.fields)
        .set('OrderBy', resource.orderBy)
        .set(
          'createDateString',
          resource.createDate ? resource.createDate.toString() : ''
        )
        .set('PageSize', resource.pageSize.toString())
        .set('Skip', resource.skip.toString())
        .set('SearchQuery', resource.searchQuery)
        .set('categoryId', resource.categoryId)
        .set('name', resource.name)
        .set('metaTags', resource.metaTags)
        .set('id', resource.id.toString());

      return this.http
        .get<DocumentInfo[]>(url, {
          params: customParams,
          observe: 'response',
        })
        .pipe(catchError(this.commonHttpErrorService.handleError));
    }

    getApprovalWorkFlows(
      resource: WorkFlowResource
    ): Observable<HttpResponse<Workflow[]> | CommonError> {
      const url = `WorkFlow`;
      const customParams = new HttpParams()
        .set('Fields', resource.fields)
        .set('OrderBy', resource.orderBy)
        .set(
          'createDateString',
          resource.createDate ? resource.createDate.toString() : ''
        )
        .set('PageSize', resource.pageSize.toString())
        .set('Skip', resource.skip.toString())
        .set('SearchQuery', resource.searchQuery)
        .set('categoryId', resource.categoryId)
        .set('documentName', resource.name)
        .set('metaTags', resource.metaTags)
        .set('id', resource.id.toString());

      return this.http
        .get<Workflow[]>(url, {
          params: customParams,
          observe: 'response',
        })
        .pipe(catchError(this.commonHttpErrorService.handleError));
    }

    getWorkFlowParticipants(workFlowId: string): Observable<WorkFlowParticipant[] | CommonError> {
      const url = `WorkFlow/${workFlowId}/participants`;
      return this.http.get<WorkFlowParticipant[]>(url)
          .pipe(catchError(this.commonHttpErrorService.handleError));
  }
}
