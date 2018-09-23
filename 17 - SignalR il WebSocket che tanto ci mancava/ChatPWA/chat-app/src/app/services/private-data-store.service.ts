import { PrivateChatData } from '../models/ChatData';
import { Injectable } from '@angular/core';
import { PrivateMessage } from '../models/message';
import { UserSignalR } from '../models/userStats';


@Injectable({
  providedIn: 'root'
})
export class PrivateDataStoreService {
  public currentUser: UserSignalR;
  public chatData: PrivateChatData[] = new Array<PrivateChatData>();
  constructor() {}
  addMessage(message: PrivateMessage, idChat: string) {
    let data = this.chatData.find(x => x.idChat === idChat);
    if (data === undefined) {
      data = new PrivateChatData();
      data.idChat = idChat;
      this.chatData.push(data);
    }
    data.messages.push(message);
  }
}
