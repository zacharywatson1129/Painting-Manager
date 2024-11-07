import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DownloadsComponent } from './downloads/downloads.component';

export const routes: Routes = [    
    {
        path: '',
        component: HomeComponent,
        title: 'Home Page'
    },
    {
        path: 'downloads',
        component: DownloadsComponent,
        title: 'Downloads Page'
    }
];
