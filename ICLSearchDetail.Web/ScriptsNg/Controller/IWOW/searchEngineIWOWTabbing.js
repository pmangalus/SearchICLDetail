app.controller('searchEngineIWOWTabbing', ['$scope', '$timeout', function ($scope, $timeout) {

    console.log("#1 searchEngineIWOWTabbing controller JS");
    //var host = 'http://localhost:53325';
    var host = 'http://10.200.1.39:9863';

    //var radioSearchUserID = document.getElementById('radios-0');
    //var radioSearchCheckNo = document.getElementById('radios-1');

    //radioSearchUserID.focus = true;
    $scope.isVisible = false;
    $scope.radioDisabled2 = false;
    $scope.radioDisabled1 = true;
    $scope.isChecked = true;

    const timeField = document.getElementById('input-time');
    console.log("time:" + timeField.checkValidity());

    const dateField = document.getElementById('input-date');
    console.log("date:" + dateField.checkValidity());


    const dateField1 = document.getElementById('input-date1');
    console.log("date1:" + dateField1.checkValidity());


    $scope.openCity = function (evt, cityName) {

        console.log("evt ", evt);
        console.log("evt ", evt.currentTarget);
        var i, tabcontent, tablinks;
        tabcontent = document.getElementsByClassName("tabcontent");
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablinks");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }

        document.getElementById(cityName).style.display = "block";
        //evt.currentTarget.className += " active";

    },
    $scope.btnSearchClick = function () {

        console.log(timeField.checkValidity());
        console.log("date:" + dateField.checkValidity());

        var letterNumber = /^[0-9a-zA-Z]+$/;
        var regTime = /((1[0-2]|0?[1-9]):([0-5][0-9]) ?([AaPp][Mm]))/;
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
            console.log(document.getElementById('input-time').msGetInputContext);
            if (document.getElementById('input-time').value !== "") {

                time = document.getElementById('input-time').value;

                time = time.replace(":", "|");
            }

            console.log('User Id :', userID);
            console.log('Input Date: ', date);
            console.log('Input Time: ', time);

            param = userID + "|" + date + "|" + time;

            if ((userID.match(letterNumber))) {

                if (!timeField.checkValidity()) {

                    that.openDialog("Please enter a valid time (HH:MM PM/AM).");

                }
                else {

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
                                that.openDialog("No records found! Kindly verify the User ID and/or Verification Date(time) entered.");
                            }
                            $scope.$apply();
                        },
                        error: function (a, b, c) {
                            console.log("Nim in ajax erroor ", a);
                        }

                    });

                }
            }
            else {
                that.openDialog("Special Characters in User ID Field is not Allowed! ");
            }

        }
        else if (!dateField.checkValidity()) {
            that.openDialog("Please enter a valid date (mm/dd/yyyy).");
        }
        else if (!radioSearchCheckNo.checked && radioSearchUserID.checked) {
            that.openDialog("Please enter User ID/ Verification Date.");
        }

        else if (radioSearchCheckNo.checked && document.getElementById('txtCheckNumber').value !== "") {

            var chkNumber = document.getElementById('txtCheckNumber').value;
            var userId = "";
            var date = "";
            $scope.IWOWDetails = [];
            var letterNumber = /^[0-9a-zA-Z]+$/;
            var NumberS = /^[0-9]+$/;

            if (!(chkNumber.match(NumberS))) {

                that.openDialog("Please enter a valid number on Check Number Field.");

            } else if (!dateField1.checkValidity()) {
                that.openDialog("Please enter a valid date (mm/dd/yyyy).");

            } else {

                if (document.getElementById('input-date1').value !== "") {
                    date = document.getElementById('input-date1').value;

                }

                if (document.getElementById('textUserCheck').value !== "") {
                    userId = document.getElementById('textUserCheck').value;

                }

                chkNumber = chkNumber + '|' + date + '|' + userId;

                if (userId !== "" && !(userId.match(letterNumber))) {

                    that.openDialog("Special Characters in User ID Field is not Allowed!");
                }
                else {
                    $.ajax({
                        type: 'GET',
                        url: host + "/api/searchEngine/searchIWOW/checkNo/" + chkNumber,
                        success: function (blob) {

                            var jsonParseC = JSON.parse(blob);
                            //console.log("jsonParse", jsonParse);
                            if (jsonParseC.length !== 0) {
                                $scope.isVisible = true;
                                $scope.IWOWDetails = jsonParseC;

                                //document.getElementById("btnSearch").disabled = false;
                                //document.getElementById("btnReset").disabled = false;
                                $scope.$apply();

                            }
                            else {
                                $scope.isVisible = false;
                                that.openDialog("No records found! Kindly check the Check Number entered.");
                            }
                        },
                        error: function (a, b, c) {
                            //$('#loading').hide();
                            //console.log("Nim in ajax erroor ", a);
                        }
                    });
                }
            }



            //$scope.isVisible = true;
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
        document.getElementById('radioSearchUserID').checked = true;
        document.getElementById('radioSearchCheckNo').checked = false;
        document.getElementById('txtUserId').value = "";
        document.getElementById('textUserCheck').value = "";
        document.getElementById('txtCheckNumber').value = "";
        document.getElementById('input-time').value = "";
        document.getElementById('input-date1').value = "";
        document.getElementById('input-date').value = "";


        $scope.radioDisabled2 = false;
        $scope.radioDisabled1 = true;

        //$scope.isChecked = true;
        //document.getElementById("btnSearch").disabled = true;
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

