var app = angular.module('MyApp', ['datatables']);
app.controller('homeCtrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder',
    function ($scope, $http, DTOptionsBuilder, DTColumnBuilder) {
        $scope.dtColumns = [
            //here We will add .withOption('name','column_name') for send column name to the server 
            DTColumnBuilder.newColumn("locCode", "Customer ID").withOption('name', 'CustomerID'),
            DTColumnBuilder.newColumn("locDesc", "Company Name").withOption('name', 'CompanyName'),
            //DTColumnBuilder.newColumn("ContactName", "Contact Name").withOption('name', 'ContactName'),
            //DTColumnBuilder.newColumn("Phone", "Phone").withOption('name', 'Phone'),
            //DTColumnBuilder.newColumn("City", "City").withOption('name', 'City')
        ]

        $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
            dataSrc: "data",
            url: "/api/location/GetAllActive",
            type: "POST"
        })
        .withOption('processing', true) //for show progress bar
        .withOption('serverSide', true) // for server side processing
        .withPaginationType('full_numbers') // for get full pagination options // first / last / prev / next and page numbers
        .withDisplayLength(10) // Page size
        .withOption('aaSorting', [0, 'asc']) // for default sorting column // here 0 means first column
    }])