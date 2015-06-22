angular.module('EnumsModule', []).
   factory('Enums', [function () {

       var service = {
           FormAction: { Create: 1, Update: 2, Delete: 3 },
       };

       return service;

   }]);