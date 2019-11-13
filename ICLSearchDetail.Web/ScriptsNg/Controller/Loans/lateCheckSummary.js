app.controller('lateCheckSummary', ['$scope', '$timeout', function ($scope, $timeout) {

    //console.log("#1 lateCheckSummary controller JS");
    //var host = 'http://10.200.1.39:9862';
    var host = 'http://localhost:53325';

    var that = this;

    var radioSearchBatch = document.getElementById('radios-0');
    var radioExport = document.getElementById('radios-1');

    radioSearchBatch.focus = true;
    $scope.btnExportDisabled = true;
    $scope.isVisible = false;			//added 11/5/2019 for bug#7416

    $('#loading').show();

    $.ajax({
        type: 'GET',
        url: host + "/api/loans/summaryLoan/",
        success: function (blob) {
            $('#loading').hide();
            var listInconsistentCountArr = [];

            ////console.log("Nim in ajax erroor ", blob);
            var jsonParse = JSON.parse(blob);
            ////console.log("jsonParse", jsonParse);
            if (jsonParse.length !== 0 && jsonParse[0].ERROR_MSG == null) {
                //var jsonReply = JSON.parse(reply);

                for (var i = 0; i < jsonParse.length; i++) {

                    if (jsonParse[i].BATCH_ID.length == 10) {
                        jsonParse[i].BATCH_ID = jsonParse[i].BATCH_ID.trim().padEnd(12, '-');
                    } else {
                        jsonParse[i].BATCH_ID = jsonParse[i].BATCH_ID.trim().padEnd(13, '-');
                    }

                    if (jsonParse[i].fldNotEqual == "1") {
                        listInconsistentCountArr.push(jsonParse[i]);
                    }
                }
                $scope.listInconsistentCount = listInconsistentCountArr;
                //$scope.listInconsistentCount = listInconsistentCountArr.sort(function (a, b) {
                //    return a - b
                //});
                $scope.listBatch = jsonParse;
                $scope.$apply();
            } else {
                document.getElementById('listBatch').value = "";
                //that.openDialog("No Batch Ids retrieved for today.");
                var a = BootstrapDialog.show({
                    message: "No Batch Ids retrieved for today.",
                    buttons: [{
                        label: 'Close',
                        action: function (dialogItself) {
                            dialogItself.close();
                        }
                    }]
                });
            }
        },
        error: function (a, b, c) {
            $('#loading').hide();
            //console.log("Nim in ajax erroor ", a);
        }

    });


    $scope.btnGoClick = function () {
        $scope.isVisible = true;			//added 11/5/2019 for bug#7416
        //console.log('displaying batch details');
        var result;
        var that = this;
        if (radioSearchBatch.checked && document.getElementById('listBatch').value != "" && $scope.listBatch !== undefined) {
            //console.log('searching');

            result = document.getElementById('listBatch').value.split("-")[0];
            var isCorrectValue = that.validateBatchID(document.getElementById('listBatch').value);
            //result = "00226_442";
            //result = "05837_28";
            if (isCorrectValue) {
                //document.getElementById('listBatch').value = "";
                $('#loading').show();
                $.ajax({
                    type: 'GET',
                    url: host + "/api/loans/summaryLoan/" + result,
                    success: function (blob) {


                        var jsonParse = JSON.parse(blob);
                        //console.log("jsonParse", jsonParse);
                        if (jsonParse.length !== 0) {

                            $scope.batchDetails = jsonParse;

                            document.getElementById("btnExport").disabled = false;
                            document.getElementById("listBatch").disabled = false;
                            $scope.$apply();

                            //console.log("$scope.batchDetails", $scope.batchDetails);

                        } else {
                            that.openDialog("Batch ID does not exist.");
                        }
                        $('#loading').hide();
                    },
                    error: function (a, b, c) {
                        $('#loading').hide();
                        //console.log("Nim in ajax erroor ", a);
                    }

                });
            } else {
                that.openDialog("Batch ID does not exist.");
            }
        } else if (radioExport.checked) {

            //that.openDialog("Export All Function is under construction. Please wait. XD");

            $('#loading').show();
            $.ajax({
                type: 'GET',
                url: host + "/api/loans/summaryLoan/exportAll/",
                success: function (blob) {
                    $('#loading').hide();
                    //var jsonParse = JSON.parse(blob);
                    var jsonParse = blob;
                    if (jsonParse.length !== 0) {

                        $scope.tblSummaryHidden = jsonParse;
                        $scope.tblSummaryHiddenlength = $scope.tblSummaryHidden.length.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
                        $scope.$apply();
                        $scope.fnExcelReport(true);

                    } else {
                        that.openDialog("Batch ID does not exist.");
                    }

                },
                error: function (a, b, c) {
                    $('#loading').hide();
                    //console.log("Nim in ajax erroor ", a);
                }
            });
        } else if (!radioExport.checked && !radioSearchBatch.checked) {
            //console.log("inside list");
            var e = document.getElementById("selectmultiple");
            var selectedInconsistent = e.options[e.selectedIndex].value.split("-")[0];

            $('#loading').show();
            $.ajax({
                type: 'GET',
                url: host + "/api/loans/summaryLoan/" + selectedInconsistent,
                success: function (blob) {


                    var jsonParse = JSON.parse(blob);
                    //console.log("jsonParse", jsonParse);
                    if (jsonParse.length !== 0) {

                        $scope.batchDetails = jsonParse;

                        document.getElementById("btnExport").disabled = false;
                        document.getElementById("listBatch").disabled = false;
                        document.getElementById('listBatch').value = "";
                        $scope.$apply();

                        //console.log("$scope.batchDetails", $scope.batchDetails);

                    } else {
                        that.openDialog("Batch ID does not exist.");
                    }
                    $('#loading').hide();
                },
                error: function (a, b, c) {
                    $('#loading').hide();
                    //console.log("Nim in ajax erroor ", a);
                }

            });
        } else {
            that.openDialog("Batch ID does not exist.");
        }
    }, //end btnGoClick
            $scope.listInconsistendDblClick = function (x) {
                $scope.isVisible = true;			//added 11/5/2019 for bug#7416
                //console.log(x);
                var that = this;
                $('#loading').show();
                $.ajax({
                    type: 'GET',
                    url: host + "/api/loans/summaryLoan/" + x.BATCH_ID.split("-")[0],
                    success: function (blob) {
                        $('#loading').hide();
                        document.getElementById("btnExport").disabled = false;
                        var jsonParse = JSON.parse(blob);
                        //console.log("jsonParse", jsonParse);
                        if (jsonParse.length !== 0) {
                            $scope.batchDetails = jsonParse;
                            $scope.$apply();
                        }
                    },
                    error: function (a, b, c) {
                        $('#loading').hide();
                        //console.log("Nim in ajax erroor ", a);
                    }

                });
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
            $scope.fnExcelReport = function (exportAll) {
                var fileNameAcc = "";
                var that = this;
                if (exportAll) {
                    fileNameAcc = "Loans Late Check Scanned Report - Export All.xls";
                    tab = document.getElementById('tblSummaryHidden'); // id of table

                } else {
                    fileNameAcc = "Loans Late Check Scanned Report - Batch ID.xls";
                    tab = document.getElementById('tblSummary'); // id of table
                }
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

                return (a);

            },
            $scope.getTotal = function () {
                var total = 0;
                if ($scope.batchDetails !== undefined && radioSearchBatch.checked) {
                    for (var i = 0; i < $scope.batchDetails.length; i++) {
                        var x = $scope.batchDetails[i];
                       // x.CAR_AMOUNT = x.CAR_AMOUNT == "" ? 0 : x.CAR_AMOUNT;
                        total = total + parseFloat(x.CAR_AMOUNT);
                        //console.log("total: " + total);
                    }
                    return total;
                }

                else if (radioExport.checked && radioSearchBatch.checked == false) {
                    //console.log("extract all export");
                    if ($scope.tblSummaryHidden != undefined) {
                        $scope.tblSummaryHiddenlength = $scope.tblSummaryHidden.length.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
                        //console.log("$scope.tblSummaryHiddenlength: ", $scope.tblSummaryHiddenlength);
                        for (var i = 0; i < $scope.tblSummaryHidden.length; i++) {
                            var x = $scope.tblSummaryHidden[i];
                            // x.CAR_AMOUNT = x.CAR_AMOUNT == "" ? 0 : x.CAR_AMOUNT;
                            total = total + parseFloat(x.CAR_AMOUNT);
                            //console.log("total: ", total);
                        }
                    }
                    return total;
                } else {
                    if ($scope.batchDetails !== undefined) {
                        for (var i = 0; i < $scope.batchDetails.length; i++) {
                            var x = $scope.batchDetails[i];
                            // x.CAR_AMOUNT = x.CAR_AMOUNT == "" ? 0 : x.CAR_AMOUNT;
                            total = total + parseFloat(x.CAR_AMOUNT);
                            //console.log("total: " + total);
                        }
                        return total;
                    }
                }

            },
            $scope.getDate = function () {
                var today = new Date().toDateString();
                return today;
            },
            $scope.listBatchFocus = function () {
                //$scope.isVisible = true;
                document.getElementById('listBatch').value = "";
                //console.log("listBatchFocus");
                radioSearchBatch.checked = true;
                //ola

                radioExport.checked = false;
                document.getElementById('listBatch').value = "";
                //document.getElementById("btnExport").disabled = true;
                //$scope.batchDetails = {};   --commented out for bug#7400

            },
            $scope.validateBatchID = function (batchValue) {
                var isValidBatch = false;
                for (var i = 0; i < $scope.listBatch.length; i++) {
                    if (document.getElementById('listBatch').value.split(" ")[0] == $scope.listBatch[i].BATCH_ID) {
                        isValidBatch = true;
                    }
                }
                return isValidBatch;
            },
            $scope.btnResetClick = function () {
                var that = this;
                $scope.isVisible = false; 	//added 11/5/2019 for bug#7416
                location.reload();
                radioSearchBatch.checked = true;
                radioExport.checked = false;
                document.getElementById('listBatch').value = "";
                document.getElementById("btnExport").disabled = true;
                $scope.batchDetails = {};
            },
            $scope.listInconsistentClick = function () {
                $scope.isVisible = true;
                document.getElementById("listBatch").disabled = false;
                document.getElementById("btnExport").disabled = true;
                document.getElementById('listBatch').value = "";
                //$scope.batchDetails = {};
                radioSearchBatch.checked = false;
                radioExport.checked = false;

            },
            $scope.radioExportAllClick = function () {
                //console.log("radioExportAllClick");
                document.getElementById("btnExport").disabled = true;
                document.getElementById('listBatch').value = "";

                //$scope.batchDetails = {};


            },
            $scope.radioSearchBatchClick = function () {
                document.getElementById("listBatch").disabled = false;
            }
    $("#listBatch").on('input', function () {
        var val = this.value;
        //console.log("new edmfvsndjkvgn");
        if ($('#resultBatch option').filter(function () {
                        return this.value.toUpperCase() === val.toUpperCase();
        }).length) {
            document.getElementById('listBatch').blur();
        }
    });

}]);

