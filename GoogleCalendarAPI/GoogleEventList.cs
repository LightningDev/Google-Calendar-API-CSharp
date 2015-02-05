using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarAPI
{
    public class GoogleEventList
    {
        private List<GoogleEvent> items;

        public List<GoogleEvent> GetItems()
        {
            return items;
        }
        public void SetItems(List<GoogleEvent> items)
        {
            this.items = items;
        }
    }
}
