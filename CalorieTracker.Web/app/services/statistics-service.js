//
// Provides access to calorie statistics
//
app.service('StatisticsService', ['$http', '$log', '$rootScope',
    function ($http, $log, $rootScope) {
        var self = this;

        self.items = [];

        self.load = function () {
            self.list().then(function (data) {
                self.items = data;
            });
        };

        var baseUrl = '/api/statistics';

        self.list = function () {
            return $http({
                method: 'GET',
                url: baseUrl,
            }).then(function (response) {
                data = response.data;
                data.forEach(function (e) { e.Date = new Date(e.Date); });
                return response.data;
            }, function (response) {
                $log.warn('error loading data');
            });
        };

        $rootScope.$on('unauthorized', function () {
            self.items = [];
        });
    }]);
