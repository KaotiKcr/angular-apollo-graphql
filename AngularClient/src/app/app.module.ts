import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { MomentModule } from 'angular2-moment';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';

// Apollo
import { GraphQLModule } from './apollo.config';
import { LinkItemComponent } from './link-item/link-item.component';
import { LinkListComponent } from './link-list/link-list.component';
import { CreateLinkComponent } from './create-link/create-link.component';
import { HeaderComponent } from './header/header.component';
import { LoginComponent } from './login/login.component';
import { AuthService } from './auth.service';
import { UserListComponent } from './login/user-list/user-list.component';
import { UserItemComponent } from './login/user-item/user-item.component';
import { SearchComponent } from './search/search.component';

@NgModule({
  declarations: [
    AppComponent,
    LinkItemComponent,
    LinkListComponent,
    UserItemComponent,
    UserListComponent,
    CreateLinkComponent,
    HeaderComponent,
    LoginComponent,
    SearchComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    GraphQLModule,
    MomentModule,
    FormsModule
  ],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule {}
