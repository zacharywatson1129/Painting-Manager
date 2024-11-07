import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule],
  template: `
    
    <h1>This is the home page!!!</h1>
    
    <p>This is the home page content. Some more content...</p>
  `,
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
