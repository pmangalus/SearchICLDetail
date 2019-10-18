app.controller('outwardCheckSummary', ['$scope', '$timeout', function ($scope, $timeout) {

    console.log("#1 outwardCheckSummary controller JS");
    var host = 'http://10.200.1.39:9861';
    //var host = 'http://localhost:53325';

    var apiUrl = host + '/api/cics/outwardChechStatusSummary/';
    var that = this;
    
    $.ajax({
        type: 'GET',
        url: apiUrl,
        success: function (blob) {
            //console.log("Nim in ajax erroor ", blob);
            var jsonParse = JSON.parse(blob);
            if (jsonParse.length !== 0) {
                $scope.display = jsonParse;
                $scope.dateRefresh = new Date().toLocaleString();
            }
            else {
                that.openDialog("Kindly refresh the page, possible records was already \nprocessed for the next status");
            }
            $scope.$apply();
        },
        error: function (a, b, c) {
            console.log("Nim in ajax erroor ", a);
        }

    });

    $scope.btnViewSummaryClick = function (x) {
        console.log("btnViewSummaryClick");
        console.log(x);
        var that = this;
        //api/cics/getDetailsSummary/
        $.ajax({
            type: 'GET',
            url: host + "/api/cics/getDetailsSummary/"+x.idx,
            success: function (blob) {
                var jsonParse = JSON.parse(blob);
                if (jsonParse.length !== 0) {
                    $scope.summaryDetails = jsonParse;
                }
                else {
                    // that.openDialog("No result Found. Please try again.");
                    that.openDialog("Kindly refresh the page, possible records was already \nprocessed for the next status");
                    $scope.summaryDetails = {};
                }
                $scope.$apply();
            },
            error: function (a, b, c) {
                console.log("Nim in ajax erroor ", a);
            }

        });
    },
    $scope.openDialog = function (message) {
        var  a = BootstrapDialog.show({
            message: message,
            buttons: [{
                label: 'Close',
                action: function (dialogItself) {
                    dialogItself.close();
                }
            }]
        });
        return a;
    },
    $scope.btnDateRefresh = function () {
        location.reload();
    },
    $scope.rowSummaryClick = function (x) {
        $('#bulkTableInfoModal').modal('show');
        document.getElementById("busDate").value =x.BUSINESS_DATE;
        document.getElementById("amt").value =x.AMOUNT;
        document.getElementById("account").value =x.ACCOUNT;
        document.getElementById("scannedBrstn").value =x.BRSTN;
        document.getElementById("serialNumber").value =x.SERIAL_NUMBER;
        document.getElementById("bofd_rt").value =x.BOFD_RT;
        document.getElementById("branchName").value =x.BRANCH_NAME;
        document.getElementById("bofd_acc").value =x.BOFD_ACC;
        document.getElementById("iclFileName").value =x.ICL_FILENAME;
        document.getElementById("scannedTime").value =x.SCANNED_TIME;
        document.getElementById("scannedBy").value =x.SCANNED_BY;
        document.getElementById("amtKeyingTime").value =x.AMOUNT_KEYING_TIME;
        document.getElementById("accountNoKeyingTime").value =x.ACCOUNT_NO_KEYING_TIME;
    },
    $scope.btnCloseModal = function () {
        $('#bulkTableInfoModal').modal('hide');
    }
}]);