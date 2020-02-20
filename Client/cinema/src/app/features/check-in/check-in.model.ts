export class SessionTicket {
    id: number;
    email: string;
    movie: string;
    lounge: string;
    date: string;
}

export class PurchasedTickets {
    items: SessionTicket[];
    count: number;
}