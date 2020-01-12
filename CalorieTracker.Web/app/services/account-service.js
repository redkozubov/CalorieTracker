//
// Provides access to sing in, sign out, registration, current user profile
//
app.service('AccountService', ['$http', '$log', '$q', '$route', '$rootScope',
    function ($http, $log, $q, $route, $rootScope) {

        var self = this;

        self.pendingRequest = false;
        self.resetUser = function () {
            self.user = {
                authenticated: false,
                IsAdmin: false,
            };
        };

        self.resetUser();

        self.signIn = function (login, password) {
            var credentials = {
                UserName: login,
                Password: password
            };
            return $http({
                method: 'POST',
                url: '/api/account/logon',
                data: credentials
            }).then(function (response) {
                self.user = response.data;
                self.user.authenticated = true;
            }, function (response) {
                self.resetUser();
                return $q.reject(response);
            });
        };

        self.register = function (login, password) {
            var credentials = {
                UserName: login,
                Password: password
            };
            return $http({
                method: 'POST',
                url: '/api/account/register',
                data: credentials
            }).then(function (response) {
                // success
            }, function (response) {
                // failure
                return $q.reject(response);
            });
        }

        self.signOut = function () {
            return $http({
                method: 'POST',
                url: '/api/account/logoff'
            }).finally(function () {
                self.resetUser();
                $route.reload();
            });
        };

        self.loadCurrentUser = function () {
            return $http({
                method: 'GET',
                url: '/api/account/current'
            }).then(function (response) {
                self.user = response.data;
                self.user.authenticated = true;
            }, function (response) {
                self.resetUser();
            });
        };

        self.saveCurrentUser = function (user) {
            return $http({
                method: 'PUT',
                url: '/api/account/current',
                data: user
            }).then(function (response) {
                self.user = response.data;
                self.user.authenticated = true;
            }, function (response) {
                self.resetUser();
            });
        };

        $rootScope.$on('unauthorized', function () {
            self.resetUser();
        });

    }]);