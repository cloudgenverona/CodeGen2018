import { Injectable } from '@angular/core';
import { UserSignalR } from '../models/userStats';
import { PrivateChatData, GroupChatData } from '../models/ChatData';
import { Message } from '../models/message';

@Injectable({
  providedIn: 'root'
})
export class GroupDataStoreService {
  public currentUser: UserSignalR;
  public chatData: GroupChatData[] = new Array<GroupChatData>();
  constructor() {}
  addMessage(message: Message, idChat: string) {
    let data = this.chatData.find(x => x.idChat === idChat);
    if (data === undefined) {
      data = new GroupChatData();
      data.idChat = idChat;
      this.chatData.push(data);
    }
    data.messages.push(message);
  }
  addUser(user: UserSignalR, idChat: string) {
    const data = this.chatData.find(x => x.idChat === idChat);
    if (data !== undefined) {
      const userExist = data.users.find(x => x.Username === user.Username);
      if (userExist === undefined) {
        data.users.push(user);
      }
    }
  }
  removeUser(user: UserSignalR, idChat: string) {
    const data = this.chatData.find(x => x.idChat === idChat);
    if (data !== undefined) {
      const index = data.users.findIndex(x => x.Username === user.Username);
      if (index >= 0) {
        data.users.slice(index, 1);
      }
    }
  }
}
