using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace Social_media_access
{

    public sealed class GetTweets : CodeActivity
    {
        public InArgument<string> Status { get; set; }
        public OutArgument<Tweet[]> tweets { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var entityContext = new SocialMediaEntities();
            var data = (from t in entityContext.Tweets where t.Status == Status.Get(context) select t).ToArray();
            tweets.Set(context, data);
        }
    }
}