import { Component, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from '../../services/language.service';
import { MatDialog } from '@angular/material/dialog';
import { SettingsDialogComponent } from '../settings-dialog/settings-dialog.component';
import { UpdateService } from 'src/app/services/update.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(
    translate: TranslateService,
    private languageService: LanguageService,
    public dialog: MatDialog,
    private updateService: UpdateService) {
    // this language will be used as a fallback when a translation isn't found in the current language
    translate.setDefaultLang('en');

    this.languageService.selectedLanguage()
      .subscribe(l => {
        translate.use(l.id);
      });
  }

  openSettings() {
    this.dialog.open(SettingsDialogComponent);
  }
}
