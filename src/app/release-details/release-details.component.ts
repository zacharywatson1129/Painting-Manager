import { Component, inject, Input } from '@angular/core';

@Component({
  selector: 'release-details',
  standalone: true,
  imports: [],
  template: `
    <p>
      release-details works!
    </p>
    <div>
      <h2>{{ myRelease.name }}</h2>
      <p>Published on: {{ myRelease.published_at.split("T")[0] }}</p>
      <a [href]="myRelease.html_url" target="_blank">View Release</a>
    </div>
  `,
  styleUrl: './release-details.component.css'
})
export class ReleaseDetailsComponent {

  @Input() myRelease: any;
}
