import { Component, OnInit } from '@angular/core';
declare var $: any;
@Component({
    selector: 'es-sidebar',
    templateUrl: './sidebar.html',
    styleUrls: [('./sidebar.scss')]
})

export class SidebarComponent implements OnInit {
    ngOnInit() {
        $.getScript('../../assets/js/sidebar-moving-tab.js');
    }
}