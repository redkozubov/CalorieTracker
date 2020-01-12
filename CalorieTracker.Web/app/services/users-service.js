//
// Provides access to user management
//
app.service('UsersService', ['$http', '$log', '$rootScope',
    function ($http, $log, $rootScope) {
        var self = this;

        self.items = [];

        self.load = function () {
            self.list().then(function (data) {
                self.items = data;
            });
        };

        var baseUrl = '/api/users';

        self.list = function () {
            return $http({
                method: 'GET',
                url: baseUrl,
            }).then(function (response) {
                data = response.data;
                data.forEach(function (e) { e.Date = new Date(e.Date); });
                return response.data;
            });
        };

        self.save = function (data) {
            return $http({
                method: 'POST',
                url: baseUrl,
                data: data,
            }).then(function (response) {
                self.load();
                return response.data;
            });
        };

        self.delete = function (id) {
            return $http({
                method: 'DELETE',
                url: baseUrl + '/' + id,
            }).then(function (response) {
                self.load();
            });
        };

        $rootScope.$on('unauthorized', function () {
            self.items = [];
        });
    }]);
