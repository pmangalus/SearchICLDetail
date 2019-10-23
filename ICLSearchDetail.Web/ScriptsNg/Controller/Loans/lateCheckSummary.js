var app = angular.module("lateCheckSummary", []);
app.controller('lateCheckSummary', function ($scope) {

    console.log("#1 lateCheckSummary controller JS");
    var host = 'http://10.200.1.39:9861';
    //var host = 'http://localhost:53325';

    var apiUrl = host + '/api/cics/LLateCheckSummary/';
    var that = this;
    //This will hide the DIV by default.
    $scope.IsVisible = false;
    $scope.ShowSearch = function (value) {
        //If DIV is visible it will be hidden and vice versa.

        $scope.IsVisible = value == "Y";
    };
    $scope.btnGoModal = function () {
        var result
        if (document.getElementById('radioSearchBatch').checked) {

            result = document.getElementById('selectResult').value;


        }

    }

});
