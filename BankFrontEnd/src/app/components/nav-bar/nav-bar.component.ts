import { Component, OnInit } from '@angular/core';
// import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  items: any;
  // items: MenuItem[];

  ngOnInit(): void {
    this.items = [
      {
        label:  "Home",
        routerLink: "/"
      },
      {
        label:  "Admin",
        routerLink: "admin"
      },
      {
        label:  "Register",
        routerLink: "register"
      },
    ]
  }

}
