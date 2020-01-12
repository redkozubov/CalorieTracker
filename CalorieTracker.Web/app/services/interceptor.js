//
// handles access denied responses (401, 403) and broadcasts them
//
app.service('APIInterceptor', ['$rootScope', '$location', '$q', function ($rootScope, $location, $q) {
    var service = this;

    service.responseError = function (response) {
        if (response.status === 401 | response.status === 403) {
            $rootScope.$broadcast('unauthorized');
            $location.url('/');
        }
        return $q.reject(response);
    };
}]);