import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'stock-watch',
  templateUrl: './stock-watch.component.html',
  styleUrls: ['./stock-watch.component.css']
})
export class StockWatchComponent implements OnInit {

  symbols: Array<string>;

  constructor() { 
    this.symbols = ["APPL", "GOOG"];
  }

  ngOnInit() {
  }

}
