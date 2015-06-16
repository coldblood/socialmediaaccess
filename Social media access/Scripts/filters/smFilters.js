smApp.filter('toDate', ['$filter',function ($filter) {
    return function (str) {
        var timestamp = +str.substring(6, str.length - 2);
        return $filter('date')(timestamp, 'medium');
    }
}])