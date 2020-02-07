import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class CookieHelperService {

    constructor() { }

    createCookie(key, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toUTCString();
        }
        else var expires = "";
        document.cookie = key + "=" + value + expires + "; path=/";
    }

    readCookie(key) {
        var nameEQ = key + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    eraseCookie(key) {
        this.createCookie(key, "", -1);
    }

}
