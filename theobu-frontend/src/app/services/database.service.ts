import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ICard } from '../models/card';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DatabaseService {

  constructor(private httpClient: HttpClient) { }

  public GetCards(): Observable<ICard> {
    return this.httpClient.get<ICard>('/assets/db/cards.json');
  }
}
