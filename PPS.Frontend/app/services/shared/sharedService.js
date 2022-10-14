﻿"use strict";

angular.module('AtlasPPS').factory('sharedService', ["PpsConstant",
    function (PpsConstant) {
        var self = this;
        self.getDemandOrderPaymentStatusClass = function (id) {
            var status = "";
            if (id === PpsConstant.DOPaymentStatus.NotApproved) {
                status = "";
            } else if (id === PpsConstant.DOPaymentStatus.Paid) {
                status = "panel-green";
            } else if (id === PpsConstant.DOPaymentStatus.Regular) {
                status = "panel-lightskyblue";
            } else if (id === PpsConstant.DOPaymentStatus.Warning) {
                status = "panel-orange";
            } else if (id === PpsConstant.DOPaymentStatus.PayDay) {
                status = "panel-blue";
            } else if (id === PpsConstant.DOPaymentStatus.Danger) {
                status = "panel-red";
            } 
            return status;
        }

        return self;
    }]);