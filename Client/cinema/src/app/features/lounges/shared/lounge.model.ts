export class Lounge {
    id: number;
    rows: number;
    columns: number;
    name: String;
}

export class LoungeAddCommand {
    rows: number;
    columns: number;
    name: String;

    constructor(value) {
        this.rows = value.rows;
        this.columns = value.columns;
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
    rows: number;
    columns: number;
    name: String;

    constructor(value) {
        this.rows = value.rows;
        this.columns = value.columns;
        this.name = value.name;
        this.id = value.id;
    }
}

export class DataGridLounges {
    items: Lounge[];
    count: number;
}