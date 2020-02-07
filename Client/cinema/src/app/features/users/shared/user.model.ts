export class User {
    id: number;
    email: string;
    password: string;
    name: String;
    type: Permission;
}

export class UserAddCommand {
    email: string;
    password: string;
    name: String;
    permissionLevel: Permission;

    constructor(value) {
        this.email = value.email;
        this.password = value.password;
        this.name = value.name;
        this.permissionLevel = value.permissionLevel;
    }
}

export class UserUpdateCommand {
    id: number;
    email: string;
    password: string;
    name: String;

    constructor(value) {
        this.email = value.email;
        this.password = value.password;
        this.name = value.name;
        this.id = value.id;
    }
}

export class DataGridUsers {
    items: User[];
    count: number;
}


export enum Permission {
    none = 0,
    admin = 1,
    employee = 2,
    customer = 3
}