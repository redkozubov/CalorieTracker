//
// controller for current user profile management
//
app.controller('AccountFormController', ['AccountService', '$scope', '$uibModalInstance', '$log', 'toastr',
    function (AccountService, $scope, $uibModalInstance, $log, toastr) {
        var self = $scope;
        self.user = angular.copy(AccountService.user);

        // correct title
        self.action = 'Update profile';
        self.profileView = true;

        // block buttons while saving
        self.saving = false;

        // save button click
        self.ok = function () {
            self.saving = true;

            // save
            AccountService.saveCurrentUser(self.user).then(function (data) {
                $uibModalInstance.close();
                toastr.success('Success!');
            }, function (response) {
                $log.warn(response);
                toastr.error(response.data.Message, 'Failed');
                // error
            }).finally(function () {
                self.saving = false;
            })
        };

        // cancel button click
        self.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }]);