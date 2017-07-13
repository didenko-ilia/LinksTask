import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { Link } from './link';

import 'rxjs/add/operator/map';
import { Observable } from "rxjs/Observable";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  //styleUrls: ['./app.component.css'],
  providers: [DataService]
})
export class AppComponent implements OnInit {

    constructor(private dataService: DataService) {}
    
    shortLink: string = "";
    apiValue: Link[] = [];

    addItem(longLink: string)
    {
        this.dataService.addData(longLink).subscribe(value => this.shortLink = value);
    }
    
    ngOnInit() {
        this.apiValue = this.dataService.getData();
    }
}
