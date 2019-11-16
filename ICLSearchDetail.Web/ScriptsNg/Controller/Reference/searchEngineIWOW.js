app.controller('searchEngineIWOW', ['$scope', '$timeout', function ($scope, $timeout) {

    console.log("#1 searchEngineIWOW controller JS");
    var host = 'http://localhost:53325';
    //var host = 'http://10.200.1.39:9863';

    $scope.isVisible = false;
    $scope.radioDisabled2 = false;
    $scope.radioDisabled1 = false;

    $scope.btnSearchClick = function () {


        console.log('Start search by User ID');

        var radioSearchUserID = document.getElementById('radioSearchUserID');
        var radioSearchCheckNo = document.getElementById('radioSearchCheckNo');
        var userID;
        var date;
        var time;
        var param;
        var that = this;

        if (radioSearchUserID.checked && document.getElementById('txtUserId').value !== "" && document.getElementById('input-date').value !== "") {
            $scope.IWOWDetails = [];
            userID = document.getElementById('txtUserId').value;
            date = document.getElementById('input-date').value;

            if (document.getElementById('input-time').value !== "") {

                time = document.getElementById('input-time').value;
                time = time.replace(":", "|");
            }

            console.log('User Id :', userID);
            console.log('Input Date: ', date);
            console.log('Input Time: ', time);

            param = userID + "|" + date + "|" + time;

            $.ajax({

                type: 'GET',
                url: host + "/api/searchEngine/searchIWOW/userID/" + param,
                success: function (blob) {
                    var jsonParse = JSON.parse(blob);
                    console.log("jsonParse", jsonParse);
                    if (jsonParse.length !== 0) {
                        if (time === undefined) {

                            $scope.IWOWDetailsEx = jsonParse;
                            $scope.$apply();


                            var fileNameAcc = "";

                            fileNameAcc = "Inward Check Details - Export All.xls";
                            tab = document.getElementById('tblIWOWDetailsEx'); // id of table


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
                            that.openDialog("Done Extract");
                            console.log("$scope.IWOWDetailsEx", $scope.IWOWDetailsEx);
                            return (a);

                        } else {
                            $scope.isVisible = true;
                            $scope.IWOWDetails = jsonParse;
                            console.log("$scope.IWOWDetails", $scope.IWOWDetails);
                        }
                    } else {
                        that.openDialog("No result Found for User ID: " + userID + " .");
                    }
                    $scope.$apply();
                },
                error: function (a, b, c) {
                    console.log("Nim in ajax erroor ", a);
                }

            });
        }
        else if (!radioSearchCheckNo.checked && radioSearchUserID.checked) {
            that.openDialog("Please enter User ID/Date.");
        }

        else if (radioSearchCheckNo.checked && document.getElementById('txtCheckNumber').value !== "") {

            var chkNumber = document.getElementById('txtCheckNumber').value;
            var userId = "";
            var date = "";
            $scope.IWOWDetails = [];

            if (document.getElementById('input-date1').value !== "") {
                date = document.getElementById('input-date1').value;

            }

            if (document.getElementById('textUserCheck').value !== "") {
                userId = document.getElementById('textUserCheck').value;

            }

            chkNumber = chkNumber + '|' + date + '|' + userId;

            $.ajax({
                type: 'GET',
                url: host + "/api/searchEngine/searchIWOW/checkNo/" + chkNumber,
                success: function (blob) {

                    var jsonParse = JSON.parse(blob);
                    //console.log("jsonParse", jsonParse);
                    if (jsonParse.length !== 0) {
                        $scope.IWOWDetails = jsonParse;

                        document.getElementById("btnSearch").disabled = false;
                        document.getElementById("btnReset").disabled = false;
                        $scope.$apply();
                    }
                    else {
                        that.openDialog("Check Number does not exist.");
                    }
                },
                error: function (a, b, c) {
                    //$('#loading').hide();
                    //console.log("Nim in ajax erroor ", a);
                }
            });

            $scope.isVisible = true;
        }

        else if (radioSearchCheckNo.checked && !radioSearchUserID.checked) {
            that.openDialog("Please enter the check number.");
        }
        //if for check no



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

        $scope.radioSearchBatchClick = function () {
            //console.logs("batchClick1");
            $scope.radioDisabled1 = true;
            $scope.radioDisabled2 = false;
            $scope.isVisible = false;
            $scope.IWOWDetails = [];
            document.getElementById('radioSearchCheckNo').checked = false;
            document.getElementById('txtCheckNumber').value = "";
            document.getElementById('textUserCheck').value = "";
            document.getElementById('input-date1').value = "";
        },

        $scope.radioSearchBatchClick1 = function () {
            $scope.isVisible = false;
            $scope.IWOWDetails = [];
            $scope.radioDisabled2 = true;
            $scope.radioDisabled1 = false;
            document.getElementById('radioSearchUserID').checked = false;
            document.getElementById('txtUserId').value = "";
            document.getElementById('input-time').value = "";
            document.getElementById('input-date').value = "";


        }

    $scope.btnResetClick = function () {
        $scope.isVisible = false;
        $scope.IWOWDetails = [];
        document.getElementById('radioSearchUserID').checked = false;
        document.getElementById('radioSearchCheckNo').checked = false;
        document.getElementById('txtUserId').value = "";
        document.getElementById('txtCheckNumber').value = "";
        document.getElementById('input-time').value = "";
        document.getElementById('input-date1').value = "";
        document.getElementById('input-date').value = "";

        $scope.radioDisabled2 = false;
        $scope.radioDisabled1 = false;

        document.getElementById("btnSearch").disabled = true;
    },
        $scope.searchByFocus = function (isBy) {

            //console.log(isBy);

            if (isBy == "1") {
                document.getElementById('radioSearchUserID').checked = true;
                document.getElementById('radioSearchCheckNo').checked = false;
                $scope.radioDisabled1 = true;
                $scope.radioDisabled2 = false;


            } else {
                document.getElementById('radioSearchUserID').checked = false;
                document.getElementById('radioSearchCheckNo').checked = true;

                $scope.radioDisabled1 = false;
                $scope.radioDisabled2 = true;
            }

            document.getElementById("btnSearch").disabled = false;
        }


}]);