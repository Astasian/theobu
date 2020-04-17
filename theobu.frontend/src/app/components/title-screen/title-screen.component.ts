import { Component, OnInit } from '@angular/core';
import { ContentfulService } from 'src/app/services/contentful.service';
import { ICard } from 'src/app/models/card';

@Component({
  selector: 'tb-title-screen',
  templateUrl: './title-screen.component.html',
  styleUrls: ['./title-screen.component.scss']
})
export class TitleScreenComponent implements OnInit {

  constructor(private contentfulService: ContentfulService) { }

  currentCard: ICard;
  currentIndex = 0;
  tp: number = 100;

  timer: number = null;

  allCards: ICard[];

  ngOnInit(): void {
    this.contentfulService.getAllCards().then(l => {
      this.allCards = l;
      this.currentCard = l[this.currentIndex];
    });
    
    this.resetTimer();
  }

  nextCard() {
    this.currentIndex = (this.currentIndex + 1) % this.allCards.length;
    this.currentCard = this.allCards[this.currentIndex];
  }

  resetTimer() {
    if(this.timer) {
      window.clearInterval(this.timer);
    }
    this.timer = window.setInterval(_ => this.tp -= 1, 400);
  }
}
