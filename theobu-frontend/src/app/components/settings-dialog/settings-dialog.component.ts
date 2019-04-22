import { Component, Inject, OnDestroy } from '@angular/core';
import { Language } from 'src/app/models/language';
import { LanguageService } from 'src/app/services/language.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { untilComponentDestroyed } from '@w11k/ngx-componentdestroyed';

@Component({
  selector: 'app-settings-dialog',
  templateUrl: './settings-dialog.component.html',
  styleUrls: ['./settings-dialog.component.scss']
})
export class SettingsDialogComponent implements OnDestroy {

  AVAILABLE_LANGUAGES = LanguageService.AVAILABLE_LANGUAGES;
  selectedLanguage: Language;

  constructor(
    public dialogRef: MatDialogRef<SettingsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private languageService: LanguageService) {

    this.languageService.selectedLanguage()
      .pipe(
        untilComponentDestroyed(this)
      )
      .subscribe(l => {
        this.selectedLanguage = l;
      });
  }

  setLanguage(id: string) {
    this.languageService.setLanguage(id);
  }

  ngOnDestroy() {
  }
}
