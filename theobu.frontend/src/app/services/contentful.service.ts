import { Injectable } from '@angular/core';
import { createClient, ContentfulClientApi, CreateClientParams } from 'contentful';
import { ICard } from '../models/card';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class ContentfulService {

  private CONFIG: CreateClientParams = {
    space: '34qzpf8upmny',
    accessToken: 'RwHFsICsB49aPqrQwmdd4lmOuMhn3l1vTbwWrwY_bJU'
  };

  private _client: ContentfulClientApi;
  private _language: string = 'de';

  constructor(private translateService: TranslateService) {
    this._client = createClient(this.CONFIG);

  //  this._language = translateService.currentLang;
  //  translateService.onTranslationChange.subscribe(l => this._language = l);
  }

  async getAllCards(): Promise<ICard[]> {
    const result = await this._client.getEntries<ICard>({locale: this._language});
    return result.items.map(c => c.fields);
  }
}
