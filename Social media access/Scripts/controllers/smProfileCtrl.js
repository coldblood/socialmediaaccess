smApp.controller('smProfileCtrl', ['$scope', 'smProfileApi', function ($scope, smProfileApi) {
    var user = userInfo;
    console.log(user);
    smProfileApi.getHomeTweets(user).then(function (data) {
        $scope.tweets = data;
    }, function () {
    });
    $scope.retweet = function (tweetId) {
        var payload = angular.extend({id: tweetId},user)
        smProfileApi.reTweet(payload).then(function () {
            console.log('success');
        }, function () { });
    }

    $scope.reply = function (tweet) {
        var payload = {
            statusId: tweet.Id,
            user: user.userName,
            screenName: tweet.Author.ScreenName,
            nick: user.Nick,
            text: tweet.newComment
        };
        smProfileApi.reply(payload).then(function () {
            console.log('success');
        }, function () { });
    }

    $scope.deleteTweet = function (tweetId) {
        var payload = angular.extend({ id: tweetId }, user);
        smProfileApi.deleteTweet(payload).then(function () {
            console.log('success');
        }, function () { });

    }
    
}])