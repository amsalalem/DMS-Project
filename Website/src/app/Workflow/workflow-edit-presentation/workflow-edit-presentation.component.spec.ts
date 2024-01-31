import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowEditPresentationComponent } from './workflow-edit-presentation.component';

describe('WorkflowEditPresentationComponent', () => {
  let component: WorkflowEditPresentationComponent;
  let fixture: ComponentFixture<WorkflowEditPresentationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkflowEditPresentationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowEditPresentationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
