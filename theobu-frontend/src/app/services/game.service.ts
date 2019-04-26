import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  private _publicTimer: BehaviorSubject<number> = new BehaviorSubject(null);
  
  private getTimer(): Observable<number> {
    return this._publicTimer;
  }
}
