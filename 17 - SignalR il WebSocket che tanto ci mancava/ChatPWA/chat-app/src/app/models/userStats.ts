class UserStatsRequestModels {
    constructor(t: StatType) {
        this.Type = t;
    }
    public Type: StatType;
    public Group: string;
}

class UserStatsResponseModels<T> {
    public Count: number;
    public Values: T[];
}

enum StatType {
    Group,
    User,
    UserInGroup
}
class UserSignalR {
    constructor(username: string, connectionId: string) {
        this.Username = username;
        this.ConnectionId = connectionId;
    }
    public Username: string;
    public ConnectionId: string;
}
export {UserStatsRequestModels, UserStatsResponseModels, StatType, UserSignalR };
