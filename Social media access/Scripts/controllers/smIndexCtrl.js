smApp.controller('smIndexCtrl', ['$scope', '$http', 'smIndexApi', function ($scope, $http, smIndexApi) {
    var payload = { userName: 'suryanak9', nick: 'Surya' };

    var init = function init(){
        smIndexApi.getAddedAccounts().then(function (data) {
            console.log('success');
            $scope.addedAccounts = data;
        }, function (error) {
            console.log(error)
        });
    }
    init();
    $scope.goToAccount = function (nick) {
        location.href = "/Home/Profile?Nick=" + nick;
    }
}])