import { Component, OnInit } from '@angular/core';
import './../../../assets/js/material-dashboard.js';
declare var $:any;
@Component({
    selector: 'user-cmp',
    templateUrl: 'user.component.html'
})

export class UserComponent implements OnInit{
    ngOnInit(){
       $.getScript('../../../assets/js/material-dashboard.js');
    }
}
