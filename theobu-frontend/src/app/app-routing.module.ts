import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TitleScreenComponent } from './components/title-screen/title-screen.component';

const routes: Routes = [
  { path: 'title-screen', component: TitleScreenComponent },
  {
    path: '',
    redirectTo: '/title-screen',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
