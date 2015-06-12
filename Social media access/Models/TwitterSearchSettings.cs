using OAuthTwitterWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social_media_access.Models
{
    public class TwitterSearchSettings : ISearchSettings
    {
        private string _SearchQuery;
        private string _SearchUrl;
        private string _SearchFormat = "https://api.twitter.com/1.1/search/tweets.json?q={0}";
        public string SearchFormat
        {
            get
            {
                return _SearchFormat;
            }
            set
            {
                _SearchFormat = value;
            }
        }

        public string SearchQuery
        {
            get
            {
                return _SearchQuery;
            }
            set
            {
                _SearchQuery = value;
            }
        }

        public string SearchUrl
        {
            get {return string.Format(SearchFormat,SearchQuery); }
        }
    }
}