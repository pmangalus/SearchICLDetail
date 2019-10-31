(function () {
    var app = angular.module('testApp', []);

app.controller('outwardCheckSummary', ['$scope', '$timeout', function ($scope, $timeout) {

    console.log("#1 outwardCheckSummary controller JS");
    //var host = 'http://10.200.1.39:9861';
    var host = 'http://localhost:53325';

    var iLength = 0;
    var tempIdx = 0;

    $scope.IsVisible = false;
    $scope.currentPage = 0;
    $scope.pageSize = 0;
    

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
    $scope.nextPage = function (x) {
        $scope.currentPage = $scope.currentPage + x;
        $scope.totalLength = iLength;
        //$scope.getCustomerData($scope.pageSize * $scope.currentPage);
        var offSet = $scope.pageSize * $scope.currentPage;
        var npLength = $scope.totalLength //- offSet;
        console.log("nextPage");

        var that = this;
        //api/cics/getDetailsSummary/
        $.ajax({
            type: 'GET',
            //url: host + "/api/cics/getDetailsSummary/"+x.idx,
            url: host + "/api/cics/getDetailsSummary/" + tempIdx + "/" + $scope.pageSize + "/" + offSet + "/" + npLength,
            success: function (blob) {
                var jsonParse = JSON.parse(blob);
                if (jsonParse.length !== 0) {
                    $scope.summaryDetails = jsonParse;

                    console.log($scope.summaryDetails);
                    $scope.IsVisible = true;
                    $scope.numberOfPages = () => {
                        return Math.ceil(
                            $scope.totalLength / $scope.pageSize
                        );
                    }
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

    };

    $scope.btnViewSummaryClick = function (x) {
        console.log("btnViewSummaryClick");
        console.log(x);
       
        $scope.currentPage = 0;
        $scope.pageSize = 50;
        $scope.totalLength = (x.summary).split("-")[0];
        iLength = $scope.totalLength;
        tempIdx = x.idx;

        var that = this;
        //api/cics/getDetailsSummary/
        $.ajax({
            type: 'GET',
            //url: host + "/api/cics/getDetailsSummary/"+x.idx,
            url: host + "/api/cics/getDetailsSummary/" + x.idx + "/" + $scope.pageSize + "/" + $scope.currentPage + "/" + $scope.totalLength,
            success: function (blob) {
                var jsonParse = JSON.parse(blob);
                if (jsonParse.length !== 0) {
                    $scope.summaryDetails = jsonParse;

                    $scope.IsVisible = true;
                    $scope.numberOfPages = () => {
                        return Math.ceil(
                            $scope.totalLength / $scope.pageSize
                        );
                    }
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

})();