//
// controller for calorie entries list
//
app.controller('EntriesController', ['EntriesService', '$scope', '$uibModal',
    function (EntriesService, $scope, $uibModal) {
    let self = $scope;
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
    
    // copy button click
    self.copyRow = function (entry) {
        let copiedEntry = angular.copy(entry);
        copiedEntry.Id = 0;
        self.editRow(copiedEntry);
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
        let newEntry = {
            Id: 0,
            Date: new Date(),
        };
        self.editRow(newEntry);
    };
}]);