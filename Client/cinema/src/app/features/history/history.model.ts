export class PurchasedSession {
    id: number;
    movie: string;
    lounge: string;
    date: string;
}

export class PurchasedSessions {
    items: PurchasedSession[];
    count: number;
}