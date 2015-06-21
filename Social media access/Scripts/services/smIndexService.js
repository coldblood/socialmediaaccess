smApp.service('smIndexApi', ['$http', function ($http) {
    var success = function (response) {
        return response.data;
    }
    var fail = function (response) {
        return response;
    }
    var getAddedAccountsUrl = '/Twitter/GetAddedccounts';

    /* Get added accounts of logged in user */
    this.getAddedAccounts = function () {
        return $http.get(getAddedAccountsUrl).then(success, fail);
    }

    

}]);