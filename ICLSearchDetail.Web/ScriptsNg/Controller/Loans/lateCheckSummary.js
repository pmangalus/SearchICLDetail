
app.controller('lateCheckSummary', ['$scope', '$timeout', function ($scope, $timeout) {


    console.log("#1 lateCheckSummary controller JS");
    var host = 'http://10.200.1.39:9861';
    //var host = 'http://localhost:53325';

    var apiUrl = host + '/api/cics/LLateCheckSummary/';
    var that = this;
    //This will hide the DIV by default.
    $scope.btnGoModal = function () {
        console.log('nakapasok na');
        var result
        if (document.getElementById('radioSearchBatch').checked) {

            result = document.getElementById('selectResult').value;
            

        }

    }

}]);
