export interface ICard {
    id: number;
    description: string;
    tabus: string[];
}

export class Card implements ICard {
    id: number;
    description: string;
    tabus: string[];
}
