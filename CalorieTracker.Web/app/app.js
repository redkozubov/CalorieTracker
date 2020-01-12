//
// angular app declaration
//

var app = angular.module('app', ['ngRoute', 'ui.bootstrap', 'ngMessages', 'toastr']);

// route configuration
app.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider
            .when('/', {
                templateUrl: 'templates/home.html',
            })
            .when('/entries', {
                templateUrl: 'templates/entries.html',
            })
            .when('/statistics', {
                templateUrl: 'templates/statistics.html',
            })
            .when('/users', {
                templateUrl: 'templates/users.html',
            })
            .otherwise({
                redirectTo: '/'
            });
    }]);

// toastr configuration
app.config(['toastrConfig',
    function (toastrConfig) {
        angular.extend(toastrConfig, {
            positionClass: 'toast-bottom-right'
        });
    }]);

// interceptor for unauthorized access
app.config(['$httpProvider',
    function ($httpProvider) {
        $httpProvider.interceptors.push('APIInterceptor');
    }]);