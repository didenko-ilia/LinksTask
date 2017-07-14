import { Injectable } from '@angular/core';
import { Link } from './link';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from "rxjs/Observable";

@Injectable()
export class DataService {
    data: Link[] = [];

    constructor(private _httpService: Http) { }

    //Server request to get initial data based on cookies
    getData(): Link[] {
        
        this._httpService.get('/link').subscribe(values => {
            var jsonObj = values.json();
            for (var item in jsonObj) {
                this.data.push(new Link(jsonObj[item]["ShortLink"], jsonObj[item]["LongLink"], jsonObj[item]["ViewCount"]));
            }
        })
        return this.data;
    }

    //Server request to add a new link
    addData(longLink: string): any {
        return this._httpService.get('/link/NewLink?longlink=' + longLink).map(values => {
            var jsonObj = values.json();
            var shortLink = jsonObj["ShortLink"];
            if (jsonObj["ViewCount"] != -1)
            {
                //Link is already on our list, view count is updated
                for (var item of this.data)
                {
                    if (item.shortLink == shortLink)
                    {
                        item.viewCount = jsonObj["ViewCount"];
                        break;
                    }
                }
            }
            else
            {
                //Link is not on our list, add new entry
                this.data.push(new Link(jsonObj["ShortLink"], jsonObj["LongLink"], 0));
            }
            return shortLink;
        });
    }
}