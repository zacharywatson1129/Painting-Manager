import { Injectable } from '@angular/core';
import { Octokit } from '@octokit/core';

@Injectable({
  providedIn: 'root'
})
export class ReleaseService {

  async getAllReleases() {
    return (await new Octokit().request('GET /repos/zacharywatson1129/Painting-Manager/releases', {
      owner: 'zacharywatson1129',
      repo: 'Painting-Manager',
      headers: {
        'X-GitHub-Api-Version': '2022-11-28'
      }
    })).data;
  }

  // This will loop through each release, printing the release url.
  /*for (let i = 0; i < releases.length; i++) 
  {
    console.log(releases[i].name);
    console.log(releases[i].published_at.split("T")[0]);
    // console.log(releases[i].published_at);
    console.log(releases[i].html_url);
    console.log();
  }*/


  constructor() { }
}
