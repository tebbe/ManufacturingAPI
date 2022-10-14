angular.module('AtlasPPS').directive('ppsModal', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            scope.modalDismiss = function () {
                element.modal('hide');
            };
        }
    }
});