import { HttpEventType, HttpResponse } from '@angular/common/http';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/base.component';
import { FileApprovalDataSource } from './file-approval-datasource';
import { Workflow } from '@core/domain-classes/workflow';
import { WorkFlowResource } from '@core/domain-classes/workflow-resource';
import { Category } from '@core/domain-classes/category';
import { Observable, fromEvent, merge } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { SelectionModel } from '@angular/cdk/collections';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { ClonerService } from '@core/services/clone.service';
import { FileApprovalRequestService } from './file-approval-request.service';
import { WorkflowService } from '../workflow.service';
import { CategoryService } from '@core/services/category.service';
import { MatDialog } from '@angular/material/dialog';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { FileApprovalRequestPresentationComponent } from './file-approval-request-presentation/file-approval-request-presentation.component';
import { ApprovalStatus } from '../workflow-list/work-flow-participant/approval-status.enum';
import { DocumentView } from '@core/domain-classes/document-view';
import { BasePreviewComponent } from '@shared/base-preview/base-preview.component';
import { DocumentInfo } from '@core/domain-classes/document-info';

@Component({
  selector: 'app-file-approval-request',
  templateUrl: './file-approval-request.component.html',
  styleUrls: ['./file-approval-request.component.css']
})
export class FileApprovalRequestComponent extends BaseComponent implements OnInit, AfterViewInit{
  //dataSource: ApprovalDocumentDataSource;
  dataSource: FileApprovalDataSource
  documents: Workflow[] = [];
  displayedColumns: string[] = ['select', 'action', 'documentName', 'categoryName', 'createdDate', 'createdBy','approvalStatus'];
  isLoadingResults = true;
  //documentResource: DocumentResource;
  documentResource: WorkFlowResource;
  categories: Category[] = [];
  allCategories: Category[] = [];
  loading$: Observable<boolean>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;
  @ViewChild('metatag') metatag: ElementRef;
  selection = new SelectionModel<Workflow>(true, []);
  constructor(
    private fileApprovalRequestService: FileApprovalRequestService,
    public overlay: OverlayPanel,
    public clonerService: ClonerService,
    private workflowService: WorkflowService,
    private categoryService: CategoryService,
    private dialog: MatDialog) {
    super();
    this.documentResource = new WorkFlowResource();
    this.documentResource.pageSize = 10;
    this.documentResource.orderBy = "CreatedDate desc";
   }

  ngOnInit(): void {
    this.dataSource = new FileApprovalDataSource(this.fileApprovalRequestService);
    this.dataSource.loadDocuments(this.documentResource);
    this.getCategories();
    this.getResourceParameter();
  }
  ngAfterViewInit() {

    this.sub$.sink = this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    this.sub$.sink = merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => {
          this.documentResource.skip = this.paginator.pageIndex * this.paginator.pageSize;
          this.documentResource.pageSize = this.paginator.pageSize;
          this.documentResource.orderBy = this.sort.active + ' ' + this.sort.direction;
          this.dataSource.loadDocuments(this.documentResource);
        })
      )
      .subscribe();

    this.sub$.sink = fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.documentResource.skip = 0;
          this.documentResource.name = this.input.nativeElement.value;
          this.dataSource.loadDocuments(this.documentResource);
        })
      ).subscribe();

      this.sub$.sink = fromEvent(this.metatag.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.documentResource.skip = 0;
          this.documentResource.metaTags = this.metatag.nativeElement.value;
          this.dataSource.loadDocuments(this.documentResource);
        })
      ).subscribe();
  }
  onCategoryChange(filtervalue: any) {
    if (filtervalue.value) {
      this.documentResource.categoryId = filtervalue.value;
    } else {
      this.documentResource.categoryId = '';
    }
    this.paginator.pageIndex = 0;
    this.documentResource.skip = 0;
    this.dataSource.loadDocuments(this.documentResource);
  }

  getCategories(): void {
    this.categoryService.getAllCategoriesForDropDown().subscribe(c => {
      this.categories = c;
      this.setDeafLevel();
    });
  }

  setDeafLevel(parent?: Category, parentId?: string) {
    const children = this.categories.filter(c => c.parentId == parentId);
    if (children.length > 0) {
      children.map((c, index) => {
        c.deafLevel = parent ? parent.deafLevel + 1 : 0;
        c.index = (parent ? parent.index : 0) + index * Math.pow(0.1, c.deafLevel);
        this.allCategories.push(c);
        this.setDeafLevel(c, c.id);
      });
    }
    return parent;
  }
  getResourceParameter() {
    this.sub$.sink = this.dataSource.responseHeaderSubject$
      .subscribe((c: ResponseHeader) => {
        if (c) {
          this.documentResource.pageSize = c.pageSize;
          this.documentResource.skip = c.skip;
          this.documentResource.totalCount = c.totalCount;
        }
      });
  }
  getFileApprovalRequest(): void {
    this.isLoadingResults = true;
    this.sub$.sink = this.fileApprovalRequestService.getFileApprovalRequest(this.documentResource)
      .subscribe(
        (resp: HttpResponse<Workflow[]>) => {
          const paginationParam = JSON.parse(
            resp.headers.get('X-Pagination')
          ) as ResponseHeader;
          this.documentResource.pageSize = paginationParam.pageSize;
          this.documentResource.skip = paginationParam.skip;
          this.documents = [...resp.body];
          this.isLoadingResults = false;
        },
        () => (this.isLoadingResults = false)
      );
  }
  addComment(document: Document) {
    const dialogRef = this.dialog.open(FileApprovalRequestPresentationComponent, {
      width: '800px',
      maxHeight: '70vh',
      data: Object.assign({}, document)
    });
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
  onDocumentView(document: Workflow) {
    const urls = document.url.split('.');
    const extension = urls[1];
    const documentView: DocumentView = {
      documentId: document.documentId,
      name: document.documentName,
      extension: extension,
      isRestricted: false,
      isVersion: false,
      isFromPreview:false
    };
    this.overlay.open(BasePreviewComponent, {
      position: 'center',
      origin: 'global',
      panelClass: ['file-preview-overlay-container', 'white-background'],
      data: documentView
    });
  }
}
