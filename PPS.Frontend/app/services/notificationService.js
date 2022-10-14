'use strict';
angular.module('AtlasPPS').factory('notificationService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', 'toaster',
    function ($http, $q, localStorageService, ngAuthSettings, toaster) {
        var notifyServiceFactory = {};

        notifyServiceFactory.showSuccessNotificatoin = function (msg) {
            toaster.success({ body: msg });
        };
        notifyServiceFactory.showErrorNotificatoin = function (msg) {
            toaster.error({ body: msg });
        };

        return notifyServiceFactory;
    }]);