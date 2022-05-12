import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { TodoItem } from '../models';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  static readonly BasePath = 'https://localhost:7050/todo';

  private refreshList$ = new BehaviorSubject<boolean>(true);

  constructor(private http: HttpClient) {
  }

  list(showCompletedItems: boolean): Observable<TodoItem[]> {
    // Could be improved on with some pagination
    let parameters = new HttpParams().set("showCompletedItems", showCompletedItems);
    return this.refreshList$.pipe(switchMap(
      _ => this.http.get<TodoItem[]>(`${TodoService.BasePath}/list`, {params: parameters})
    ))
  }

  create(text: string): Observable<string> {
    return this.http.post<string>(`${TodoService.BasePath}/create`, { text }).pipe(
      tap(_ => {
        this.refreshList$.next(false);
      })
    );
  }
}
