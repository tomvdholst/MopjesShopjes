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

  it('should display the shopping list heading', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();

    httpMock.expectOne('/api/items').flush([]);

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Shopping list');
  });

  it('should post a new item and reload the list', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    httpMock.expectOne('/api/items').flush([]);

    fixture.componentInstance.addItem('Milk');

    const postReq = httpMock.expectOne('/api/items');
    expect(postReq.request.method).toBe('POST');
    expect(postReq.request.body).toEqual({ name: 'Milk' });
    postReq.flush({ id: 1, name: 'Milk', isChecked: false });

    httpMock.expectOne('/api/items').flush([{ id: 1, name: 'Milk', isChecked: false }]);
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Milk');
  });
});