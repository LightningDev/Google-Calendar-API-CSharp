# Google-Calendar-API-.NET#
Wrapper classes for using Google Calendar API

This is my wrapper classes to handle the REST method in Google Calendar API. Use this if you don't want to use Google Library for .NET. It's written in C#.

#Prepare:#
Json library for .NET ( Json.NET)

#Classes structure:#
<ul>
  <li>APIWrapper.cs: Main class for calling the Google Calendar request (Insert, Update, Delete, Get)</li>
  <li>GoogleCalendar.cs</li>
  <li>GoogleCalendarList.cs</li>
  <li>GoogleDateTime.cs</li>
  <li>GoogleEvent.cs</li>
  <li>GoogleEventList.cs</li>
  <li>.GoogleToken.cs</li>
  <li>WebUtils.cs: Web request implementations (GET, POST, PUT, DELETE)</li>
</ul>


#How to use:#
<h3>Authorizing the user permission</h3>
string authURL = APIWrapper.GetAuthenticateUrl("User gmail address");
<br/>
After this you can use any methods you want to get the aut horized code from user after they accept or cancel.
<br/>
For example:
Response.Redirect(authURL);
<br/>
After that, either use:
<br/>
string authCode = Request.QueryString["code"]; // If user accept
<br/>
string errorCode = Request.QueryString["error"]; // otherwise, they cancel it
<br/>
This is just an example, you can write ashx file in ASP.NET if you want, the implementation how to get the Authorized code depends on you.
<br/>
<h3>Get the refresh and access token</h3>
To send the request to Google, you must have the token.
<br/>
APIWrapper wrapper = new APIWrapper();
<br/>
There are 2 functions, 1 return the GoogleToken object, the other is string of token. 
<br/>
GoogleToken refreshToken = wrapper.GetRefreshTokenObject(authCode);
<br/>
string refreshToken = wrapper.GetRefreshToken(Code);
<br/>
To get the access token, you must have refresh token.
<br/>
GoogleToken accessToken = wrapper.GetAccessTokenObject(refreshToken);
<br/>
string accessToken = wrapper.GetAccessToken(accessToken);
<br/>
After this, store it into database or anywhere you want.

<h3>Get the list of calendar</h3>
<p>
Set the access token to the APIWrapper after you got it, then just simply call method GetCalendarList(), this will return the list of GoogleCalendar object. (modify it with GoogleCalendarList if you want).</p>

List<GoogleCalendar> calendarList = wrapper.GetCalendarList();

<h3>Create a new calendar</h3>
This method will return the new object GoogleCalendar after you insert it.
<br/>
GoogleCalendar calendar = wrapper.InsertCalendar("calendar name");

<h3>Update the calendar</h3>
Modify at your own if you want to update more information.
<br/>
GoogleCalendar calendar = wrapper.UpdateCalendar(calendar ID,"calendar new name");

About the event, it's all the same as calendar.
