import { UserSignalR } from './userStats';

export class Message {
    From: UserSignalR;
    TextMessage: string;
    IdChat: string;
}

export class PrivateMessage extends Message {
    To: UserSignalR;
}

export class GroupMessage extends Message {
    Group: string;
}
