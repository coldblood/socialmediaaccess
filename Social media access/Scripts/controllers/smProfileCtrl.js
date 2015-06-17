smApp.controller('smProfileCtrl', ['$scope', 'smProfileApi', function ($scope, smProfileApi) {
    var user = userInfo;
    var lastTweetId = LastTweetId;
    $scope.newPost = {};
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
    
    $scope.loadMoreTweets = function () {
        var payload = angular.extend({ id: lastTweetId }, user);
        $scope.showLoader = true;
        smProfileApi.loadMoreTweet(payload).then(function (data) {
            lastTweetId = data[data.length - 1] && (data[data.length - 1]).Id;
            $scope.tweets = $scope.tweets.concat(data);
            $scope.showLoader = false;
        }, function (error) {
            console.log(error);
            $scope.showLoader = false;
        });
    }
    $scope.updateStatus = function (status) {
        var payload = angular.extend({ statusText: status }, user);
        $scope.disableStatusBox = true;
        smProfileApi.updateStatus(payload).then(function (data) {
            $scope.newPost.status = '';
            $scope.disableStatusBox = false;
        }, function (error) {
            console.log('Unable to post currently');
        });
    }
    
}])