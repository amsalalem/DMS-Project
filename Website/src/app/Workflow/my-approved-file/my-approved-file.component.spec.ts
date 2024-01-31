import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyApprovedFileComponent } from './my-approved-file.component';

describe('MyApprovedFileComponent', () => {
  let component: MyApprovedFileComponent;
  let fixture: ComponentFixture<MyApprovedFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyApprovedFileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyApprovedFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
