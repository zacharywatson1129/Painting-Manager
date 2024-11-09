import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgbRatingModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, NgbRatingModule],
  template: `
    
    <div class="container">
    <div class="left-column">
      <img class="main-img" src="././Gallery-Screenshot.PNG" alt="Gallery Screenshot">
    </div>
    <div class="right-column">
      <p class="lead"><strong>A program made for artists by an artist. For those who always wanted a way to categorize and bundle a portfolio of their work. Enjoy!</strong></p>
    </div>
    </div>

    
  `,
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  rating = 8;

}
