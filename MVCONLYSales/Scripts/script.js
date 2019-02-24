/// <reference path="angular.min.js" />
var myApp = angular
    .module("myModule", [])
    .controller("myController", function ($scope) {
        $scope.myPhoto = "/Images/rupesh.jpg";
        $scope.message = "I am Rupesh Sah";
        var employee = [{ firstName: "Ritesh", lastName: "Sah", age: 40, gender: "Male", image: "/Images/Ritesh.jpg" },
            { firstName: "Rikit", lastName: "Sah", age: 12, gender: "Male", image: "/Images/Rishita.jpg" },
            { firstName: "Rubi", lastName: "Sah", age: 35, gender: "Female", image: "/Images/babhi.jpg" },
            { firstName: "Rishita", lastName: "Sah", age: 4, gender: "Female", image: "/Images/bunu.jpg" }
        ];
        $scope.employee = employee;
    });
