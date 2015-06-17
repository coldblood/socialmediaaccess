smApp.service('smProfileApi', ['$http', function ($http) {
    var success = function (response) {
        return response.data;
    }
    var fail = function (response) {
        return response;
    }
    var getTweetsUrl = '/Twitter/GetHomeTweets',
        retweetUrl = "/Twitter/Retweet",
        replyUrl = "/Twitter/ReplyTweet",
        deleteTweetUrl = "/Twitter/Delete",
        loadMoreTweetUrl = "/Twitter/GetHomeTweetsPrev",
        updateStatusUrl = "/Twitter/UpdateStatus";

    this.getHomeTweets = function (payload) {
        return $http.get(getTweetsUrl, { params: payload }).then(success, fail);
    }
    this.reTweet = function (payload) {
        return $http.get(retweetUrl, { params: payload }).then(success, fail);
    }
    this.deleteTweet = function (payload) {
        return $http.get(deleteTweetUrl, { params: payload }).then(success, fail);
    }
    this.loadMoreTweet = function (payload) {
        return $http.get(loadMoreTweetUrl, { params: payload }).then(success, fail);
    }
    this.updateStatus = function (payload) {
        return $http.post(updateStatusUrl, payload).then(success, fail);
    }

}]);