import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from './services/language.service';
import { Language } from './models/language';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {

  AVAILABLE_LANGUAGES = LanguageService.AVAILABLE_LANGUAGES;
  selectedLanguage: Language;

  constructor(translate: TranslateService, private languageService: LanguageService) {
    // this language will be used as a fallback when a translation isn't found in the current language
    translate.setDefaultLang('en');

    this.languageService.selectedLanguage()
      .subscribe(l => {
        this.selectedLanguage = l;
        translate.use(l.id);
      });
  }

  setLanguage(id: string) {
    this.languageService.setLanguage(id);
  }
}
