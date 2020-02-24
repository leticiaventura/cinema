import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieReportRoutingModule } from './movie-report-routing.module';
import { MovieReportComponent } from './movie-report.component';
import { MaterialModule } from 'src/app/shared/material/material.module';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';


@NgModule({
  declarations: [MovieReportComponent],
  imports: [
    CommonModule,
    MovieReportRoutingModule,
    MaterialModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class MovieReportModule { }
