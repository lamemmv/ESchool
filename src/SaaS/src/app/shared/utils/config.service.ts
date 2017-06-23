import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {

    _apiURI: string;
    _adminApiURI: string;
    _pageSize: number = 10;

    constructor() {
        this._apiURI = 'http://localhost:53090/'; 
        this._adminApiURI = 'http://localhost:53090/admin/';
    }

    getApiURI() {
        return this._apiURI;
    }

    getApiHost() {
        return this._apiURI.replace('api/', '');
    }

    getAdminApiURI() {
        return this._adminApiURI;
    }

    getPageSize(): number {
        return this._pageSize;
    }
}
