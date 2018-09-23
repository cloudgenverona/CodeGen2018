import { Component, Inject, OnInit } from '@angular/core';
import { HubConnectionBuilder, HubConnection, LogLevel} from '@aspnet/signalr';

@Component({
  selector: 'app-countdown',
  templateUrl: './countdown.component.html',
  styleUrls: ['./countdown.component.css']
})
export class CountDownComponent implements OnInit {
  public currentValue: number;
  public startCountDownValue: number;

  private connection: HubConnection;

  ngOnInit(): void {
    this.currentValue = -1;
    this.connection = new HubConnectionBuilder()
      .withUrl("/countdownhub")
      .configureLogging(LogLevel.Trace)
      .build();

    this.connection.start();
  }

  startCountDown() {
    this.connection.stream('CountDownStream', this.startCountDownValue)
      .subscribe({
        next: (item) => {
            this.currentValue = item;
        },
        complete: () => {
          this.currentValue = -1;
        },
        error: (err) => {
        },
      });
  }
}
