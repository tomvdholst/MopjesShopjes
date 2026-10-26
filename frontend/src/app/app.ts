import { Component, signal, OnInit, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DragDropModule, CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Item } from './item';

@Component({
  imports: [DragDropModule,

  ],
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

  drop(event: CdkDragDrop<Item[]>) {
    // Update UI immediately
    const items = [...this.items()];
    moveItemInArray(items, event.previousIndex, event.currentIndex);
    this.items.set(items);

    // Update sort order
    if ( items.length === 1 ) {
      items[event.currentIndex].sortOrder = 1;
    }
    else if ( event.currentIndex === 0 ) {
      items[event.currentIndex].sortOrder = items[event.currentIndex + 1].sortOrder - 1;
    }
    else if ( event.currentIndex === items.length - 1 ) {
      items[event.currentIndex].sortOrder = items[event.currentIndex - 1].sortOrder + 1;
    }
    else {
      items[event.currentIndex].sortOrder = (items[event.currentIndex - 1].sortOrder + items[event.currentIndex + 1].sortOrder) / 2;
    }

    // Send new order to backend
    this.http.patch(`/api/items/${items[event.currentIndex].id}/order`, { sortOrder: items[event.currentIndex].sortOrder }).subscribe(() => {
      this.loadItems();
    })
  }
}
