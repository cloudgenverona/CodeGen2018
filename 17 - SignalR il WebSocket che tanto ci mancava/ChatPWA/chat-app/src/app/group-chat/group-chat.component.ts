import { Component, OnInit, OnDestroy } from '@angular/core';
import { ChatHubService } from '../services/chatHub.service';
import { GroupDataStoreService } from '../services/group-data-store.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Message, GroupMessage } from '../models/message';
import { ChatService } from '../services/chat.service';
import { JoinGroupModel, JoinGroupNotifyModel } from '../models/groupData';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { GroupChatData } from '../models/ChatData';

@Component({
  selector: 'app-group-chat',
  templateUrl: './group-chat.component.html',
  styleUrls: ['./group-chat.component.css']
})
export class GroupChatComponent implements OnInit, OnDestroy {
  public newMessage: GroupMessage = new GroupMessage();
  public groupName: string;
  constructor(private chatHubService: ChatHubService, public groupChatDataStore: GroupDataStoreService,
    private chatService: ChatService, private activatedRoute: ActivatedRoute, private router: Router,
    private modalService: NgbModal) { }
  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      this.groupName = params['groupName'];
      this.newMessage.Group = this.groupName;
      const joinModel = new JoinGroupNotifyModel();
      joinModel.Group = this.groupName;
      joinModel.User = this.groupChatDataStore.currentUser;
      this.chatService.addUserToGroup(joinModel);
      this.chatService.getUserInGroupStats(this.groupName);
      // Clear or Set data
      let currentChat = this.groupChatDataStore.chatData.find(x => x.idChat === this.groupName);
      if (currentChat !== undefined) {
        currentChat.messages = new Array<Message>();
      } else {
        currentChat = new GroupChatData();
        currentChat.idChat = this.groupName;
        currentChat.users.push(this.groupChatDataStore.currentUser);
        this.groupChatDataStore.chatData.push(currentChat);
      }
    });
    this.newMessage.From = this.groupChatDataStore.currentUser;
    this.newMessage.TextMessage = '';
  }
  ngOnDestroy(): void {
    const joinModel = new JoinGroupNotifyModel();
    joinModel.Group = this.groupName;
    joinModel.User = this.groupChatDataStore.currentUser;
    this.chatService.removeUserFromGroup(joinModel);
  }

  addMessage() {
    this.chatHubService.addGroupMessage(this.newMessage);
    this.groupChatDataStore.addMessage(Object.assign({}, this.newMessage), this.groupName);
    this.newMessage.TextMessage = '';
  }
  getMessages(): Message[] {
    const chatData = this.groupChatDataStore.chatData.find(x => x.idChat === this.groupName);
    if (chatData === undefined) {
      return new Array<Message>();
    }
    return chatData.messages;
  }
  leaveGroup() {
    this.router.navigate(['/chat-list']);
  }
  getCurrentChat(): GroupChatData {
    let currentChat = this.groupChatDataStore.chatData.find(x => x.idChat === this.groupName);
    if (currentChat === undefined) {
      currentChat = new GroupChatData();
    }
    return currentChat;
  }
  showUsersConnected(content) {
    this.modalService.open(content, { centered: true, size: 'lg' });
  }
}
