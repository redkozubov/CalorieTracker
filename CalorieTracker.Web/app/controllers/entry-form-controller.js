//
// controller for entry edit/create form
//
app.controller('EntryFormController', ['EntriesService', '$scope', '$uibModalInstance', '$log', 'entry', 'toastr',
    function (EntriesService, $scope, $uibModalInstance, $log, entry, toastr) {
        var self = $scope;
        self.entry = angular.copy(entry);
        // unlink date and time fields
        self.entry.datePart = self.entry.Date;
        self.entry.timePart = self.entry.Date;

        // correct title
        self.action = entry.Id > 0 ? 'Edit entry' : 'New entry';

        // datepicker popup
        self.openDatePicker = function () {
            self.popup.opened = true;
        };
        self.popup = {
            opened: false
        };

        // block buttons while saving
        self.saving = false;

        // save button click
        self.ok = function () {
            if (self.saving) {
                return;
            }

            self.saving = true;

            // sum date and time
            var time = self.entry.timePart;
            var date = self.entry.datePart;

            self.entry.Date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), time.getHours(), time.getMinutes());

            // save
            EntriesService.save(self.entry).then(function (data) {
                $uibModalInstance.close();
                toastr.success('Entry saved');
            }, function (response) {
                $log.warn(response);
                toastr.error(response.data.Message, 'Saving failed');
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