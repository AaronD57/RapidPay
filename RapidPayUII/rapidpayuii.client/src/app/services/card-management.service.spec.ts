import { TestBed } from '@angular/core/testing';

import { CardManagementService } from './card-management.service';

describe('CardManagementService', () => {
  let service: CardManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CardManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
