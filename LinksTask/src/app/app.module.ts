import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { DataService } from './data.service';
import { AppComponent } from './app.component';
import { HomeComponent } from './home.component';
import { ListComponent } from './list.component';
import { NotFoundComponent } from './notfound.component';
import { MainComponent } from './main.component';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { Routes, RouterModule } from '@angular/router';

const appRoutes: Routes = [
    { path: '', component: MainComponent },
    { path: 'home', component: MainComponent },
    //{ path: 'not-found', component: NotFoundComponent },
    { path: '**', component: NotFoundComponent }
]

@NgModule({
  declarations: [
      AppComponent,
      HomeComponent,
      ListComponent,
      NotFoundComponent,
      MainComponent
  ],
  imports: [
      RouterModule.forRoot(appRoutes),
      BrowserModule,
      FormsModule,
      HttpModule
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
