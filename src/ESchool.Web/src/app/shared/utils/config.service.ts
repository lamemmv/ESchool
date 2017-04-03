import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
    
    _apiURI : string;
    _adminApiURI: string;

    constructor() {
        this._apiURI = 'http://localhost:27629/api/';
        this._adminApiURI = 'http://localhost:27629/admin/';
     }

     getApiURI() {
         return this._apiURI;
     };

     getApiHost() {
         return this._apiURI.replace('api/','');
     };

     getAdminApiURI(){
        return this._adminApiURI    
     };
}