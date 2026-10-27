import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { App } from './app';

describe('App', () => {
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();

    httpMock.expectOne('/api/items').flush([]);

    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should post a new item and reload the list', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    httpMock.expectOne('/api/items').flush([]);

    const compiled = fixture.nativeElement as HTMLElement;
    const input = compiled.querySelector('.add-bar input[type="text"]') as HTMLInputElement;
    input.value = 'Milk';

    fixture.componentInstance.addItem(input);

    // Verify that the input got cleared
    expect(input.value).toBe('');

    const postReq = httpMock.expectOne('/api/items');
    expect(postReq.request.method).toBe('POST');
    expect(postReq.request.body).toEqual({ name: 'Milk' });
    postReq.flush({ id: 1, name: 'Milk', isChecked: false });

    httpMock.expectOne('/api/items').flush([{ id: 1, name: 'Milk', isChecked: false }]);
    fixture.detectChanges();

    expect(compiled.textContent).toContain('Milk');
  });
  
  // Test adding an item with keypress enter
  it('should add an item when Enter is pressed', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    httpMock.expectOne('/api/items').flush([]);

    const compiled = fixture.nativeElement as HTMLElement;
    const input = compiled.querySelector('.add-bar input[type="text"]') as HTMLInputElement;
    input.value = 'Milk';

    input.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter' }));

    // Verify that the input got cleared
    expect(input.value).toBe('');

    const postReq = httpMock.expectOne('/api/items');
    expect(postReq.request.method).toBe('POST');
    expect(postReq.request.body).toEqual({ name: 'Milk' });
    postReq.flush({ id: 1, name: 'Milk', isChecked: false });

    httpMock.expectOne('/api/items').flush([{ id: 1, name: 'Milk', isChecked: false }]);
    fixture.detectChanges();

    expect(compiled.textContent).toContain('Milk');
  });
});