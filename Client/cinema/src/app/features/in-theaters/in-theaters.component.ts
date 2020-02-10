import { Router, ActivatedRoute } from '@angular/router';
import { InTheatersService } from './in-theaters.service';
import { Component, OnInit } from '@angular/core';
import { Session } from '../sessions/shared/session.model';

@Component({
    selector: 'app-in-theaters',
    templateUrl: './in-theaters.component.html',
    styleUrls: ['./in-theaters.component.scss']
})
export class InTheatersComponent implements OnInit {

    dates = [];
    private audioDisplay = {
        0: "Leg",
        1: "Dub"
    };

    private animationDisplay = {
        0: "2D",
        1: "3D"
    }

    constructor(private service: InTheatersService, private router: Router, private activeRoute: ActivatedRoute, ) { }

    ngOnInit() {
        this.selectDates();
        this.getSessions();
    }

    selectDates() {
        for (let i = 0; i < 7; i++) {
            var date = new Date();
            this.dates.push({
                date: date.setDate(date.getDate() + i)
            });
        }
        this.dates.sort();
    }

    getSessions() {
        this.dates.forEach((date) => {
            this.service.getSessionsByDate(date.date).subscribe(sessions => {
                date.sessions = sessions;
            })
        })
    }

    buy(session: Session) {
        this.router.navigate([`../compras/${session.id}`], { relativeTo: this.activeRoute });
    }
}
