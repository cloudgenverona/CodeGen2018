import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AlertsService {
  private alertData: Array<Alert> = [];

  constructor() { }
  add(message: IAlert) {
    const alertMessage = new Alert();
    alertMessage.message = message.message;
    alertMessage.type = message.type;
    this.alertData.push(alertMessage);
  }
  getAlert(): Array<Alert> {
    return this.alertData;
  }
  close(message: IAlert) {
    const index: number = this.alertData.findIndex(x => x.message === message.message);
    this.alertData.splice(index, 1);
  }
}
export interface IAlert {
  type: AlertType;
  message: string;
}
export class Alert implements IAlert {
  type: AlertType;
  message: string;
  getType(): string {
    return AlertType[this.type];
  }
}

export enum AlertType {
  success,
  info,
  warning,
  danger,
  primary,
  secondary,
  light,
  dark
}
