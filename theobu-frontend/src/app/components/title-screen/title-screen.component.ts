import { Component, OnInit, HostBinding } from '@angular/core';

@Component({
  selector: 'app-title-screen',
  templateUrl: './title-screen.component.html',
  styleUrls: ['./title-screen.component.scss']
})
export class TitleScreenComponent implements OnInit {
  @HostBinding('class') flex = 'router-flex';
  
  constructor() { }

  ngOnInit() {
  }

}
