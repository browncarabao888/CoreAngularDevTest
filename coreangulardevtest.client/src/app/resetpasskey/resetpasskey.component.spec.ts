import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetpasskeyComponent } from './resetpasskey.component';

describe('ResetpasskeyComponent', () => {
  let component: ResetpasskeyComponent;
  let fixture: ComponentFixture<ResetpasskeyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ResetpasskeyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResetpasskeyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
