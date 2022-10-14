'use strict';
angular.module('AtlasPPS').factory('authInterceptorService', ['$q', '$injector', '$location', 'localStorageService',
    function ($q, $injector, $location, localStorageService) {

        var authInterceptorServiceFactory = {};

        authInterceptorServiceFactory.request = function (config) {

            config.headers = config.headers || {};
            config.headers = {
                //'Access-Control-Allow-Origin': '*',
                //'Access-Control-Allow-Credentials': 'true',
                //'Access-Control-Allow-Methods': 'POST, GET, OPTIONS, PUT',
                //'Access-Control-Allow-Headers': 'Origin, X-Requested-With, Content-Type, Accept',
                'Content-Type': 'application/json'
            };
            //if (config.method = "POST" && _.contains(config.url, "/api/File/UploadFile")) {
            //    config.headers["Content-Type"] = 'multipart/form-data';
            //}
            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
                //'Access-Control-Allow-Methods': 'POST, GET, OPTIONS, PUT',
                //'Access-Control-Allow-Headers': 'Origin, X-Requested-With, Content-Type, Accept',
                //'Accept': 'application/json'
            }
            //$("#page-ui-loader").show();
            //$("#page-ui-view").hide();

            return config;            
        };
        authInterceptorServiceFactory.response = function (response) {
            //$("#page-ui-loader").hide();
            //$("#page-ui-view").show();
            return response;
        };
        authInterceptorServiceFactory.responseError = function (rejection) {            
            var authService = $injector.get('authService');
            authService.loadingOff();
            if (rejection.status === 401) {                
                //var authData = localStorageService.get('authorizationData');

                //if (authData) {
                //    if (authData.useRefreshTokens) {
                //        $location.path('/refresh');
                //        return $q.reject(rejection);
                //    }
                //}
                // TODO: Need to redirect to unauthorized page
                authService.logout();
                $location.path('/login');
                return;
            } else if (rejection.status === 403) {
                $location.path('/UnAuthorized');
                return;
            }
            
            return $q.reject(rejection);
        };

        //authInterceptorServiceFactory.request = _request;
        //authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
    }]);