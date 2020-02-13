app.controller('byPassController', ['$scope', '$timeout', function ($scope, $timeout) {

    console.log("asdasdjhasjkdhajshdj");
    var host = 'http://localhost:53325';
    //var host = 'http://10.200.1.39:9868';
    $scope.currDate = new Date().toLocaleString();


    var currTime = $scope.currDate.split(",")[1];
    var total = "01/01/1970".concat(currTime);

    if (Date.parse("01/01/1970 ".concat(currTime)) >= Date.parse('01/01/1970 4:31:00 PM')) {
        document.getElementById("btnUpdate").disabled = false;
        //alert("active");
    } else {
        document.getElementById("btnUpdate").disabled = true;
        //alert("hidden");
    }
    getCountAmountNoKeying();

    
 
    $scope.btnUpdateClick = function () {
        var that = this;
        if ($scope.count != "0") {
            BootstrapDialog.show({
                message: "Manually Update " + $scope.count + " record/s?",
                buttons: [{
                    label: 'Yes',
                    action: function (dialogItself) {
                        $.ajax({

                            type: 'GET',
                            url: host + "/api/bypass/batchAccountNoKeyUpdate/",
                            success: function (blob) {
                                // $('#loading').hide();
                                $scope.count = blob;
                                that.openDialog(blob);
                                console.log("blob ", blob);
                                $scope.$apply();
                                getCountAmountNoKeying();
                            },
                            error: function (a, b, c) {
                                getCountAmountNoKeying();
                                console.log("Nim in ajax erroor ", a);
                                // $('#loading').hide();
                            }

                        });
                        dialogItself.close();
                    }
                },
                    {
                        label: 'No',
                        action: function (dialogItself) {
                            dialogItself.close();
                        }
                    }
                ]
            });
        } else {
            BootstrapDialog.show({
                message: "No Records to Update",
                buttons: [{
                    label: 'Ok',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
            });
        }


    },
    $scope.anchorClick = function () {
        location.reload();
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
    }
    function getCountAmountNoKeying(p1, p2) {
        
        $.ajax({

            type: 'GET',
            url: host + "/api/bypass/batchAccountNoKeySelect/",
            success: function (blob) {
                // $('#loading').hide();
                $scope.count = blob;
                console.log("blob ", blob);
                $scope.$apply();
            },
            error: function (a, b, c) {
                console.log("Nim in ajax erroor ", a);
               // $('#loading').hide();
            }

        });

    }

    

}]);