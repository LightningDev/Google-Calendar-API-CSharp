using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InspectRealEstate.GoogleCalendarAPI
{
    public class GoogleDateTime
    {
        private DateTime dateTime;

        public GoogleDateTime(DateTime dateTime)
        {
            this.dateTime = dateTime.ToUniversalTime();
        }
        public DateTime GetDateTime()
        {
            return dateTime.ToUniversalTime();
        }
        public void SetDateTime(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }
    }
}
