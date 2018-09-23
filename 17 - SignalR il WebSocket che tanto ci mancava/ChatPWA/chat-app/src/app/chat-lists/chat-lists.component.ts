import { Component, OnInit } from '@angular/core';
import { LoginService } from '../services/login.service';
import { ChatService } from '../services/chat.service';
import { ChatHubService } from '../services/chatHub.service';
import { UserSignalR } from '../models/userStats';
import { Router } from '@angular/router';
import { PrivateDataStoreService } from '../services/private-data-store.service';
import { OnlineDataStoreService } from '../services/online-data-store.service';
import { GroupModel, UpdateGroupModel } from '../models/groupData';

@Component({
  selector: 'app-chat-lists',
  templateUrl: './chat-lists.component.html',
  styleUrls: ['./chat-lists.component.css'],
})
export class ChatListsComponent implements OnInit {
  public addGroupName: string;
  public updateGroupName: string;
  constructor(public loginService: LoginService, private chatService: ChatService, private router: Router,
    private privateChatDataStore: PrivateDataStoreService, public onlineDataStore: OnlineDataStoreService) { }

  ngOnInit() {
    this.chatService.getUsersStats();
    this.chatService.getGroupsStats();
  }
  privateChat(user: UserSignalR) {
    this.router.navigate(['/private-chat'], {queryParams: {connectionId: user.ConnectionId, username: user.Username}});
  }
  groupChat(group: string) {
    this.router.navigate(['/group-chat'], {queryParams: {groupName: group}});
  }
  addGroup() {
    const model = new GroupModel();
    model.Group = this.addGroupName;
    this.chatService.addGroup(model);
  }
  editGroup(group: string) {
    const model = new UpdateGroupModel();
    model.Group = this.updateGroupName;
    model.OldGroup = group;
    this.chatService.updateGroup(model);
  }
  deleteGroup(group: string) {
    this.chatService.deleteGroup(group);
  }
}
