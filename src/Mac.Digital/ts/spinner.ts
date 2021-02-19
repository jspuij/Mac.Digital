declare global {
    interface JQuery {
        inputSpinner(): JQuery;
    }
    interface JQueryStatic {
        inputSpinner(): JQuery;
    }
}

/// Spinner class.
export class Spinner {
    public static CreateSpinner(htmlElement: HTMLElement) {
        $(htmlElement).inputSpinner();
    }
}
