import { NgModule } from '@angular/core';

import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import {MatMenuModule} from '@angular/material/menu';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatDialogModule} from '@angular/material/dialog';
import {MatSelectModule} from '@angular/material/select';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatBadgeModule} from '@angular/material/badge';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import {MatRadioModule} from '@angular/material/radio';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatTableModule} from '@angular/material/table';
import {TextFieldModule} from '@angular/cdk/text-field';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatTabsModule} from '@angular/material/tabs';
import {MatChipsModule} from '@angular/material/chips';
import {MatListModule} from '@angular/material/list';

const materialModules = [
  MatButtonModule,
  MatIconModule,
  MatInputModule,
  MatFormFieldModule,
  MatProgressSpinnerModule,
  MatSnackBarModule,
  MatCheckboxModule,
  MatButtonToggleModule,
  MatMenuModule,
  MatExpansionModule,
  MatTooltipModule,
  MatDialogModule,
  MatSelectModule,
  MatAutocompleteModule,
  MatBadgeModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatRadioModule,
  MatSlideToggleModule,
  MatTableModule,
  TextFieldModule,
  MatSidenavModule,
  MatTabsModule,
  MatChipsModule,
  MatListModule,
  
];

@NgModule({
  imports: [...materialModules],
  exports: [...materialModules]
})
export class MaterialModule { }
