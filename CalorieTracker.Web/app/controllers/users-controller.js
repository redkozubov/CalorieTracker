//
// controller for user list
//
app.controller('UsersController', ['UsersService', '$scope', '$uibModal',
    function (UsersService, $scope, $uibModal) {

        var self = $scope;

        UsersService.load();

        // users list
        self.items = function () {
            return UsersService.items;
        };

        // edit button click
        self.editRow = function (user) {
            $uibModal.open({
                animation: true,
                templateUrl: 'templates/edit-user-form.html',
                controller: 'UserFormController',
                resolve: {
                    user: function () { return user; }
                }
            });
        };

        // delete button click
        self.deleteRow = function (id) {
            // todo - are you sure?
            UsersService.delete(id);
        };
    }]);