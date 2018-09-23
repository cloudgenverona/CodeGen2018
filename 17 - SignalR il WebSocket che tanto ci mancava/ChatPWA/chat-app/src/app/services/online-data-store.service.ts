import { Injectable } from '@angular/core';
import { UserSignalR } from '../models/userStats';

@Injectable({
  providedIn: 'root'
})
export class OnlineDataStoreService {
  public usersConnected: number;
  public usersConnectedList: UserSignalR[] = new Array<UserSignalR>();
  public groups = 0;
  public groupsList: string[] = new Array<string>();
  constructor() { }
}
