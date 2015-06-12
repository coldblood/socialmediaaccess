using OAuthTwitterWrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Social_media_access.Models
{
    public class TwitterSecretData : IAuthenticateSettings
    {
        public string _OAuthAccessToken;

        public string _OAuthAccessTokenSecret;

        public string _OAuthConsumerKey;

        public string _OAuthConsumerSecret;

        public string _OAuthUrl = "https://api.twitter.com/oauth2/token";

        string IAuthenticateSettings.OAuthConsumerKey
        {
            get
            {
                return _OAuthConsumerKey;
            }
            set
            {
                _OAuthConsumerKey = value;
            }
        }

        string IAuthenticateSettings.OAuthConsumerSecret
        {
            get
            {
                return _OAuthConsumerSecret;
            }
            set
            {
                _OAuthConsumerSecret = value;
            }
        }

        string IAuthenticateSettings.OAuthUrl
        {
            get
            {
                return _OAuthUrl;
            }
            set
            {
                _OAuthUrl = value;
            }
        }

        public TwitterSecretData(string userName,string nick) {
            var context = new SocialMediaEntities();
            var twitterSecret = context.TwitterSecrets.Where(x => x.UserId == userName && x.Nick == nick).FirstOrDefault();
            this._OAuthAccessToken = twitterSecret.AccessToken;
            this._OAuthAccessTokenSecret = twitterSecret.AccessTokenSecret;
            this._OAuthConsumerKey = twitterSecret.ConsumerKey;
            this._OAuthConsumerSecret = twitterSecret.ConsumerSecret;
        }
    }
}