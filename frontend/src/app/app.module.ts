import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { AppComponent } from './app.component';
import { StockWatchComponent } from './components/stock-watch/stock-watch.component';
import { StockComponent } from './components/stock/stock.component';
import { SignalrService } from './services/signalr.service';

@NgModule({
  declarations: [
    AppComponent,
    StockWatchComponent,
    StockComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    NgbModule.forRoot()
  ],
  providers: [SignalrService],
  bootstrap: [AppComponent]
})
export class AppModule { }
