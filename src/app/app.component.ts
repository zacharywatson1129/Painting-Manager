import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Route } from '@angular/router';
import { RouterModule } from '@angular/router';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, NgbNavModule],
  template: `<main>
              <nav class="navbar navbar-expand-lg navbar-light bg-light myNav">

    
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                  <a class="navbar-brand" href="#"><img class="main-logo" width="32" height="32" src="././main_icon.ico">Painting Manager</a>
                  <ul ngbNav class="navbar-nav ms-auto">
                    <li ngbNavItem class="nav-item active">
                      <a class="nav-link" routerLink="/" >Home</a>
                    </li>
                    <li ngbNavItem class="nav-item active">
                      <a class="nav-link" routerLink="downloads">Downloads</a>
                    </li>
                  </ul>
                </div>
              </nav>
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
