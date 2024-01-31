import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FileApprovalRequestComponent } from './file-approval-request.component';

describe('FileApprovalRequestComponent', () => {
  let component: FileApprovalRequestComponent;
  let fixture: ComponentFixture<FileApprovalRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FileApprovalRequestComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FileApprovalRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
