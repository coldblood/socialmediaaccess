﻿using Newtonsoft.Json;
using OAuthTwitterWrapper.JsonTypes;
using Social_media_access.Models;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TweetSharp;

namespace Social_media_access.Controllers
{
    public class TwitterController : Controller
    {
        //
        // GET: /Twitter/
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public string UserTimeline()
        //{
        //    var oAuthTwitterWrapper = new OAuthTwitterWrapper.OAuthTwitterWrapper(new TwitterSecret(), new TwitterTimelineSettings(), new TwitterSearchSettings());
        //    return oAuthTwitterWrapper.GetMyTimeline();
        //}
        [HttpGet]
        public JsonResult GetAddedccounts()
        {
            List<SelectListItem> twitterSecretsMap = new List<SelectListItem>();
            try
            {
                var context = new Social_media_access.SocialMediaEntities();
                context.TwitterSecrets.ToList().ForEach(i => twitterSecretsMap.Add(new SelectListItem()
                {
                    Text = i.Nick,
                    Value = i.Nick
                }));
                return Json(twitterSecretsMap, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Search(string userName, string term, string Nick)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.Q = term;
            return Json(service.Search(searchOptions), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStatus(string statusText, string userName, string Nick)
        {
            SocialMediaEntities SocialMediaContext = new SocialMediaEntities();
            Tweet Tweet = new Tweet();
            Tweet.Author = "suryaklsv";
            Tweet.Text = statusText;
            Tweet.Status = "Pending";
            Tweet.TimeStamp = DateTime.Now;

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "{\"processDefinitionId\":\"" + ConfigurationManager.AppSettings["processDefinitionId"] + "\",";
            postData += "\"variables\":[";
            postData += "{\"name\":\"tweet\",";
            postData += "\"value\":\"" + Tweet.Text + "\"}";
            postData += ",{\"name\":\"userName\",";
            postData += "\"value\":\""+userName+"\"}";
            postData += ",{\"name\":\"Nick\",";
            postData += "\"value\":\"" + Nick + "\"}";
            postData += "]}";
            byte[] data = encoding.GetBytes(postData);

            HttpWebRequest processInstanceReq = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ActivitiTweetProcessInstance"]);
            processInstanceReq.Method = "POST";
            processInstanceReq.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["processInsUser"], ConfigurationManager.AppSettings["processInsPswd"]);
            processInstanceReq.ContentType = "application/json";
            processInstanceReq.Accept = "application/json";
            processInstanceReq.ContentLength = data.Length;
            Stream processInstanceReqStream = processInstanceReq.GetRequestStream();

            processInstanceReqStream.Write(data, 0, data.Length);
            processInstanceReqStream.Close();

            var processInsRes = new StreamReader(processInstanceReq.GetResponse().GetResponseStream()).ReadToEnd();

            SocialMediaContext.Tweets.Add(Tweet);
            SocialMediaContext.SaveChanges();
            return Json("{}", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ProcessTweet(string status, string userName, string Nick)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var sendTweetOptions = new SendTweetOptions();
            sendTweetOptions.Status = status;
            service.SendTweet(sendTweetOptions);
        }

        [HttpGet]
        public void ProcessReply(string statusId, string userName, string screenName, string Nick, string text)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var sendTweetOptions = new SendTweetOptions();
            sendTweetOptions.Status = "@" + screenName + " " + text;
            sendTweetOptions.InReplyToStatusId = Convert.ToInt64(statusId);
            service.SendTweet(sendTweetOptions);
        }

        [HttpGet]
        public void ReplyTweet(long statusId, string userName, string screenName, string Nick, string text)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "{\"processDefinitionId\":\"" + ConfigurationManager.AppSettings["commentProcessDefinitionId"] + "\",";
            postData += "\"variables\":[";
            postData += "{\"name\":\"reply\",";
            postData += "\"value\":\"" + text + "\"}";
            postData += ",{\"name\":\"user\",";
            postData += "\"value\":\"" + userName + "\"}";
            postData += ",{\"name\":\"nick\",";
            postData += "\"value\":\"" + Nick + "\"}";
            postData += ",{\"name\":\"id\",";
            postData += "\"value\":\"" + statusId + "\"}";
            postData += ",{\"name\":\"screen\",";
            postData += "\"value\":\"" + screenName + "\"}";
            postData += "]}";
            byte[] data = encoding.GetBytes(postData);

            HttpWebRequest processInstanceReq = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ActivitiCommentProcessInstance"]);
            processInstanceReq.Method = "POST";
            processInstanceReq.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["processInsUser"], ConfigurationManager.AppSettings["processInsPswd"]);
            processInstanceReq.ContentType = "application/json";
            processInstanceReq.Accept = "application/json";
            processInstanceReq.ContentLength = data.Length;
            Stream processInstanceReqStream = processInstanceReq.GetRequestStream();

            processInstanceReqStream.Write(data, 0, data.Length);
            processInstanceReqStream.Close();

            var processInsRes = new StreamReader(processInstanceReq.GetResponse().GetResponseStream()).ReadToEnd();
        }


        [HttpGet]
        public void PostTweet(Tweet tweet)
        {
            SocialMediaEntities context = new SocialMediaEntities();
        }

        [HttpGet]
        public JsonResult GetHomeTweets(string userName, string Nick)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var tweets = service.ListTweetsOnHomeTimeline(new TweetSharp.ListTweetsOnHomeTimelineOptions { Count = 10 });
            return Json(tweets, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string Delete(string userName, string Nick, long id)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var Del = service.DeleteTweet(new DeleteTweetOptions { Id = id });
            return "Deleted successfully";
        }

        [HttpGet]
        public JsonResult GetTweets(string userName, string screenName, string Nick)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var tweets = service.ListTweetsOnUserTimeline(new TweetSharp.ListTweetsOnUserTimelineOptions { ScreenName = screenName, Count = 10 });
            return Json(tweets, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetHomeTweetsPrev(string userName, string Nick,long id)
        {
            var twitterSecret = new Social_media_access.Models.TwitterSecretData(userName, Nick);
            var service = new TweetSharp.TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var _ListTweetsOnHomeTimelineOptions = new TweetSharp.ListTweetsOnHomeTimelineOptions();
            _ListTweetsOnHomeTimelineOptions.Count = 10;
            _ListTweetsOnHomeTimelineOptions.MaxId = id;
            var tweets = service.ListTweetsOnHomeTimeline(_ListTweetsOnHomeTimelineOptions);
            return Json(tweets, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string Retweet(string userName, string Nick, long id)
        {
            var twitterSecret = new Social_media_access.Models.TwitterSecretData(userName, Nick);
            var service = new TweetSharp.TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var TwitterStatus = service.Retweet(new RetweetOptions { Id = id });
            return "Retweeted successfully";
        }

        [HttpGet]
        public JsonResult GetHomeTweetsSince(string userName, string Nick, long id)
        {
            var twitterSecret = new Social_media_access.Models.TwitterSecretData(userName, Nick);
            var service = new TweetSharp.TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var _ListTweetsOnHomeTimelineOptions = new TweetSharp.ListTweetsOnHomeTimelineOptions();
            _ListTweetsOnHomeTimelineOptions.Count = 11;
            _ListTweetsOnHomeTimelineOptions.SinceId = id;
            var tweets = service.ListTweetsOnHomeTimeline(_ListTweetsOnHomeTimelineOptions);
            return Json(tweets, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTweetsSince(string userName, string screenName, string Nick, long id)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var tweets = service.ListTweetsOnUserTimeline(new TweetSharp.ListTweetsOnUserTimelineOptions { ScreenName = screenName, Count = 10, SinceId = id });
            return Json(tweets, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTweetsPrev(string userName, string screenName, string Nick, long id)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var tweets = service.ListTweetsOnUserTimeline(new TweetSharp.ListTweetsOnUserTimelineOptions { ScreenName = screenName, Count = 10, MaxId = id });
            return Json(tweets, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMentions(string userName, string Nick)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            ListTweetsMentioningMeOptions _ListTweetsMentioningMeOptions = new ListTweetsMentioningMeOptions();
            _ListTweetsMentioningMeOptions.Count = 10;
            return Json(service.ListTweetsMentioningMe(_ListTweetsMentioningMeOptions),JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string Follow(string userName, string Nick, string screenName)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            FollowUserOptions _FollowUserOptions = new TweetSharp.FollowUserOptions();
            _FollowUserOptions.ScreenName = screenName;
            service.FollowUser(_FollowUserOptions);
            return "Followed successfully!";
        }


        [HttpGet]
        public JsonResult GetBasicInfo(string userName, string Nick, string screenName)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            var profile = service.GetUserProfileFor(new GetUserProfileForOptions { ScreenName = screenName });
            return Json(profile, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string UnFollow(string userName, string Nick, string screenName)
        {
            var twitterSecret = new TwitterSecretData(userName, Nick);
            var service = new TwitterService(twitterSecret._OAuthConsumerKey, twitterSecret._OAuthConsumerSecret);
            service.AuthenticateWith(twitterSecret._OAuthAccessToken, twitterSecret._OAuthAccessTokenSecret);
            UnfollowUserOptions _UnFollowUserOptions = new TweetSharp.UnfollowUserOptions();
            _UnFollowUserOptions.ScreenName = screenName;
            service.UnfollowUser(_UnFollowUserOptions);
            return "UnFollowed successfully!";
        }

        public ActionResult Add()
        {
            ViewBag.Message = "add a twitter account.";

            return View();
        }

        [HttpPost]
        public string AddAccount(string accessToken, string accessTokenSecret, string consumerKey, string consumerKeySecret, string userName, string Nick)
        {
            SocialMediaEntities SocialMediaContext = new SocialMediaEntities();
            TwitterSecret data = new TwitterSecret();
            data.AccessToken = accessToken;
            data.AccessTokenSecret = accessTokenSecret;
            data.ConsumerKey = consumerKey;
            data.ConsumerSecret = consumerKeySecret;
            data.UserId = userName;
            data.Nick = Nick;
            SocialMediaContext.TwitterSecrets.Add(data);
            SocialMediaContext.SaveChanges();
            return "success";
        }
    }
}
