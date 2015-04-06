var newControllers = angular.module('newControllers', []);

newControllers.controller('HomeCtrl', function ($scope, $http) {
    $http.post('Home/GetDriversList', null).success(function (data) {
        $scope.drivers = data;
    });

    $scope.searchBy = {};

    $scope.searchByID = function (actual, expected) { return angular.equals(expected, actual) };
});

