//
// controller for statistics page
//
app.controller('StatisticsController', ['StatisticsService', 'AccountService', '$scope',
    function (StatisticsService, AccountService, $scope) {
        var self = $scope;
        StatisticsService.load();

        self.items = function () {
            return StatisticsService.items;
        };

        // for rows highligting
        self.dailyGoal = function () {
            return AccountService.user.DailyGoal;
        };
    }]);