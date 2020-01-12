//
// controller for calorie entries list
//
app.controller('EntriesController', ['EntriesService', '$scope', '$uibModal',
    function (EntriesService, $scope, $uibModal) {
    var self = $scope;
    EntriesService.load();

    self.filter = { };

    self.reload = function () {
        EntriesService.filter = angular.copy(self.filter);
        EntriesService.load();
    };

    // entries list
    self.items = function () {
        return EntriesService.items;
    };

    // edit button click
    self.editRow = function (entry) {
        $uibModal.open({
            animation: true,
            templateUrl: 'templates/edit-entry-form.html',
            controller: 'EntryFormController',
            resolve: {
                entry: function () {
                    return entry;
                }
            }
        });
    };

    // delete button click
    self.deleteRow = function (id) {
        // todo - are you sure?
        EntriesService.delete(id);
    };

    self.createEntry = function () {
        newEntry = {
            Id: 0,
            Date: new Date(),
        };
        self.editRow(newEntry);
    };
}]);