import { Component, OnInit } from '@angular/core';
import { ChatHubService } from '../services/chatHub.service';
import { PrivateMessage } from '../models/message';
import { PrivateDataStoreService } from '../services/private-data-store.service';
import { ActivatedRoute } from '@angular/router';
import { UserSignalR } from '../models/userStats';

@Component({
  selector: 'app-private-chat',
  templateUrl: './private-chat.component.html',
  styleUrls: ['./private-chat.component.css']
})
export class PrivateChatComponent implements OnInit {
  public newMessage: PrivateMessage = new PrivateMessage();
  public toUser: UserSignalR;
  constructor(private chatHubService: ChatHubService, public privateChatDataStore: PrivateDataStoreService,
    private activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      this.toUser = {Username :  params['username'], ConnectionId: params['connectionId']};
      this.newMessage.To = this.toUser;
    });
    this.newMessage.From = this.privateChatDataStore.currentUser;
    this.newMessage.TextMessage = '';
  }

  addMessage() {
    this.chatHubService.addPrivateMessage(this.newMessage);
    this.privateChatDataStore.addMessage(Object.assign({}, this.newMessage), this.toUser.ConnectionId);
    this.newMessage.TextMessage = '';
  }
  getMessages(): PrivateMessage[] {
    const chatData = this.privateChatDataStore.chatData.find(x => x.idChat === this.toUser.ConnectionId);
    if (chatData === undefined) {
      return new Array<PrivateMessage>();
    }
    return chatData.messages;
  }
}
