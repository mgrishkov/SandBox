/// <reference path="../_refferences.js" />

angular
	.module("formSampleModule")
	.controller("formSampleController", function ($scope, Enums) {
		$scope.Users = [ { Login: "FirstUser" , Email: "first-user@gmail.com" }
			           , { Login: "SecondUser", Email: "second-user@gmail.com" } ];

        $scope.SelectedUser = {
            EditFormAction: Enums.FormAction.Create
        };

		$scope.PerformAction = function(user) {
		    switch (user.EditFormAction) {
		        case Enums.FormAction.Create:
		            $scope.Create(user);
                    break;
		    }
		}

        $scope.Create = function(newUser) {
            $scope.Users.push(newUser);
        }
	});