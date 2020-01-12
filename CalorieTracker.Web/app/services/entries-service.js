//
// Provides access to entries list and operations
//
app.service('EntriesService', ['$http', '$log', '$rootScope', '$filter',
    function ($http, $log, $rootScope, $filter) {
        var self = this;

        self.items = [];

        self.filter = {};

        self.load = function () {
            self.list().then(function (data) {
                self.items = data;
            });
        };

        var baseUrl = '/api/entries';

        self.list = function () {
            return $http({
                method: 'GET',
                url: baseUrl,
                params: {
                    dateFrom: self.filter.dateFrom,
                    dateTo: self.filter.dateTo,
                    timeFrom: $filter('date')(self.filter.timeFrom, 'HH:mm'),
                    timeTo: $filter('date')(self.filter.timeTo, 'HH:mm')
                }
            }).then(function (response) {
                data = response.data;
                data.forEach(function (e) { e.Date = new Date(e.Date); });
                return response.data;
            }, function (response) {
                $log.warn('error loading data');
            });
        };

        self.get = function (id) {
            return $http({
                method: 'GET',
                url: baseUrl + '/' + id,
            }).then(function (response) {
                return response.data;
            });
        };

        self.save = function (data) {
            // ensure date is saved with time zone
            // this ensures that days and hours are persisted as entered by user
            // so that filtering and such will work as expected
            data.Date = $filter('date')(data.Date, 'yyyy-MM-ddTHH:mm:ssZ');
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
