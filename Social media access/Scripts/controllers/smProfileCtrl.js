smApp.controller('smProfileCtrl', ['$scope', 'smProfileApi', function ($scope, smProfileApi) {
    var user = userInfo;
    var lastTweetId = LastTweetId;
    var lastProfileTweetId;
    $scope.newPost = {};
    $scope.myProfile = true;
     
    /* get tweets and mentions of logged in user*/
    var onInit = function () {
        $scope.showBackDrop = true;
        smProfileApi.getHomeTweets(user).then(function (data) {
            $scope.tweets = data;
            $scope.showBackDrop = false;
        }, function () {
            $scope.showBackDrop = false;
        });

        smProfileApi.getMentions(user).then(function (data) {
            $scope.mentions = data;
        }, function () {
            console.log('Unable to complete the request');
        });
    }
    onInit();
    

    $scope.retweet = function (tweetId) {
        var payload = angular.extend({id: tweetId},user)
        smProfileApi.reTweet(payload).then(function () {
            console.log('success');
        }, function () { });
    }

    /* comment on tweets */
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
    /* delete tweets */
    $scope.deleteTweet = function (tweetId) {
        var payload = angular.extend({ id: tweetId }, user);

        smProfileApi.deleteTweet(payload).then(function () {
            console.log('success');
        }, function () { });

    }
    
    /* load more logged in user tweets */
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
    /* post new tweet */
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

    /* utility method */
    function isEmptyObject(obj) {
        var key;
        for (key in obj) {
            return false;
        }
        return true;
    }

    /* get tweets of searched profile*/
    $scope.getTweets = function (screenName) {
        $scope.showBackDrop = true;
        var payload = angular.extend({ screenName: screenName }, user);

        smProfileApi.getBasicInfo(payload).then(function (screenInfo) {
            if (!isEmptyObject(screenInfo)) {
                $scope.myProfile = false;
                $scope.profile = screenInfo;
                smProfileApi.getTweets(payload).then(function (data) {
                    lastProfileTweetId = data[data.length - 1] && (data[data.length - 1]).Id;
                    $scope.tweets = data;
                    $scope.hideStatusBox = true;
                    $scope.hideHeader = false;
                    $scope.showBackDrop = false;
                }, function (error) {
                    $scope.showBackDrop = false;
                    console.log('Unable to get your tweets');
                });
            }
        }, function () {
            $scope.showBackDrop = false;
            console.log('Unable to get screen information');
        });
    }
    /* Follow searched profile*/
    $scope.follow = function (screenName) {
        var payload = angular.extend({ screenName: screenName }, user);

        smProfileApi.follow(payload).then(function (screenInfo) {
            console.log('following..');
        }, function () {
            console.log('Unable to complete the request');
        });
    }
    /* Unfollow searched profile*/
    $scope.unfollow = function (screenName) {
        var payload = angular.extend({ screenName: screenName }, user);

        smProfileApi.unfollow(payload).then(function (screenInfo) {
            console.log('unfollowing..');
        }, function () {
            console.log('Unable to complete the request');
        });
    }
    /* Search in all (People, Page, Content) */
    $scope.search = function (searchStr) {
        $scope.showBackDrop = true;
        var payload = angular.extend({ term: searchStr }, user);
        $scope.searchStr = searchStr;
        smProfileApi.search(payload).then(function (data) {
            $scope.myProfile = false;
            $scope.hideHeader = true;
            $scope.hideStatusBox = true;
            $scope.tweets = data.Statuses;
            $scope.showBackDrop = false;
        }, function () {
            $scope.showBackDrop = false;
            console.log('Unable to complete the request');
        });
    }
    
    /* load more of searched profile*/
    $scope.loadMoreProfileTweets = function (screenName) {
        var payload = angular.extend({ id: lastProfileTweetId, screenName: screenName }, user);
        $scope.showLoader = true;
        smProfileApi.loadMoreProfileTweets(payload).then(function (data) {
            lastProfileTweetId = data[data.length - 1] && (data[data.length - 1]).Id;
            $scope.tweets = $scope.tweets.concat(data);
            $scope.showLoader = false;
        }, function (error) {
            console.log(error);
            $scope.showLoader = false;
        });
    }
    
}])