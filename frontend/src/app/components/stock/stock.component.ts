import { Component, Input, OnInit, NgZone } from '@angular/core';

import { Stock } from '../../models/stock';

import { SignalrService } from '../../services/signalr.service';

@Component({
  selector: 'stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})
export class StockComponent implements OnInit {

  @Input() symbol: string;
  stock: Stock;

  constructor(private service: SignalrService, private ngZone: NgZone) { 
  }

  ngOnInit(): void {
    this.service.newStockReceived.subscribe((data: Stock) => {
      if (data.Symbol === this.symbol) {
        this.ngZone.run(() => {
          this.stock = data;
        });
      }
    });

    this.service.connectionEstablished.subscribe(() => {
      this.service.Subscribe(this.symbol);
    });
  }
}
