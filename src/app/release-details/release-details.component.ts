import { Component, inject, Input } from '@angular/core';

@Component({
  selector: 'release-details',
  standalone: true,
  imports: [],
  template: `

    <div class="release-detail text-center d-flex justify-content-between align-items-center">
      <div class="release-info">
        <h2 class="display-4">{{ myRelease.name }}</h2>
        <p class="lead"><strong>Published on: {{ myRelease.published_at.split("T")[0] }}</strong></p>
      </div>
      
      
        <a class="download-link btn btn-primary d-flex align-items-center" [href]="myRelease.html_url" target="_blank">
          <svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" fill="currentColor" class="bi bi-download" viewBox="0 0 16 16">
            <path d="M.5 9.9a.5.5 0 0 1 .5.5v2.5a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1v-2.5a.5.5 0 0 1 1 0v2.5a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2v-2.5a.5.5 0 0 1 .5-.5"/>
            <path d="M7.646 11.854a.5.5 0 0 0 .708 0l3-3a.5.5 0 0 0-.708-.708L8.5 10.293V1.5a.5.5 0 0 0-1 0v8.793L5.354 8.146a.5.5 0 1 0-.708.708z"/>
          </svg>
        </a>
      
    </div>
  `,
  styleUrl: './release-details.component.css'
})
export class ReleaseDetailsComponent {

  @Input() myRelease: any;
}
