import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {MatSelectModule} from '@angular/material/select';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatCardModule} from '@angular/material/card';
import {MatBottomSheetModule} from '@angular/material/bottom-sheet';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatDialogModule} from '@angular/material/dialog';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  exports: [
    MatToolbarModule,
    MatSelectModule,
    MatCardModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule
  ]
})
export class MaterialModule { }
