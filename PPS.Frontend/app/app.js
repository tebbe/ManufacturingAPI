/**
 * INSPINIA - Responsive Admin Theme
 *
 */
(function () {
    angular.module('AtlasPPS', [
        'ui.router',                    // Routing
        'oc.lazyLoad',                  // ocLazyLoad
        'ui.bootstrap',                 // Ui Bootstrap
        'pascalprecht.translate',       // Angular Translate
        'ngIdle',                       // Idle timer
        'ngSanitize',                    // ngSanitize
        'LocalStorageModule',
        'ngMaterial',
        'daterangepicker',
        'ui.select',
        'ngTouch',
        'ui.grid',
        'ui.grid.pagination',
        'ui.grid.expandable',
        'ui.grid.selection',
        'ui.grid.pinning'
    ])
})();

// Other libraries are loaded dynamically in the config.js file using the library ocLazyLoad