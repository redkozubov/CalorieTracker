//
// controller for home page, user registration
//
app.controller('HomeController', ['AccountService', '$scope', 'toastr', '$log', 
    function (AccountService, $scope, toastr, $log) {

        var self = $scope;

        self.user = function () {
            return AccountService.user
        };

        self.register = function () {
            self.saving = true;
            AccountService.register(self.userName, self.password).then(
                function () { 
                    toastr.success('Success!');
                    AccountService.signIn(self.userName, self.password);
                }, function (response) {
                    $log.warn(response);
                    toastr.error(response.data.Message, 'Registration failed');
                }
            ).finally(function () {
                self.saving = false
            });
        };
    }]);