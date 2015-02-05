using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectRealEstate.GoogleCalendarAPI
{
    public class GoogleToken
    {
        private string Access_token;
        private string Refresh_token;
        private string Expires_in;
        private string Token_type;

        public string GetAccessToken()
        {
            return Access_token;
        }
        public void SetAccessToken(string Access_token)
        {
            this.Access_token = Access_token;
        }
        public string GetRefreshToken()
        {
            return Refresh_token;
        }
        public void SetRefreshToken(string Refresh_token)
        {
            this.Refresh_token = Refresh_token;
        }
        public string GetExpiresIn()
        {
            return Expires_in;
        }
        public void SetExpiresIn(string Expires_in)
        {
            this.Expires_in = Expires_in;
        }
        public string GetTokenType()
        {
            return Token_type;
        }
        public void SetTokenType(string Token_type)
        {
            this.Token_type = Token_type;
        }
        public DateTime GetExpiresInDateTime()
        {
            DateTime now = DateTime.UtcNow;
            now = now.AddSeconds(Double.Parse(Expires_in));
            return now;
        }
    }
}
