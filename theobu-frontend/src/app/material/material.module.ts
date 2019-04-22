import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {MatSelectModule} from '@angular/material/select';
import {MatToolbarModule} from '@angular/material/toolbar';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  exports: [
    MatToolbarModule,
    MatSelectModule
  ]
})
export class MaterialModule { }
