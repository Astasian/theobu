import { Injectable } from '@angular/core';
import { ICard } from '../models/card';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class ContentfulService {

  async getAllCards(): Promise<ICard[]> {
   return Promise.resolve([]);
  }
}
