using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectRealEstate.GoogleCalendarAPI
{
    public class CalendarEventList
    {
        private List<CalendarEvent> items;
        public int Count
        {
            get
            {
                return items.Count;
            }
        }
        public CalendarEventList()
        {
            items = new List<CalendarEvent>();
        }
        public List<CalendarEvent> GetItems()
        {
            return items;
        }
        public void SetItems(List<CalendarEvent> items)
        {
            this.items = items;
        }
        public void Add(CalendarEvent item)
        {
            items.Add(item);
        }
        public bool Contains(CalendarEvent item)
        {
            string partialID = item.GetID().Substring(35);
            int index = items.FindIndex(i => i.GetID().Substring(35) == partialID);
            if (index > -1)
                return true;
            return false;
        }
        public void Remove(CalendarEvent item)
        {
            string partialID = item.GetID().Substring(35);
            int index = items.FindIndex(i => i.GetID().Substring(35) == partialID);
            items.RemoveAt(index);
        }
        public CalendarEvent this[int i]
        {
            get
            {
                return items[i];
            }
            set
            {
                items[i] = value;
            }
        }
    }
}
