'use strict';
angular
    .module('AtlasPPS').factory('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
        function ($http, $q, localStorageService, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;
            var authServiceFactory = {};

            var _authentication = {
                isAuth: false,
                token: "",
                userName: "",
                refreshToken: "",
                useRefreshTokens: false,
                roles: "",
                userId: "",
                company: "",
                companyAddress: "",
                companyEmail: "",
                companyContact: "",
                companyLogoPath: "",
                companyId: "",
                fiscalYear: "",
                fullName: "",
                expires_in: "",
                totalBackAllowsDays: 0
            };

            var _saveRegistration = function (registration) {
                _logOut();

                return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
                    return response;
                });
            };

            var _login = function (loginData) {
                var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
                console.log("Login=", data);
                var deferred = $q.defer();

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                    if (response) {
                        var p;
                        if (response.policies.length > 0) {
                            p = response.policies.split(",").map(function (item) {
                                return _.parseInt(item);
                            });
                        }
                        _authentication = {
                            isAuth: true,
                            companyName: response.companyName,
                            token: response.access_token,
                            userName: loginData.userName,
                            refreshToken: response.refresh_token,
                            useRefreshTokens: true,
                            roles: response.roles,
                            userId: response.userId,
                            company: response.company,
                            companyAddress: response.companyAddress,
                            companyEmail: response.companyEmail,
                            companyContact: response.companyContact,
                            companyLogoPath: response.companyLogoPath,
                            companyId: response.companyId,
                            fiscalYear: response.fiscalYear,
                            fullName: response.fullName,
                            expires_in: response.expires_in,
                            totalBackAllowsDays: response.totalBackAllowsDays,
                            policies: p
                        };
                    } else {
                        _authentication = {
                            isAuth: false,
                            companyName: "",
                            token: "",
                            userName: "",
                            refreshToken: "",
                            useRefreshTokens: false,
                            roles: "",
                            userId: "",
                            company: "",
                            companyAddress: "",
                            companyEmail: "",
                            companyContact: "",
                            companyLogoPath: "",
                            companyId: "",
                            fiscalYear: "",
                            fullName: "",
                            expires_in: "",
                            totalBackAllowsDays: 0,
                            policies: []
                        };
                    }
                    localStorageService.set('authorizationData', _authentication);

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });

                return deferred.promise;
            };

            var _logOut = function () {
                localStorageService.remove('authorizationData');

                _authentication = {
                    isAuth: false,
                    token: "",
                    userName: "",
                    refreshToken: "",
                    useRefreshTokens: false,
                    roles: "",
                    userId: "",
                    companyId: "",
                    companyEmail: "",
                    companyContact: "",
                    companyLogoPath: "",
                    fiscalYear: "",
                    fullName: "",
                    expires_in: "",
                    totalBackAllowsDays: 0
                };
            };

            var _fillAuthData = function () {
                var authData = localStorageService.get('authorizationData');
                if (authData) {
                    _authentication = {
                        isAuth: true,
                        token: authData.access_token,
                        userName: authData.userName,
                        refreshToken: authData.refresh_token,
                        useRefreshTokens: true,
                        roles: authData.roles,
                        userId: authData.userId,
                        company: authData.company,
                        companyAddress: authData.companyAddress,
                        companyEmail: authData.companyEmail,
                        companyContact: authData.companyContact,
                        companyLogoPath: authData.companyLogoPath,
                        companyId: authData.companyId,
                        fiscalYear: authData.fiscalYear,
                        fullName: authData.fullName,
                        expires_in: authData.expires_in,
                        totalBackAllowsDays: authData.totalBackAllowsDays
                    };
                } else {
                    _logOut();
                }
            };

            var _isValidAuth = function () {
                var authData = localStorageService.get('authorizationData');
                if (authData && authData.isAuth) {
                    return true;
                } else {
                    _logOut();

                    return false;
                }
            };

            var _hasAuthentication = function (ids) {
                if (ids.length > 0) {
                    var authData = localStorageService.get('authorizationData');
                    if (authData && authData.isAuth) {
                        _.forEach(ids, function (id) {
                            var hasPolicy = _.filter(authData.policies, function (p) {
                                return p === id;
                            });
                            if (hasPolicy) return true;
                            return false;
                        });
                        return true;
                    } else {
                        _logOut();
                        return false;
                    }
                }
                return false;
            };

            var _loadingOn = function () {
                $("#page-ui-loader").show();
                $("#page-ui-view").hide();
            };
            var _loadingOff = function () {
                $("#page-ui-loader").hide();
                $("#page-ui-view").show();
            };
            //var _refreshToken = function () {
            //    var deferred = $q.defer();

            //    var authData = localStorageService.get('authorizationData');

            //    if (authData) {

            //        if (authData.useRefreshTokens) {
            //            var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;
            //            localStorageService.remove('authorizationData');
            //            $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            //                localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
            //                deferred.resolve(response);
            //            }).error(function (err, status) {
            //                _logOut();
            //                deferred.reject(err);
            //            });
            //        }
            //    }

            //    return deferred.promise;
            //};

            //var _obtainAccessToken = function (externalData) {

            //    var deferred = $q.defer();

            //    $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

            //        localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            //        _authentication.isAuth = true;
            //        _authentication.userName = response.userName;
            //        _authentication.useRefreshTokens = false;

            //        deferred.resolve(response);

            //    }).error(function (err, status) {
            //        _logOut();
            //        deferred.reject(err);
            //    });

            //    return deferred.promise;

            //};

            //var _registerExternal = function (registerExternalData) {

            //    var deferred = $q.defer();

            //    $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).success(function (response) {

            //        localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            //        _authentication.isAuth = true;
            //        _authentication.userName = response.userName;
            //        _authentication.useRefreshTokens = false;

            //        deferred.resolve(response);

            //    }).error(function (err, status) {
            //        _logOut();
            //        deferred.reject(err);
            //    });

            //    return deferred.promise;

            //};

            authServiceFactory.loadingOn = _loadingOn;
            authServiceFactory.loadingOff = _loadingOff;
            authServiceFactory.saveRegistration = _saveRegistration;
            authServiceFactory.login = _login;
            authServiceFactory.logout = _logOut;
            authServiceFactory.fillAuthData = _fillAuthData;
            authServiceFactory.authentication = _authentication;
            authServiceFactory.isValidAuth = _isValidAuth;
            authServiceFactory.hasAuthentication = _hasAuthentication;

            //authServiceFactory.refreshToken = _refreshToken;

            //authServiceFactory.obtainAccessToken = _obtainAccessToken;
            //authServiceFactory.externalAuthData = _externalAuthData;
            //authServiceFactory.registerExternal = _registerExternal;

            return authServiceFactory;
        }]);

//var serviceBase = 'http://pps-erp.azurewebsites.net/';
var serviceBase = 'http://localhost:56206/';
//var serviceBase = 'http://erp.roslyn.com.bd/';
//var serviceBase = 'http://ramateapi.ramate.net/';

angular.module('AtlasPPS').constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    reportBaseUri: '/index.html',
    clientId: 'ngAuthApp'
});

//angular.module('AtlasPPS').config(['$httpProvider', function($httpProvider) {
//    $httpProvider.interceptors.push('authInterceptorService');
//}]);

//angular.module('AtlasPPS').run(['authService', function (authService) {
//    authService.fillAuthData();
//}]);
