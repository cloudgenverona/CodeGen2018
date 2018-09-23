import { Component, OnInit, HostListener } from '@angular/core';
import { LoginService } from './services/login.service';
import { ChatHubService } from './services/chatHub.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(public loginService: LoginService, private chatHubService: ChatHubService) {
  }
  ngOnInit(): void {
  }

  @HostListener('window:beforeunload', ['$event'])
    unloadNotification($event: any) {
      this.chatHubService.stop();
      $event.returnValue = true;
    }
}
