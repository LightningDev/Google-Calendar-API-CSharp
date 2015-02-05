using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectRealEstate.GoogleCalendarAPI
{
    public class GoogleCalendarList
    {
        private List<GoogleCalendar> items;   
   
        public GoogleCalendarList()
        {
            items = new List<GoogleCalendar>();
        }
        public List<GoogleCalendar> GetItems()
        {
            return items;
        }
        public void SetItems(List<GoogleCalendar> items)
        {
            this.items = items;
        }
    }
}
