import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { catchError, map, Observable, throwError } from 'rxjs';
import { TodoService } from 'src/app/core/services/todo.service';
import { BaseComponent } from '../base.component';
import { faCheck } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent extends BaseComponent {

  vm$: Observable<any>;
  form: FormGroup;
  faCheck = faCheck;
  inProgress : boolean = false;

  constructor(
    private todoService: TodoService,
    private formBuilder: FormBuilder) {
    super();

    this.form = this.formBuilder.group({
      text: new FormControl(null)
    })

    this.vm$ = this.todoService.list$.pipe(
      map(items => {
        return {
          items
        };
      }),
      catchError(err => {
        this.handleError('Failed to fetch items', err.message);
        return throwError(() => err);
      })
    );
  }

  submit() {
    this.inProgress = true;
    this.todoService.create(this.form.get('text')?.value)
      .subscribe(() => this.form.reset()).add(() => this.inProgress = false);
  }
}
