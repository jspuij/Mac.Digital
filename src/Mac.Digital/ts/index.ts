import $ from 'jquery';
window.$ = window.jQuery = $;

import 'bootstrap';

import "bootstrap-input-spinner/src/bootstrap-input-spinner.js";


import { Spinner } from './spinner';

declare global {
    interface Window {
        $: JQueryStatic;
        jQuery: JQueryStatic;
        MacDigital: any;
    }
}

window.MacDigital = window.MacDigital || {};

window.MacDigital.createSpinner = function (element: HTMLElement) {
    Spinner.CreateSpinner(element);
}

