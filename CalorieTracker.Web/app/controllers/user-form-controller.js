//
// controller for create/edit user form
//
app.controller('UserFormController', ['UsersService', '$scope', '$uibModalInstance', '$log', 'user', 'toastr',
    function (UsersService, $scope, $uibModalInstance, $log, user, toastr) {
        var self = $scope;

        self.user = angular.copy(user);

        // correct title
        self.action = user.Id > 0 ? 'Edit user' : 'New user';

        // block buttons while saving
        self.saving = false;

        // save button click
        self.ok = function () {
            self.saving = true;

            // save
            UsersService.save(self.user).then(function (data) {
                $uibModalInstance.close();
                toastr.success('user saved');
            }, function (response) {
                $log.warn(response);
                toastr.error(response.data.Message, 'Saving failed');
                // error
            }).finally(function () {
                self.saving = false
            })
        };

        // cancel button click
        self.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }]);