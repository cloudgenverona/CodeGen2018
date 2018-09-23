import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection, LogLevel, JsonHubProtocol } from '@aspnet/signalr';
import { MessagePackHubProtocol } from '@aspnet/signalr-protocol-msgpack';
import { LoginService } from './login.service';
import { environment } from '../../environments/environment.prod';
import { Message, PrivateMessage, GroupMessage } from '../models/message';
import { UserSignalR } from '../models/userStats';
import { PrivateDataStoreService } from './private-data-store.service';
import { OnlineDataStoreService } from './online-data-store.service';
import { GroupModel, JoinGroupNotifyModel } from '../models/groupData';
import { GroupDataStoreService } from './group-data-store.service';
import { AlertsService, AlertType } from './alerts.service';

@Injectable({
  providedIn: 'root'
})
export class ChatHubService {
  private connection: HubConnection;
  private isConnected: boolean;
  private currentUser: UserSignalR;

  constructor(private loginService: LoginService,
    public privateDataStore: PrivateDataStoreService, public groupDataStore: GroupDataStoreService,
    private onlineDataStore: OnlineDataStoreService, private alertsService: AlertsService) { }

  connect() {
    const token: string = this.loginService.getToken();
    if (this.connection === undefined && token !== undefined) {
      this.connection = new HubConnectionBuilder()
          .withUrl(environment.baseHubs + '/chat', {
            accessTokenFactory: () => token,
            logger: LogLevel.Trace
          })
          // .withHubProtocol(new JsonHubProtocol())
          .withHubProtocol(new MessagePackHubProtocol())
          .build();

      this.connection.start()
                     .catch(this.errorConnection.bind(this))
                     .then(x => {
                        this.connection.invoke('GetUserContext').then(this.getUserContext.bind(this));
                      });
      // Register Callback
      this.connection.onclose(this.closeConnection.bind(this));
      this.connection.on('ReceivePrivateMessage', this.receivePrivateMessage.bind(this));
      this.connection.on('ReceiveGroupMessage', this.receiveGroupMessage.bind(this));
      this.connection.on('NewConnectedUser', this.newConnectedUser.bind(this));
      this.connection.on('NewDisconnectedUser', this.newDisconnectedUser.bind(this));
      this.connection.on('NewUserInGroup', this.newUserInGroup.bind(this));
      this.connection.on('NewUserLeaveGroup', this.newUserLeaveGroup.bind(this));
      this.connection.on('NewGroup', this.newGroup.bind(this));
      this.connection.on('UpdateGroup', this.updateGroup.bind(this));
      this.connection.on('DeleteGroup', this.deleteGroup.bind(this));
    }
  }
  stop() {
    this.connection.stop();
  }
  //#region [Connection]
  closeConnection(error: Error) {
    this.isConnected = false;
    this.alertsService.add({type: AlertType.warning, message: 'Close Connection'});
    this.alertsService.add({type: AlertType.danger, message: JSON.stringify(error)});
  }
  errorConnection(error: Error) {
    this.isConnected = false;
    this.alertsService.add({type: AlertType.danger, message: JSON.stringify(error)});
    console.error(error.toString());
  }
  getUserContext(user: UserSignalR) {
    this.privateDataStore.currentUser = new UserSignalR(user.Username, user.ConnectionId);
    this.groupDataStore.currentUser = new UserSignalR(user.Username, user.ConnectionId);
  }
  //#endregion

  //#region [PrivateChat]
  addPrivateMessage(message: PrivateMessage) {
    this.connection.send('AddPrivateMessage', message);
  }
  receivePrivateMessage(message: PrivateMessage) {
    this.privateDataStore.addMessage(message, message.From.ConnectionId);
  }
  //#endregion

  //#region [GroupChat]
  addGroupMessage(message: GroupMessage) {
    this.connection.send('AddGroupMessage', message);
  }
  receiveGroupMessage(message: GroupMessage) {
    this.groupDataStore.addMessage(message, message.Group);
  }
  //#endregion

  //#region [NotifyConnection]
  newConnectedUser(user: UserSignalR) {
    const userExist = this.onlineDataStore.usersConnectedList.find(x => x.Username === user.Username);
    if (userExist) { // ReConnect
      userExist.ConnectionId = user.ConnectionId;
    } else {
      this.onlineDataStore.usersConnected++;
      this.onlineDataStore.usersConnectedList.push(user);
      this.alertsService.add({type: AlertType.info, message: 'user connected: ' + user.Username});
    }
  }
  newDisconnectedUser(user: UserSignalR) {
    const userExist = this.onlineDataStore.usersConnectedList.find(x => x.Username === user.Username);
    if (userExist) {
      const index = this.onlineDataStore.usersConnectedList.indexOf(userExist);
      this.onlineDataStore.usersConnectedList.splice(index, 1);
    }
  }
  newUserInGroup(user: JoinGroupNotifyModel) {
    this.alertsService.add({type: AlertType.info, message: user.User.Username + ' join to ' + user.Group});
    this.groupDataStore.addUser(user.User, user.Group);
  }
  newUserLeaveGroup(user: JoinGroupNotifyModel) {
    this.alertsService.add({type: AlertType.info, message: user.User.Username + ' leave from ' + user.Group});
    this.groupDataStore.removeUser(user.User, user.Group);
  }
  newGroup(group: GroupModel) {
    const groupExist = this.onlineDataStore.groupsList.find(x => x === group.Group);
    if (!groupExist) {
      this.onlineDataStore.groups++;
      this.onlineDataStore.groupsList.push(group.Group);
    }
    this.alertsService.add({type: AlertType.info, message: ' new group added: ' + group.Group});
  }
  updateGroup(group: GroupModel) {
    let groupExist = this.onlineDataStore.groupsList.find(x => x === group.Group);
    if (groupExist) {
      this.alertsService.add({type: AlertType.info, message: 'group change from ' + groupExist + ' to ' + group.Group});
      groupExist = group.Group;
    }
    this.alertsService.add({type: AlertType.info, message: 'group change name: ' + group.Group});
  }
  deleteGroup(group: GroupModel) {
    const groupExist = this.onlineDataStore.groupsList.find(x => x === group.Group);
    if (groupExist) {
      const index = this.onlineDataStore.groupsList.indexOf(groupExist);
      this.onlineDataStore.groupsList.splice(index, 1);
      this.onlineDataStore.groups--;
    }
    this.alertsService.add({type: AlertType.info, message: 'group deleted: ' + group.Group});
  }
  //#endregion
}
