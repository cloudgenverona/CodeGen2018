import { Message, PrivateMessage } from './message';
import { UserSignalR } from './userStats';

export class PrivateChatData {
  public idChat: string;
  public messages: PrivateMessage[] = new Array<PrivateMessage>();
}

export class GroupChatData {
  public idChat: string;
  public messages: Message[] = new Array<Message>();
  public users: UserSignalR[] = new Array<UserSignalR>();
}
