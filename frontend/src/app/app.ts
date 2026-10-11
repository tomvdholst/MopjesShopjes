import { Component, signal, OnInit, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Item } from './item';

@Component({
  imports: [],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})

export class App implements OnInit {
  private readonly http = inject(HttpClient);
  protected readonly items = signal<Item[]>([]);

  private loadItems() {
    this.http.get<Item[]>('/api/items').subscribe(data => {
      this.items.set(data);
    });
  }

  ngOnInit() {
    this.loadItems();
  }

  addItem(name: string){
    this.http.post<Item>('/api/items', {name}).subscribe(() => {
      this.loadItems();
    });
  }

  deleteItem(id: number){
    this.http.delete(`/api/items/${id}`).subscribe(() => {
      this.loadItems();
    });
  }

  toggleItem(id: number){
    this.http.patch(`/api/items/${id}`, {}).subscribe(() => {
      this.loadItems();
    });
  }
}
