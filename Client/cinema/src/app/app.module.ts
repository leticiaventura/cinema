import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './shared/material/material.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { LoginComponent } from './features/login/login.component';
import { CookieHelperService } from './shared/cookie-helper/cookie-helper.service';
import { AuthService } from './features/login/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { ModalComponent } from './shared/modal/modal.component';
import { ModalMessageComponent } from './shared/modal-message/modal-message.component';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { BrowserAnimationsModule} from '@angular/platform-browser/animations';

import ptBr from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
import { InterceptorModule } from './shared/Interceptors/interceptor.module';

registerLocaleData(ptBr)

@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        LoginComponent,
        ModalComponent,
        ModalMessageComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        MaterialModule,
        HttpClientModule,
        NoopAnimationsModule,
        LayoutModule,
        MatToolbarModule,
        MatButtonModule,
        MatSidenavModule,
        MatIconModule,
        MatListModule,
        ReactiveFormsModule,
        NgxMaterialTimepickerModule,
        BrowserAnimationsModule,
        InterceptorModule
    ],
    exports: [
        ModalComponent,
        ModalMessageComponent
    ],
    entryComponents: [ModalComponent, ModalMessageComponent],
    providers: [AuthService, CookieHelperService, { provide: LOCALE_ID, useValue: 'pt-PT' }],
    bootstrap: [AppComponent]
})
export class AppModule { }
