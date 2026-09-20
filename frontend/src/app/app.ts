import { Component, signal } from '@angular/core';
import { Item } from './item';

@Component({
  imports: [],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})

export class App {
  protected readonly items = signal<Item[]>([{identifier : 1, name : 'Milk', isChecked : false}, {identifier : 2, name : 'Butter', isChecked : false}, {identifier : 3, name : 'Eggs', isChecked : true}, {identifier : 4, name : 'Cheese', isChecked : true}]);
  private nextId = 5;
  
  toggleItem(id : number) {
    this.items.update(current =>
      current.map(item => 
        item.identifier === id ? { ...item, isChecked: !item.isChecked } : item
      )
    );
  }

  addItem(name: string){
    this.items.update(current => [... current, {identifier: this.nextId++, name, isChecked: false}]);
  }

  deleteItem(id: number){
    this.items.update(current =>
      current.filter(item => item.identifier !== id)
    )
  }
}
