import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { Link } from './link';
import { PatternValidator } from '@angular/forms';

@Component({
    selector: 'home-app',
    templateUrl: './home.component.html'
})

export class HomeComponent
{
    constructor(private dataService: DataService) { }

    shortLink: string = "";
    valid: boolean = true;

    //Service request to add a new link, response is a short link to be shown to the user
    addItem(longLink: string, valid: boolean) {
        this.valid = valid;
        if (valid)
          this.dataService.addData(longLink).subscribe(value => this.shortLink = value);
    }
}