/// <reference path="../_refferences.js" />

angular
	.module("formSampleModule")
	.controller("formSampleController", function ($scope, Enums) {
	    $scope.Requests = [ { RequestTypeID: 1, RequestTypeName: "Visitor Pass", RequestFor: { FirstName      : "Maxim"         , LastName: "Grishkov"      }, AccessAreas: [ { ID: 1, Name: "Adm. 1st floor" }, { ID: 2, Name: "Adm. 2nd floor"}] }
			              , { RequestTypeID: 2, RequestTypeName: "Vehicle Pass", RequestFor: { RegistrationTag: "M722HY 178RUSS", Model   : "Honda Insight" }, AccessAreas: [ { ID: 1, Name: "Parking"        }                                  ] } ];

        $scope.SelectedRequest = {
            EditFormAction: Enums.FormAction.Create
        };

        $scope.PerformAction = function (request) {
		    switch (user.EditFormAction) {
		        case Enums.FormAction.Create:
		            $scope.Create(request);
                    break;
		    }
		}

        $scope.Create = function(newRequest) {
            $scope.Requests.push(newRequest);
        }
	});