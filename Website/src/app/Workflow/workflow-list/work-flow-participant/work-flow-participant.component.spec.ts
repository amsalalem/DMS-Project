import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkFlowParticipantComponent } from './work-flow-participant.component';

describe('WorkFlowParticipantComponent', () => {
  let component: WorkFlowParticipantComponent;
  let fixture: ComponentFixture<WorkFlowParticipantComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkFlowParticipantComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkFlowParticipantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
