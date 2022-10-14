'use strict';
angular.module('AtlasPPS').factory('reportService',
    function () {
        var reportFactory = this;

        var data;

        reportFactory.getData = function () {
            return data;
        }
        reportFactory.setData = function (data) {
            this.data = data;
        }

        return reportFactory;
    });
