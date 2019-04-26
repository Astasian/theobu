export interface ILanguage {
    name: string;
    id: string;
}

export class Language implements ILanguage {
    name: string;
    id: string;
    constructor(name: string, id: string) {
        this.name = name;
        this.id = id;
    }
}