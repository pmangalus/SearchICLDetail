app.controller('iclViewSearchCtrl', ['$scope', '$timeout', function ($scope, $timeout) {
    console.log("iclViewSearchCtrl");
    var host = 'http://localhost:53325';

    var txtMobile = document.getElementById("txtICL");

    $scope.btnSearchClick = function () {
        console.log("sdfsdfsdfsfsdfsd");
        
        if (txtMobile.value.trim() != "") {
            var apiUrl = host + '/api/icl/searchIcl/' + txtMobile.value;
            var that = this;
            $.ajax({
                type: 'GET',
                url: apiUrl,
                success: function (blob) {
                    console.log("Nim in ajax erroor ", blob);
                    var jsonParse = JSON.parse(blob);
                    if (jsonParse.length != 0) {
                        $scope.display = jsonParse;
                    }
                    else {
                        that.openDialog("No result Found. Please try again.");
                    }
                    $scope.$apply();
                },
                error: function (a, b, c) {
                    console.log("Nim in ajax erroor ", a);
                }

            });
        }
    },
    $scope.focus = function () {
        console.log("focus");
    },
    $scope.openDialog = function (message) {
        BootstrapDialog.show({
            message: message,
            buttons: [{
                label: 'Close',
                action: function (dialogItself) {
                    dialogItself.close();
                }
            }]
        });
    }


}]);