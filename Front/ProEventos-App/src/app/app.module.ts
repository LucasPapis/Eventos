import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EventosComponent } from './eventos/eventos.component';
import { PalestrantesComponent } from './palestrantes/palestrantes.component';
// O bootstrap preciso do AnimationsModule
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
//Para usar o Collapse co ndx-Bootstrap
import { CollapseModule } from 'ngx-bootstrap/collapse';
 //Usado para o ngModel
import { FormsModule } from '@angular/forms';
import { EventoService } from './services/evento.service';
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';

@NgModule({
  declarations: [
    AppComponent,
    EventosComponent,
    PalestrantesComponent,
    NavComponent,
    DateTimeFormatPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    //Recomendado colocar o collapse bem abaixo do AnimationsModule
    CollapseModule,
    FormsModule
  ],
  providers: [EventoService],
  bootstrap: [AppComponent]
})
export class AppModule { }
