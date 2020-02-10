import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  constructor(private router: Router, private activeRoute: ActivatedRoute,) { }

  ngOnInit() {
  }

  checkAccount(){
    this.router.navigate(['./conta'], { relativeTo: this.activeRoute });
  }

}
