import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'main-app',
    templateUrl: './main.component.html'
})

export class MainComponent {
    tabs: string[] = ["active", ""];

    //Initialization, opens the tab that was previously used
    ngOnInit() {
        var activeTab = localStorage.getItem("activeTab");
        if (activeTab == "List") {
            this.tabs[0] = "";
            this.tabs[1] = "active";
        }
        else {
            this.tabs[0] = "active";
            this.tabs[1] = "";
        }

    }

    //Save active tab in case of page refresh
    homeTab() {
        localStorage.setItem("activeTab", "Home");
    }

    listTab() {
        localStorage.setItem("activeTab", "List");
    }
}