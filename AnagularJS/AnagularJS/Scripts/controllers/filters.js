
var filters = angular.module('newFilters', []);

filters.filter('normalCase', function () {
    return function (input) {
        var aStr = input.split(' ');
        var aProp = [];
        for (str in aStr) {
            aProp.push(aStr[str].charAt(0).toUpperCase() + aStr[str].slice(1));
        }
        return aProp.join(' ');
    };

});

filters.filter('formatDate', function () {
    return function (date, formatString) {
        if (formatString === null || formatString === undefined) {
            formatString = "DD MMM YYYY";
        }
        return moment(date).format(formatString);
    };
    
});