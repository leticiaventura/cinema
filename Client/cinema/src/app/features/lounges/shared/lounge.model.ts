export class Lounge {
    id: number;
    seats: number;
    name: String;
}

export class LoungeAddCommand {
    seats: number;
    name: String;

    constructor(value) {
        this.seats = value.seats;
        this.name = value.name;
    }
}

export class LoungeCheckNameQuery {
    id: number;
    name: String;

    constructor(name, id) {
        this.id = id;
        this.name = name;
    }
}

export class LoungeUpdateCommand {
    id: number;
    seats: number;
    name: String;

    constructor(value) {
        this.seats = value.seats;
        this.name = value.name;    
        this.id = value.id;
    }
}

export class DataGridLounges {
    items: Lounge[];
    count: number;
}