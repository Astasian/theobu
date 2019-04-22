import { Injectable } from '@angular/core';
import { Language } from '../models/language';
import { Observable, BehaviorSubject } from 'rxjs';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  static AVAILABLE_LANGUAGES: Language[] = [
    new Language('English', 'en'),
    new Language('Deutsch', 'de')
  ];

  private _selectedLanguage: BehaviorSubject<Language> = new BehaviorSubject(null);

  constructor() {
    this.loadLangauge();
  }

  public selectedLanguage(): Observable<Language> {
    return this._selectedLanguage.pipe(
      filter(r => r !== null)
    );
  }

  public setLanguage(id: string): void {
    const lang = LanguageService.AVAILABLE_LANGUAGES.find(l => l.id === id);
    if (lang) {
      this._selectedLanguage.next(lang);
      localStorage.setItem('lang', id);
    }
  }

  private loadLangauge() {
    const langId = localStorage.getItem('lang');
    if (!langId) {
      this.setLanguage('en');
    } else {
      const lang = LanguageService.AVAILABLE_LANGUAGES.find(l => l.id === langId);
      this._selectedLanguage.next(lang);
    }
  }
}
