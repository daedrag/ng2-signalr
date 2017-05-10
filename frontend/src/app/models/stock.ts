export class Stock {
    public Symbol: string;
    public Price: number;
    public Time: Date;

    constructor(symbol: string, price: number, time: Date) {
        this.Symbol = symbol;
        this.Price = price;
        this.Time = time;
    }
}
