import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Route } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule],
  template: `<main class="nav-bar">
              <a routerLink="/">Home</a>
              <a routerLink="downloads">Downloads</a>
            </main>
            <section class="content">
              <router-outlet></router-outlet>
            </section>
          `,
  styleUrl: './app.component.css'
})
export class AppComponent {

  title = 'basic-angular-site';

  

}
