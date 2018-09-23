import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { UserStatsResponseModels, UserStatsRequestModels, StatType, UserSignalR } from '../models/userStats';
import { OnlineDataStoreService } from './online-data-store.service';
import { GroupModel, UpdateGroupModel, JoinGroupModel, JoinGroupNotifyModel } from '../models/groupData';
import { AlertsService, AlertType } from './alerts.service';
import { GroupDataStoreService } from './group-data-store.service';
import { GroupChatData } from '../models/ChatData';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  constructor(private http: HttpClient, private onlineDataStore: OnlineDataStoreService,
    private alertService: AlertsService, private groupDataStore: GroupDataStoreService) { }

  getUsersStats() {
    const requestModel = new UserStatsRequestModels(StatType.User);
    this.http.post<UserStatsResponseModels<UserSignalR>>(environment.baseUrl + environment.controllers.UserStats, requestModel)
      .subscribe(x => {
        this.onlineDataStore.usersConnected = x.Count;
        this.onlineDataStore.usersConnectedList = x.Values;
      },
      (err) => this.alertService.add({message: JSON.stringify(err), type: AlertType.danger}));
  }

  getGroupsStats() {
    const requestModel = new UserStatsRequestModels(StatType.Group);
    this.http.post<UserStatsResponseModels<string>>(environment.baseUrl + environment.controllers.UserStats, requestModel)
    .subscribe(x => {
      this.onlineDataStore.groups = x.Count;
      this.onlineDataStore.groupsList = x.Values;
      this.onlineDataStore.groupsList.forEach(t => {
        this.getUserInGroupStats(t);
      });
    },
    (err) => this.alertService.add({message: JSON.stringify(err), type: AlertType.danger}));
  }

  getUserInGroupStats(groupName: string) {
    const userInGroupRequest = new UserStatsRequestModels(StatType.UserInGroup);
    userInGroupRequest.Group = groupName;
    this.http.post<UserStatsResponseModels<UserSignalR>>(environment.baseUrl + environment.controllers.UserStats, userInGroupRequest)
    .subscribe(x => {
      let currentGroup = this.groupDataStore.chatData.find(t => t.idChat === groupName);
      if (currentGroup !== undefined) {
        currentGroup.users.forEach(function(u) {
          const userFound = x.Values.find(t => t.Username === u.Username);
          if (userFound === undefined) {
            x.Values.push(u);
          }
        });
        currentGroup.users = x.Values;
      } else {
        currentGroup = new GroupChatData();
        currentGroup.idChat = groupName;
        currentGroup.users = currentGroup.users;
        this.groupDataStore.chatData.push(currentGroup);
      }
    },
    (err) => this.alertService.add({message: JSON.stringify(err), type: AlertType.danger}));
  }

  addGroup(group: GroupModel) {
    return this.http.post(environment.baseUrl + environment.controllers.Groups, group)
    .subscribe(() => {
      this.onlineDataStore.groups++;
      this.onlineDataStore.groupsList.push(group.Group);
    },
    (err) => this.alertService.add({message: JSON.stringify(err), type: AlertType.danger}));
  }
  updateGroup(group: UpdateGroupModel) {
    return this.http.put(environment.baseUrl + environment.controllers.Groups, group)
    .subscribe(() => {
      const oldGroupIndex = this.onlineDataStore.groupsList.findIndex(x => x === group.OldGroup);
      if (oldGroupIndex >= 0) {
        this.onlineDataStore.groupsList.splice(oldGroupIndex, 1);
        this.onlineDataStore.groupsList.push(group.Group);
      }
    },
    (err) => this.alertService.add({message: JSON.stringify(err), type: AlertType.danger}));
  }
  deleteGroup(group: string) {
    return this.http.delete(environment.baseUrl + environment.controllers.Groups + '/' + group)
    .subscribe(() => {
      const oldGroup = this.onlineDataStore.groupsList.find(x => x === group);
      if (oldGroup !== undefined) {
        const index = this.onlineDataStore.groupsList.indexOf(oldGroup);
        this.onlineDataStore.groupsList.splice(index, 1);
        this.onlineDataStore.groups--;
      }
    },
    (err) => {
      if (err.ErrorState === 409) {
        this.alertService.add({message: 'Group contains other ursers', type: AlertType.danger});
      } else {
        this.alertService.add({message: JSON.stringify(err), type: AlertType.danger});
      }
    });
  }
  addUserToGroup(group: JoinGroupNotifyModel) {
    const request = new JoinGroupModel();
    request.Group = group.Group;
    request.Username = group.User.Username;
    return this.http.post(environment.baseUrl + environment.controllers.JoinGroups + '/AddUserToGroup', request)
    .subscribe(() => { this.groupDataStore.addUser(group.User, group.Group); },
    (err) => this.alertService.add({message: JSON.stringify(err), type: AlertType.danger}));
  }
  removeUserFromGroup(group: JoinGroupNotifyModel) {
    const request = new JoinGroupModel();
    request.Group = group.Group;
    request.Username = group.User.Username;
    return this.http.post(environment.baseUrl + environment.controllers.JoinGroups + '/RemoveUserFromGroup', request)
    .subscribe(() => { this.groupDataStore.removeUser(group.User, group.Group);  },
    (err) => this.alertService.add({message: JSON.stringify(err), type: AlertType.danger}));
  }
}
