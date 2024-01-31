import { Component, OnInit } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { BaseComponent } from 'src/app/base.component';
import { Router } from '@angular/router';
import { CommonService } from '@core/services/common.service';
import { TranslationService } from '@core/services/translation.service';
import { Workflow } from '@core/domain-classes/workflow';

@Component({
  selector: 'app-workflow-edit',
  templateUrl: './workflow-edit.component.html',
  styleUrls: ['./workflow-edit.component.css']
})
export class WorkflowEditComponent extends BaseComponent implements OnInit {
  documentForm: UntypedFormGroup;
  loading$: Observable<boolean>;
  documentSource: string;
  constructor(
    private toastrService: ToastrService,
    private router: Router,
    private commonService: CommonService,
    private translationService: TranslationService) {
    super();
    }

  ngOnInit(): void {
  }
  saveWorkFlowt(workflow: Workflow) {}
}
