import { Injectable } from '@angular/core';
import { ConfigService } from './../../shared/utils/config.service';

@Injectable()
export class OidcSecurityCommon {

    storage: any;

    storage_access_token = 'authorizationData';
    storage_id_token = 'authorizationDataIdToken';
    storage_is_authorized = '_isAuthorized';
    storage_user_data = 'userData';
    storage_auth_nonce = 'authNonce';
    storage_auth_state_control = 'authStateControl';
    storage_well_known_endpoints = 'wellknownendpoints';
    storage_session_state = 'session_state';
    storage_silent_renew_running = 'storage_silent_renew_running';

    constructor(private authConfiguration: ConfigService) {
    }

    setupModule() {
        this.storage = sessionStorage;
    }

    setStorage(storage: any) {
        this.storage = storage;
    }

    retrieve(key: string): any {
        if (this.storage) {
            return JSON.parse(this.storage.getItem(key));
        }

        return;
    }

    store(key: string, value: any) {
        if (this.storage) {
            this.storage.setItem(key, JSON.stringify(value));
        }
    }

    resetStorageData(isRenewProcess: boolean) {
        if (!isRenewProcess) {
            this.store(this.storage_session_state, '');
            this.store(this.storage_silent_renew_running, '');
            this.store(this.storage_is_authorized, false);
            this.store(this.storage_access_token, '');
            this.store(this.storage_id_token, '');
            this.store(this.storage_user_data, '');
        }
    }

    getAccessToken(): any {
        return this.retrieve(this.storage_access_token);
    }

    logError(message: any) {
        console.error(message);
    }

    logWarning(message: any) {
        if (this.authConfiguration.logConsoleWarningActive) {
            console.warn(message);
        }
    }

    logDebug(message: any) {
        if (this.authConfiguration.logConsoleDebugActive) {
            console.log(message);
        }
    }
}