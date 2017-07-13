import { Injectable } from '@angular/core';
import { Link } from './link';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from "rxjs/Observable";

@Injectable()
export class DataService {
    data: Link[] = [];

    constructor(private _httpService: Http) { }

    getData(): Link[] {
        
        this._httpService.get('/link').subscribe(values => {
            var jsonObj = values.json();
            for (var item in jsonObj) {
                this.data.push(new Link(jsonObj[item]["shortLink"], jsonObj[item]["longLink"], jsonObj[item]["viewCount"]));
            }
        })
        return this.data;
    }

    addData(longLink: string): any {
        return this._httpService.get('/link/NewLink?longlink=' + longLink).map(values => {
            var jsonObj = values.json();
            var shortLink = jsonObj["shortLink"];
            if (jsonObj["exists"] == "True")
            {
                for (var item of this.data)
                {
                    if (item.shortLink == shortLink)
                    {
                        item.viewCount = jsonObj["viewCount"];
                        break;
                    }
                }
            }
            else
            {
                this.data.push(new Link(jsonObj["shortLink"], jsonObj["longLink"], jsonObj["viewCount"]));
                console.log("push ", jsonObj["exists"]);
            }
            return shortLink;
        });
    }
}