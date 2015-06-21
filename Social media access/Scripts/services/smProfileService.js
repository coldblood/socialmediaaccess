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
        updateStatusUrl = "/Twitter/UpdateStatus",
        getTweetsUrl = "/Twitter/GetTweets",
        getHomeTweetsUrl = "/Twitter/GetHomeTweets",
        getBasicInfoUrl = "/Twitter/GetBasicInfo",
        followUrl = "/Twitter/Follow",
        unfollowUrl = "/Twitter/UnFollow",
        searchUrl = "/Twitter/Search",
        getMentionsUrl = "/Twitter/GetMentions",
        loadMoreProfileTweetsUrl = "/Twitter/GetTweetsPrev";

    /* Get tweets of logged in user */
    this.getHomeTweets = function (payload) {
        return $http.get(getHomeTweetsUrl, { params: payload }).then(success, fail);
    }
    this.reTweet = function (payload) {
        return $http.get(retweetUrl, { params: payload }).then(success, fail);
    }
    this.deleteTweet = function (payload) {
        return $http.get(deleteTweetUrl, { params: payload }).then(success, fail);
    }
    /* load more tweets of logged in user */
    this.loadMoreTweet = function (payload) {
        return $http.get(loadMoreTweetUrl, { params: payload }).then(success, fail);
    }
    /* Post New Tweet */
    this.updateStatus = function (payload) {
        return $http.post(updateStatusUrl, payload).then(success, fail);
    }
    /* Get basic information of the searched profile */
    this.getBasicInfo = function (payload) {
        return $http.get(getBasicInfoUrl, { params: payload }).then(success, fail);
    }
    /* Get tweets of the searched profile */
    this.getTweets = function (payload) {
        return $http.get(getTweetsUrl, { params: payload }).then(success, fail);
    }
    /* Follow searched profile */
    this.follow = function (payload) {
        return $http.get(followUrl, { params: payload }).then(success, fail);
    }
    /* Unfollow searched profile */
    this.unfollow = function (payload) {
        return $http.get(unfollowUrl, { params: payload }).then(success, fail);
    }
    /* Search in all (People, Page, Content) */
    this.search = function (payload) {
        return $http.get(searchUrl, { params: payload }).then(success, fail);
    }
    /* Get mentions of the logged in user */
    this.getMentions = function (payload) {
        return $http.get(getMentionsUrl, { params: payload }).then(success, fail);
    }
    /* load more tweets of searched profile */
    this.loadMoreProfileTweets = function (payload) {
        return $http.get(loadMoreProfileTweetsUrl, { params: payload }).then(success, fail);
    }

    

}]);