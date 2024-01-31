import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FileApprovalRequestPresentationComponent } from './file-approval-request-presentation.component';

describe('FileApprovalRequestPresentationComponent', () => {
  let component: FileApprovalRequestPresentationComponent;
  let fixture: ComponentFixture<FileApprovalRequestPresentationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FileApprovalRequestPresentationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FileApprovalRequestPresentationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
