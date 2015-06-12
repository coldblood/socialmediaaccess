using OAuthTwitterWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social_media_access.Controllers
{
    public class TwitterTimelineSettings : ITimeLineSettings
    {
        private int _Count = 5;
        private string _IncludeRts ="1";
        private string _ScreenName ="suryaklsv1";
        private string _TimelineFormat = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&amp;include_rts={1}&amp;count={2}";
        private string _TimelineUrl = "";
        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                _Count = value;
            }
        }

        public string IncludeRts
        {
            get
            {
                return _IncludeRts;
            }
            set
            {
                _IncludeRts = value;
            }
        }

        public string ScreenName
        {
            get
            {
                return _ScreenName;
            }
            set
            {
                _ScreenName = value;
            }
        }

        public string TimelineFormat
        {
            get
            {
                return _TimelineFormat;
            }
            set
            {
                _TimelineFormat = value;
            }
        }

        public string TimelineUrl
        {
            get { return string.Format(TimelineFormat, ScreenName, IncludeRts, Count); }
        }
    }
}