using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarAPI
{
    public class CalendarEvent
    {
        private string id;
        private string summary;
        private string description;
        private GoogleDateTime start;
        private GoogleDateTime end;
        private int sequence;
        private string location;

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
        public string GetDescription()
        {
            return description;
        }
        public void SetDescription(string description)
        {
            this.description = description;
        }
        public GoogleDateTime GetStartTime()
        {
            return start;
        }
        public void SetStartTime(GoogleDateTime start)
        {
            this.start = start;
        }
        public GoogleDateTime GetEndTime()
        {
            return end;
        }
        public void SetEndTime(GoogleDateTime end)
        {
            this.end = end;
        }
        public int GetSequence()
        {
            return sequence;
        }
        public void SetSequence(int sequence)
        {
            this.sequence = sequence;
        }
        public string GetLocation()
        {
            return location;
        }
        public void SetLocation(string location)
        {
            this.location = location;
        }
    }
}
