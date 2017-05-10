import { Injectable, EventEmitter } from '@angular/core';

import { environment } from '../../environments/environment';

import { Stock } from '../models/stock';

// declare global variable for signalr
declare var $: any;

@Injectable()
export class SignalrService {

  private connection: any;
  private stockHub: any;

  public connectionExists: boolean;
  public connectionEstablished: EventEmitter<boolean>;
  public newStockReceived: EventEmitter<Stock>;

  constructor() {
    this.newStockReceived = new EventEmitter<Stock>();
    this.connectionEstablished = new EventEmitter<boolean>();
    this.connectionExists = false;

    this.initSignalRHubs();
    this.registerOnServerEvents();
    this.startConnection();
  }

  public Subscribe(symbol: string) {
    console.log("Subscribe symbol: " + symbol);
    if (symbol && symbol.length != 0)
      this.stockHub.server.subscribe(symbol);
  }

  public Unsubscribe(symbol: string) {
    // TODO
  }

  private initSignalRHubs() {
    this.connection = $.connection;
    this.connection.hub.url = environment.signalr.url;
    this.stockHub = this.connection.stockHub;
  }

  private registerOnServerEvents() {
    this.stockHub.client.onNewStock = (stock) => {
      this.newStockReceived.emit(stock);
    };
  }

  private startConnection() {
    this.connection.hub.start()
      .done(response => {
        console.log("SignalR connection established!");
        this.connectionEstablished.emit(true);
        this.connectionExists = true;
      })
      .fail(error => {
        console.log("SignalR connection error!");
        console.error(error);
        this.connectionEstablished.emit(false);
        this.connectionExists = false;
      });
  }
}
