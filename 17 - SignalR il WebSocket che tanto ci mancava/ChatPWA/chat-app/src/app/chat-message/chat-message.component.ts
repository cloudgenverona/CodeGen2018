import { Component, OnInit, Input } from '@angular/core';
import { Message } from '../models/message';
import { UserSignalR } from '../models/userStats';

@Component({
  selector: 'app-chat-message',
  templateUrl: './chat-message.component.html',
  styleUrls: ['./chat-message.component.css']
})
export class ChatMessageComponent implements OnInit {
  @Input() message: Message;
  @Input() currentUser: UserSignalR;
  @Input() IsGroup: boolean;
  constructor() {
    this.IsGroup = true;
  }

  ngOnInit() {
  }
  isMyMessage(): boolean {
    return this.currentUser.Username === this.message.From.Username;
  }
}
