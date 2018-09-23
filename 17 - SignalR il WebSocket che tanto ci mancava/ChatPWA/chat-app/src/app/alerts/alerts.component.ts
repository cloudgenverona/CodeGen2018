import { Component, OnInit, Input } from '@angular/core';
import { AlertsService, IAlert, Alert } from '../services/alerts.service';
import {Subject} from 'rxjs';
import {debounceTime} from 'rxjs/operators';

@Component({
  selector: 'app-alerts',
  templateUrl: './alerts.component.html',
  styleUrls: ['./alerts.component.css']
})
export class AlertsComponent implements OnInit {
  public alerts: Array<Alert> = this.alertService.getAlert();

  constructor(private alertService: AlertsService) { }

  ngOnInit() {
    setTimeout(() => {
      let alertsToDelete = this.alerts;
      alertsToDelete.forEach(function(a) {
        this.alertService.close(a);
      }.bind(this));
      alertsToDelete = new Array<Alert>();
    }, 2000);
  }

  closeError(message: IAlert) {
    this.alertService.close(message);
  }
}
