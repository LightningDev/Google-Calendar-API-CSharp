using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarAPI
{
    /// <summary>
    /// The Json setting for serializing/deserializing objects which contains private attributes
    /// Inherited the DefaulContractResolver class
    /// </summary>
    public class PrivateReader : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Select(p => base.CreateProperty(p, memberSerialization))
                        .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                   .Select(f => base.CreateProperty(f, memberSerialization)))
                        .ToList();
            props.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props;
        }
    }

    /// <summary>
    /// The API wrapper class for using the Google Calendar API
    /// </summary>
    public class APIWrapper
    {
        public static string CLIENT_ID = "";
        public static string CLIENT_SECRET = "";
        public static string REDIRECT_URI = "";
        public string refreshToken;
        public string accessToken;
        private JsonSerializerSettings setting;

        public APIWrapper()
        {
            setting = new JsonSerializerSettings() { ContractResolver = new PrivateReader() };
        }

        public APIWrapper(string refreshToken)
        {
            setting = new JsonSerializerSettings() { ContractResolver = new PrivateReader() };
            this.refreshToken = refreshToken;
            this.accessToken = GetAccessToken(refreshToken);            
        }

        /// <summary>
        /// Get the URL for authenticating and authorizing the permission
        /// </summary>
        /// <returns>The string of URL</returns>
        public static string GetAuthenticateUrl(string emailAddress)
        {
            string authenticateUrl = "https://accounts.google.com/o/oauth2/auth?";

            // The set of query string parameters supported by the Google Authorization Server
            // Insert the new parameter if needed (following the default parameters below)
            string scope = "scope=" + "https://www.googleapis.com/auth/calendar";
            string redirectUri = "redirect_uri=" + REDIRECT_URI;
            string accessType = "access_type=" + "offline";
            string responseType = "response_type=" + "code";
            string clientID = "client_id=" + CLIENT_ID;
            string approvalPrompt = "approval_prompt=" + "force";
            string loginHint = "login_hint=" + emailAddress;

            string finalAuthUrl = authenticateUrl + scope + "&" + responseType + "&"
                + clientID + "&" + accessType + "&" + approvalPrompt + "&" + redirectUri + "&" + loginHint;
            return finalAuthUrl;
        }

        /// <summary>
        /// Get the Refresh token from Authorization code
        /// Refresh token is used for getting the new Access token (usually expired after 1 hour)
        /// </summary>
        /// <param name="authorizedCode">The authorization code from user</param>
        /// <returns></returns>
        public string GetRefreshToken(string authorizedCode)
        {
            string url = "https://accounts.google.com/o/oauth2/token";
            NameValueCollection myNameValueCollection = new NameValueCollection();
            myNameValueCollection.Add("code", authorizedCode);
            myNameValueCollection.Add("client_id", CLIENT_ID);
            myNameValueCollection.Add("client_secret", CLIENT_SECRET);
            myNameValueCollection.Add("redirect_uri", REDIRECT_URI);
            myNameValueCollection.Add("grant_type", "authorization_code");

            string result = WebUtils.PostResult(url, myNameValueCollection);
            GoogleToken token = JsonConvert.DeserializeObject<GoogleToken>(result, setting);
            return token.GetRefreshToken();
        }

        public GoogleToken GetRefreshTokenObject(string authorizedCode)
        {
            string url = "https://accounts.google.com/o/oauth2/token";
            NameValueCollection myNameValueCollection = new NameValueCollection();
            myNameValueCollection.Add("code", authorizedCode);
            myNameValueCollection.Add("client_id", CLIENT_ID);
            myNameValueCollection.Add("client_secret", CLIENT_SECRET);
            myNameValueCollection.Add("redirect_uri", REDIRECT_URI);
            myNameValueCollection.Add("grant_type", "authorization_code");

            string result = WebUtils.PostResult(url, myNameValueCollection);
            GoogleToken token = JsonConvert.DeserializeObject<GoogleToken>(result, setting);
            return token;
        }

        /// <summary>
        /// Get the Access token from Refresh token
        /// Access token is used to access/edit the user calendar information
        /// </summary>
        /// <param name="refreshToken">The Refresh token</param>
        /// <returns></returns>
        public string GetAccessToken(string refreshToken)
        {
            string url = "https://accounts.google.com/o/oauth2/token";
            NameValueCollection myNameValueCollection = new NameValueCollection();
            myNameValueCollection.Add("refresh_token", refreshToken);
            myNameValueCollection.Add("client_id", CLIENT_ID);
            myNameValueCollection.Add("client_secret", CLIENT_SECRET);
            myNameValueCollection.Add("grant_type", "refresh_token");

            string result = WebUtils.PostResult(url, myNameValueCollection);
            GoogleToken token = JsonConvert.DeserializeObject<GoogleToken>(result, setting);
            return token.GetAccessToken();
        }

        public GoogleToken GetAccessTokenObject(string refreshToken)
        {
            string url = "https://accounts.google.com/o/oauth2/token";
            NameValueCollection myNameValueCollection = new NameValueCollection();
            myNameValueCollection.Add("refresh_token", refreshToken);
            myNameValueCollection.Add("client_id", CLIENT_ID);
            myNameValueCollection.Add("client_secret", CLIENT_SECRET);
            myNameValueCollection.Add("grant_type", "refresh_token");

            string result = WebUtils.PostResult(url, myNameValueCollection);
            GoogleToken token = JsonConvert.DeserializeObject<GoogleToken>(result, setting);
            return token;
        }

        /// <summary>
        /// Get the calendar list from user Google Calendar
        /// </summary>
        /// <returns>List of Google Calendar (using GoogleCalendar class)</returns>
        public List<GoogleCalendar> GetCalendarList()
        {
            string url = "https://www.googleapis.com/calendar/v3/users/me/calendarList";
            url += "?fields=items(id, summary, timeZone)";

            string result = WebUtils.GetResult(url, accessToken);
            GoogleCalendarList calendarList = JsonConvert.DeserializeObject<GoogleCalendarList>(result, setting);
            return calendarList.GetItems();
        }

        /// <summary>
        /// Insert the new calendar into user Google Calendar
        /// </summary>
        /// <param name="calendarName">The calendar name</param>
        /// <returns>New GoogleCalendar object</returns>
        public GoogleCalendar InsertCalendar(string calendarName)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars";

            // Create a new calendar with description
            GoogleCalendar newCalendar = new GoogleCalendar();
            newCalendar.SetSummary(calendarName);

            // Convert calendar into json format (serialize)
            string calendarJson = JsonConvert.SerializeObject(newCalendar, setting);

            // Upload Json and get the response message
            string result = WebUtils.PostResult(url, accessToken, calendarJson);

            // Convert Json into GoogleCalendar object
            newCalendar = JsonConvert.DeserializeObject<GoogleCalendar>(result);

            return newCalendar;
        }

        /// <summary>
        /// Update the current calendar
        /// </summary>
        /// <param name="calendarID">The calendar ID from user Google Calendar</param>
        /// <param name="newName">New name for the calendar</param>
        /// <returns></returns>
        public GoogleCalendar UpdateCalendar(string calendarID, string newName)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/" + calendarID;

            // Create a new calendar with description
            GoogleCalendar newCalendar = new GoogleCalendar();
            newCalendar.SetSummary(newName);

            // Convert calendar into json format (serialize)
            string calendarJson = JsonConvert.SerializeObject(newCalendar, setting);

            // Upload Json and get the response message
            string result = WebUtils.PutResult(url, accessToken, calendarJson);

            // Convert Json into GoogleCalendar object
            newCalendar = JsonConvert.DeserializeObject<GoogleCalendar>(result, setting);

            return newCalendar;
        }

        /// <summary>
        /// Get the list of events in Google Calendar
        /// </summary>
        /// <param name="calendarID">The calendar ID</param>
        /// <returns>The list of GoogleEvent objects</returns>
        public List<CalendarEvent> GetEventList(string calendarID)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/" + calendarID + "/events";
            url += "?fields=items(id, summary, description, start/dateTime, end/dateTime, sequence, location)";

            string result = WebUtils.GetResult(url, accessToken);
            CalendarEventList eventList = JsonConvert.DeserializeObject<CalendarEventList>(result, setting);
            return eventList.GetItems();
        }

        /// <summary>
        /// Get the list of events in Google Calendar
        /// </summary>
        /// <param name="calendarID">The calendar ID</param>
        /// <returns>The CalendarEventList object</returns>
        public CalendarEventList GetEventListObject(string calendarID)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/" + calendarID + "/events";
            url += "?fields=items(id, summary, description, start/dateTime, end/dateTime, sequence, location)";

            string result = WebUtils.GetResult(url, accessToken);
            CalendarEventList eventList = JsonConvert.DeserializeObject<CalendarEventList>(result, setting);
            return eventList;
        }

        /// <summary>
        /// Get the specific event in Google Calendar
        /// </summary>
        /// <param name="calendarID">The calendar ID</param>
        /// <param name="eventID">The event ID</param>
        /// <returns>GoogleEvent object</returns>
        public CalendarEvent GetEvent(string calendarID, string eventID)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/" + calendarID + "/events/" + eventID;

            string result = WebUtils.GetResult(url, accessToken);
            CalendarEvent evt = JsonConvert.DeserializeObject<CalendarEvent>(result, setting);
            return evt;
        }

        /// <summary>
        /// Insert new event into the current calendar
        /// </summary>
        /// <param name="calendarID">The calendar ID</param>
        /// <param name="summary">Title of the event</param>
        /// <param name="startDate">Start time of the event</param>
        /// <param name="endDate">End time of the event</param>
        /// <returns>New GoogleEvent object</returns>
        public CalendarEvent InsertEvent(string calendarID, string eventID, string iCalUID, string summary, string description, string location, DateTime startDate, DateTime endDate)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/" + calendarID + "/events";

            // Create a new event with summary, start time and end time
            CalendarEvent evt = new CalendarEvent();
            //description = JsonConvert.ToString(description);
            //description = description.Substring(1, description.Length - 2);
            evt.SetSummary(summary);
            evt.SetDescription(description);
            GoogleDateTime start = new GoogleDateTime(startDate);
            GoogleDateTime end = new GoogleDateTime(endDate);
            evt.SetStartTime(start);
            evt.SetEndTime(end);
            evt.SetLocation(location);
            //evt.SetID(eventID);
            //evt.SetICalUID(iCalUID);

            // Convert event into json
            string eventJson = JsonConvert.SerializeObject(evt, setting);

            // Send to server and get response message
            string result = WebUtils.PostResult(url, accessToken, eventJson);

            // Convert response message(Json format) into Google Event
            evt = JsonConvert.DeserializeObject<CalendarEvent>(result, setting);

            return evt;
        }

        public CalendarEvent InsertEvent(string calendarID, CalendarEvent evt)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/" + calendarID + "/events";

            // Convert event into json
            string eventJson = JsonConvert.SerializeObject(evt, setting);

            // Send to server and get response message
            string result = WebUtils.PostResult(url, accessToken, eventJson);

            // Convert response message(Json format) into Google Event
            evt = JsonConvert.DeserializeObject<CalendarEvent>(result, setting);

            return evt;
        }

        /// <summary>
        /// Update the current event in calendar
        /// </summary>
        /// <param name="calendarID">The calendar ID</param>
        /// <param name="eventID">The event ID</param>
        /// <param name="summary">Title of calendar</param>
        /// <param name="startDate">Start time of the event</param>
        /// <param name="endDate">End time of the event</param>
        /// <returns></returns>
        public CalendarEvent UpdateEvent(string calendarID, string eventID, string description, string summary,string location, DateTime startDate, DateTime endDate)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/" + calendarID + "/events/" + eventID;

            // Get the Google event
            CalendarEvent evt = GetEvent(calendarID, eventID);
            evt.SetSummary(summary);
            evt.SetDescription(description);
            GoogleDateTime start = new GoogleDateTime(startDate);
            GoogleDateTime end = new GoogleDateTime(endDate);
            evt.SetStartTime(start);
            evt.SetEndTime(end);
            evt.SetLocation(location);

            // Increase sequence to 1 for updating
            evt.SetSequence(evt.GetSequence() + 1);

            // Convert event into json
            string eventJson = JsonConvert.SerializeObject(evt, setting);

            // Send request to Google Server
            string result = WebUtils.PutResult(url, accessToken, eventJson);

            // Convert response message(Json format) into Google Event
            evt = JsonConvert.DeserializeObject<CalendarEvent>(result, setting);

            return evt;
        }

        public CalendarEvent UpdateEvent(string calendarID, CalendarEvent evt)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/" + calendarID + "/events/" + evt.GetID();

            // Increase sequence to 1 for updating
            evt.SetSequence(evt.GetSequence() + 1);

            // Convert event into json
            string eventJson = JsonConvert.SerializeObject(evt, setting);

            // Send request to Google Server
            string result = WebUtils.PutResult(url, accessToken, eventJson);

            // Convert response message(Json format) into Google Event
            evt = JsonConvert.DeserializeObject<CalendarEvent>(result, setting);

            return evt;
        }

        /// <summary>
        /// Delete the event
        /// </summary>
        /// <param name="calendarID"></param>
        /// <param name="eventID"></param>
        public void DeleteEvent(string calendarID, string eventID)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/" + calendarID + "/events/" + eventID;
            
            WebUtils.DeleteResult(url, accessToken);
        }
    }
}
