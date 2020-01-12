//
// controller for main menu
//
app.controller('MenuController', ['$scope', '$location', '$http', '$uibModal', 'AccountService', 'toastr',
    function ($scope, $location, $http, $uibModal, AccountService, toastr) {

    var self = $scope;

    self.isActive = function (route) {
        return route === $location.path();
    };

    self.user = function () {
        return AccountService.user
    };

    self.loading = false;
    self.credentials = {};

    AccountService.loadCurrentUser();

    self.signIn = function () {
        if (self.loading) {
            return;
        }
        self.loading = true;
        AccountService.signIn(self.credentials.email, self.credentials.password)
        .then(function (result) {
            toastr.success('Welcome');
        }, function (response) {
            toastr.error(response.data.Message, 'Sign in failed');
        }).finally(function () {
            self.loading = false
        });
    };

    self.signOut = function () {
        AccountService.signOut();
    };

    self.updateProfile = function () {
        $uibModal.open({
            animation: true,
            templateUrl: 'templates/edit-user-form.html',
            controller: 'AccountFormController',
        });
    }
}]);