import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { TokenInterceptor } from './services/token.interceptor';

import { AppComponent } from './app.component';
import { routing } from './app.routes';
import { LoginComponent } from './login/login.component';
import { ChatListsComponent } from './chat-lists/chat-lists.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HomeComponent } from './home/home.component';
import { AlertsComponent } from './alerts/alerts.component';
import { ChatMessageComponent } from './chat-message/chat-message.component';
import { PrivateChatComponent } from './private-chat/private-chat.component';
import { GroupChatComponent } from './group-chat/group-chat.component';
import { PrivateDataStoreService } from './services/private-data-store.service';

@NgModule({
  declarations: [
    AppComponent,
    ChatListsComponent,
    LoginComponent,
    HomeComponent,
    AlertsComponent,
    ChatMessageComponent,
    PrivateChatComponent,
    GroupChatComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    routing,
    NgbModule.forRoot()
  ],
  providers: [{
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    PrivateDataStoreService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
