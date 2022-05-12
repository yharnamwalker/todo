import { Pipe, PipeTransform } from '@angular/core';
import { TodoItem } from '../../../core/models';

@Pipe({
  name: 'todoListFilter',
  pure: false
})
export class TodoListFilterPipe implements PipeTransform {
  transform(items: TodoItem[], showCompleted: boolean): any {
      if (!items || showCompleted) {
          return items;
      }

      return items.filter(item => typeof item.completed === 'undefined');
  }
}