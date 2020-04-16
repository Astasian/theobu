import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ICard } from 'src/app/models/card';

@Component({
  selector: 'tb-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss']
})
export class CardComponent {

  static TABU_COUNT = 5;

  private _card: ICard = null;

  @Input()
  set card(value: ICard) {
    if (value) {
      this._card = value;
      this.selectedTabus = this.getRandomTabus();
    }
  }

  get card(): ICard {
    return this._card;
  }

  @Input()
  timerProgress: number


  @Output()
  next = new EventEmitter<any>();

  selectedTabus: string[] = [];

  getRandomTabus(): string[] {
    const selectedTabus = [];
    const totalLength = this.card.tabus.length;
    while (selectedTabus.length < CardComponent.TABU_COUNT && selectedTabus.length < totalLength) {
      const i = Math.floor(Math.random() * totalLength);
      const e = this.card.tabus[i];
      if (!selectedTabus.includes(e)) {
        selectedTabus.push(e);
      }
      console.log(selectedTabus);
    }
    return selectedTabus;
  }

  clickNext(){
    this.next.emit();
  }
}
