import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { Link } from './link';

@Component({
    selector: 'list-app',
    templateUrl: './list.component.html'
})

export class ListComponent implements OnInit {

    constructor(private dataService: DataService) { };

    apiValue: Link[] = [];

    //Initialization, service request for initial data based on cookies
    ngOnInit() {
        this.apiValue = this.dataService.getData();
    }
}