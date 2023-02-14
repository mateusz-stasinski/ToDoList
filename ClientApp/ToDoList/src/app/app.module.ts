import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { CalendarTaskDialogComponent } from './components/calendar-task-dialog.component';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { DatePipe, HashLocationStrategy, LocationStrategy, registerLocaleData } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ReactiveFormsModule } from '@angular/forms';
import { DictionaryPipe } from './models/to-do-task';
import localePl from '@angular/common/locales/pl';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';

@NgModule({
  declarations: [
    AppComponent,
    CalendarTaskDialogComponent,
    DictionaryPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: 'pl'
    }),
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'pl-PL' },
    { provide: LOCALE_ID, useValue: 'pl' },
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

registerLocaleData(localePl);