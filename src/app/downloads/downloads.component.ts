import { Component, inject } from '@angular/core';
import { ReleaseService } from '../release.service';
import { ReleaseDetailsComponent } from '../release-details/release-details.component';
import {Octokit} from "@octokit/core";
import { RouterModule } from '@angular/router';

import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-downloads',
  standalone: true,
  imports: [RouterModule, CommonModule, ReleaseDetailsComponent],
  template: `
    <h1 class="display-2 text-center">
      Painting Manager Downloads
    </h1>

    <release-details *ngFor="let release of releaseData"
    [myRelease]="release"> </release-details>

  `,
  styleUrl: './downloads.component.css'
})
export class DownloadsComponent {

  releases: ReleaseDetailsComponent[] = [];
  octokit: Octokit = new Octokit();
  releaseService = inject(ReleaseService);

  releaseData: any[] = [];
   
  constructor() {
    this.loadReleases();
  }

  async loadReleases() {
    try {
      this.releaseData = await this.releaseService.getAllReleases();
    } catch (error) {
      console.error('There was an error: ', error)
    }
  }

  // response = this.getResponse();



  

}
