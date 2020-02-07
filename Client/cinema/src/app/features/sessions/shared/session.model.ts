export class Session {
    id: number;
    start: Date;
    end : Date;
    movie: string;
    lounge: string;
    price: number;
    audio: number;
    animation: number;
    freeSeats: number;
}

export class SessionAddCommand {
    start: Date;
    price: number;
    movieId: number;
    loungeId: number;

    constructor(value) {
        this.start = value.date;
        this.price = value.price;
        this.movieId = value.movieId;
        this.loungeId = value.loungeId;
    }
}

export class DataGridSessions {
    items: Session[];
    count: number;
}

export class SessionGetAvailableLoungesQuery {
    start: Date;
    movieLength: number;

    constructor(start, movieLength) {
        this.start = start;
        this.movieLength = movieLength;
    }
}