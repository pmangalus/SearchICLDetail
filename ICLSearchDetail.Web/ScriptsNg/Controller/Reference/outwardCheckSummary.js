(function () {
    var app = angular.module('testApp', []);

    app.controller('outwardCheckSummary', ['$scope', '$timeout', function ($scope, $timeout) {

        console.log("#1 outwardCheckSummary controller JS");
        //var host = 'http://10.200.1.39:9864';
        var host = 'http://localhost:53325';

        var iLength = 0;
        var tempIdx = 0;

        $scope.IsVisible = false;
        $scope.currentPage = 0;
        $scope.pageSize = 0;

        $scope.colors = ['cornflowerblue', 'cornflowerblue', 'cornflowerblue', 'cornflowerblue', 'cornflowerblue', 'cornflowerblue', 'coral'];

        var apiUrl = host + '/api/cics/outwardChechStatusSummary/';
        var that = this;

        $('#loading').show();

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
                $('#loading').hide();
            },
            error: function (a, b, c) {
                $('#loading').hide();
                //console.log("Nim in ajax erroor ", a);
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
            $('#loading').show();
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
                    $('#loading').hide();
                    $scope.$apply();

                },
                error: function (a, b, c) {
                    $('#loading').hide();
                    console.log("Nim in ajax erroor ", a);
                }

            });

        };
        $scope.getTotal = function () {
            var total = 0;

            if ($scope.loansummaryDetails !== undefined) {
                for (var i = 0; i < $scope.loansummaryDetails.length; i++) {
                    var x = $scope.loansummaryDetails[i];
                    // x.CAR_AMOUNT = x.CAR_AMOUNT == "" ? 0 : x.CAR_AMOUNT;
                    total = total + parseFloat(x.AMOUNT);
                    //console.log("total: " + total);
                }
            }
          
            return total;

        };
        $scope.btnViewSummaryClick = function (x) {
            console.log("btnViewSummaryClick");
            console.log(x);

            $scope.currentPage = 0;
            $scope.pageSize = 50;
            $scope.totalLength = (x.summary).split("-")[0];
            iLength = $scope.totalLength;
            tempIdx = x.idx; // idx = 6 for loans

            $('#loading').show();

            //api/cics/getDetailsSummary/
            if (tempIdx === 6) {
                console.log("Try");
                var that = this;
                $.ajax({
                    type: 'GET',
                    //url: host + "/api/cics/getDetailsSummary/"+x.idx,
                    url: host + "/api/cics/getDetailsSummary/" + x.idx,
                    success: function (blob) {


                        var jsonParse = JSON.parse(blob);
                        if (jsonParse.length !== 0) {
                            $scope.loansummaryDetails = jsonParse;
                            $scope.$apply();


                            var fileNameAcc = "";

                            fileNameAcc = "Loans Outward Transmitted Report - Export All.xls";
                            tab = document.getElementById('tblSummaryHidden'); // id of table


                            var tab_text = "<table border='2px'><tr>";
                            var textRange;
                            var j = 0;


                            for (j = 0; j < tab.rows.length; j++) {
                                tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
                                //tab_text=tab_text+"</tr>";
                            }



                            tab_text = tab_text + "</table>";
                            tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, ""); //remove if u want links in your table
                            tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
                            tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params



                            var ua = window.navigator.userAgent;
                            var msie = ua.indexOf("MSIE ");



                            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer
                            {
                                txtArea1.document.open("txt/html", "replace");
                                txtArea1.document.write(tab_text);
                                txtArea1.document.close();
                                txtArea1.focus();
                                a = txtArea1.document.execCommand("SaveAs", true, "excel.xls");
                            } else {
                                var blob = new Blob([tab_text], {
                                    type: 'application/vnd.ms-excel'
                                });
                                var downloadUrl = URL.createObjectURL(blob);
                                var a = document.createElement("a");
                                a.href = downloadUrl;
                                a.download = fileNameAcc;
                                document.body.appendChild(a);
                                a.click();
                            }
                            $('#loading').hide();
                            that.openDialog("Done Extract");
                            return (a);



                            //$scope.IsVisible = true;
                            //$scope.numberOfPages = () => {
                            //    return Math.ceil(
                            //        $scope.totalLength / $scope.pageSize
                            //    );
                            //}

                        }
                        else {
                            // that.openDialog("No result Found. Please try again.");
                            that.openDialog("Kindly refresh the page, possible records was already \nprocessed for the next status");
                            $scope.loansummaryDetails = {};
                        }

                        $('#loading').hide();


                    },
                    error: function (a, b, c) {
                        $('#loading').hide();
                        console.log("Nim in ajax erroor ", a);
                    }

                });

            } else {
                var that = this;
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
                        $('#loading').hide();

                    },
                    error: function (a, b, c) {
                        $('#loading').hide();
                        console.log("Nim in ajax erroor ", a);
                    }

                });
            }



        },
            $scope.openDialog = function (message) {
                var a = BootstrapDialog.show({
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
                document.getElementById("busDate").value = x.BUSINESS_DATE;
                document.getElementById("amt").value = x.AMOUNT;
                document.getElementById("account").value = x.ACCOUNT;
                document.getElementById("scannedBrstn").value = x.BRSTN;
                document.getElementById("serialNumber").value = x.SERIAL_NUMBER;
                document.getElementById("bofd_rt").value = x.BOFD_RT;
                document.getElementById("branchName").value = x.BRANCH_NAME;
                document.getElementById("bofd_acc").value = x.BOFD_ACC;
                document.getElementById("iclFileName").value = x.ICL_FILENAME;
                document.getElementById("scannedTime").value = x.SCANNED_TIME;
                document.getElementById("scannedBy").value = x.SCANNED_BY;
                document.getElementById("amtKeyingTime").value = x.AMOUNT_KEYING_TIME;
                document.getElementById("accountNoKeyingTime").value = x.ACCOUNT_NO_KEYING_TIME;
            },
            $scope.btnCloseModal = function () {
                $('#bulkTableInfoModal').modal('hide');
            }
    }]);

})();