/// <reference path="../_refferences.js" />

angular
	.module("formSampleModule")
	.controller("formSampleController", function($scope) {
		$scope.users = [ { Login: "FirstUser" , Email: "first-user@gmail.com" }
			           , { Login: "SecondUser", Email: "second-user@gmail.com" } ];
		
		$scope.addUser = function(user) {
			$scope.uses.push(user);
		}
	});