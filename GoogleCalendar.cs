using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectRealEstate.GoogleCalendarAPI
{
    public class GoogleCalendar
    {
        private string id;
        private string summary;
        private string timeZone;

        public string GetID()
        {
            return id;
        }
        public void SetID(string id)
        {
            this.id = id;
        }
        public string GetSummary()
        {
            return summary;
        }
        public void SetSummary(string summary)
        {
            this.summary = summary;
        }
        public string GetTimeZone()
        {
            return timeZone;
        }
        public void SetTimeZone(string timeZone)
        {
            this.timeZone = timeZone;
        }
    }
}
