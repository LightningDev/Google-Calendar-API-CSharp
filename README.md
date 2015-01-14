# Google-Calendar-API-C##
Wrapper classes for using Google Calendar API

This is my wrapper classes to handle the REST method in Google Calendar API. Use this if you don't want to use Google Library for .NET. It's written in C#.

#Prepare:#
Json library for .NET ( Json.NET)

#Classes structure:#
-APIWrapper.cs: Main class for calling the Google Calendar request (Insert, Update, Delete, Get)
-GoogleCalendar.cs
-GoogleCalendarList.cs (actually not need, just make it to serialize)
-GoogleDateTime.cs (actually not need, just make it to serialize)
-GoogleEvent.cs
-GoogleEventList.cs (actually not need, just make it to serialize)
-GoogleToken.cs
-WebUtils.cs: Web request implementations (GET, POST, PUT, DELETE).

#How to use:#
1. Authorizing the user permission
string authURL = APIWrapper.GetAuthenticateUrl("User gmail address");
After this you can use any methods you want to get the aut horized code from user after they accept or cancel.

For example:
Response.Redirect(authURL);

After that, either use:
string authCode = Request.QueryString["code"]; // If user accept
string errorCode = Request.QueryString["error"]; // otherwise, they cancel it

This is just an example, you can write ashx file in ASP.NET if you want, the implementation how to get the Authorized code depends on you.

2. Get the refresh and access token
To send the request to Google, you must have the token.
APIWrapper wrapper = new APIWrapper();

There are 2 functions, 1 return the GoogleToken object, the other is string of token. 
GoogleToken refreshToken = wrapper.GetRefreshTokenObject(authCode);
string refreshToken = wrapper.GetRefreshToken(Code);

To get the access token, you must have refresh token.
GoogleToken accessToken = wrapper.GetAccessTokenObject(refreshToken);
string accessToken = wrapper.GetAccessToken(accessToken);

After this, store it into database or anywhere you want.

3. Get the list of calendar
Set the access token to the APIWrapper after you got it, then just simply call method GetCalendarList(), this will return the list of GoogleCalendar object. (modify it with GoogleCalendarList if you want).

List<GoogleCalendar> calendarList = wrapper.GetCalendarList();

4. Create a new calendar
This method will return the new object GoogleCalendar after you insert it.

GoogleCalendar calendar = wrapper.InsertCalendar("calendar name");

5. Update the calendar
Modify at your own if you want to update more information.

GoogleCalendar calendar = wrapper.UpdateCalendar(calendar ID,"calendar new name");

About the event, it's all the same as calendar.
