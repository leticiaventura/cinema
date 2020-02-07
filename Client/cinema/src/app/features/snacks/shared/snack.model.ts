export class Snack {
    id: number;
    name: string;
    price: number;
}

export class SnackAddCommand {
    name: string;
    price: number;
    image: string;

    constructor(value, image) {
        this.name = value.name;
        this.price = value.price;
        this.image = image;
    }
}

export class SnackUpdateCommand {
    id: number;
    name: string;
    price: number;
    image: string;

    constructor(value, image) {
        this.name = value.name;
        this.price = value.price;
        this.image = image;
        this.id = value.id;
    }
}

export class DataGridSnacks {
    items: Snack[];
    count: number;
}

export class SnackCheckNameQuery {
    id: number;
    name: String;

    constructor(name, id) {
        this.id = id;
        this.name = name;
    }
}