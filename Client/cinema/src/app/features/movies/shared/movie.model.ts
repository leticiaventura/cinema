export class Movie {
    id: number;
    name: string;
    description: string;
    length: number;
    animation: Animation;
    audio: Audio;
}

export class MovieAddCommand {
    name: string;
    description: string;
    length: number;
    animation: Animation;
    audio: Audio;
    image: string;

    constructor(value, image) {
        this.name = value.name;
        this.description = value.description;
        this.length = value.length;
        this.animation = value.animation;
        this.audio = value.audio;
        this.image = image;
    }
}

export class MovieUpdateCommand {
    id: number;
    name: string;
    description: string;
    length: number;
    animation: Animation;
    audio: Audio;
    image: string;

    constructor(value, image) {
        this.name = value.name;
        this.description = value.description;
        this.length = value.length;
        this.animation = value.animation;
        this.audio = value.audio;
        this.id = value.id;
        this.image = image;
    }
}

export class DataGridMovies {
    items: Movie[];
    count: number;
}

export class MovieCheckNameQuery {
    id: number;
    name: String;

    constructor(name, id) {
        this.id = id;
        this.name = name;
    }
}

export class MovieReport {
    id: number;
    name: string;
    revenue: number;
}

export class DataGridMovieReport {
    items: MovieReport[];
    count: number;
}

export enum Audio {
    original = 0,
    dubbed = 1
}


export enum Animation {
    _2D = 0,
    _3D = 1
}