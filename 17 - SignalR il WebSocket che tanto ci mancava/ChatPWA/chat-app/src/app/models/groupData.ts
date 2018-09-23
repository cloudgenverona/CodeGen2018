import { UserSignalR } from './userStats';

export class GroupModel {
    public Group: string;
}
export class UpdateGroupModel extends GroupModel {
    public OldGroup: string;
}
export class JoinGroupModel extends GroupModel {
    public Username: string;
}
export class JoinGroupNotifyModel extends GroupModel {
    public User: UserSignalR;
}
