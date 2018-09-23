import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ChatListsComponent } from './chat-lists/chat-lists.component';
import { HomeComponent } from './home/home.component';
import { PrivateChatComponent } from './private-chat/private-chat.component';
import { GroupChatComponent } from './group-chat/group-chat.component';

const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'chat-list', component: ChatListsComponent },
    { path: 'private-chat', component: PrivateChatComponent },
    { path: 'group-chat', component: GroupChatComponent },
    { path: '**', redirectTo: '' }
  ];

export const routing = RouterModule.forRoot(appRoutes);
