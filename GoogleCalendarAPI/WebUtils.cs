using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarAPI
{
    public class WebUtils
    {
        public static string GetResult(string url, string accessToken)
        {
            string result = "";
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.ServicePoint.ConnectionLimit = 100;
                WebHeaderCollection headers = myHttpWebRequest.Headers;
                headers.Set("Accept-Charset", "utf-8");
                headers.Set("Authorization", "Bearer " + accessToken);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream receiveStream = myHttpWebResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                result = readStream.ReadToEnd();
                myHttpWebResponse.Close();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static string DeleteResult(string url, string accessToken)
        {
            string result = "";
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.ServicePoint.ConnectionLimit = 100;
                WebHeaderCollection headers = myHttpWebRequest.Headers;
                myHttpWebRequest.Method = "DELETE";
                headers.Set("Accept-Charset", "utf-8");
                headers.Set("Authorization", "Bearer " + accessToken);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream receiveStream = myHttpWebResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                result = readStream.ReadToEnd();
                myHttpWebResponse.Close();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static string PostResult(string url, NameValueCollection form)
        {
            string result = "";
            try
            {
                WebClient myWebClient = new WebClient();
                byte[] responseArray = myWebClient.UploadValues(url, form);
                result = Encoding.ASCII.GetString(responseArray);
            }
            catch (Exception ex)
            {
                result = ex.Message;                
            }
            return result;
        }

        public static string PostResult(string url, string accessToken, string jSon)
        {
            string result = "";
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.ServicePoint.ConnectionLimit = 100;
                WebHeaderCollection headers = myHttpWebRequest.Headers;
                myHttpWebRequest.Method = "POST";
                headers.Set("Authorization", "OAuth " + accessToken);

                // POST data
                string postData = jSon;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postData);

                // Data type
                myHttpWebRequest.ContentType = "application/json; charset=utf-8";

                // Data length
                myHttpWebRequest.ContentLength = data.Length;

                Stream newStream = myHttpWebRequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream receiveStream = myHttpWebResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                result = readStream.ReadToEnd();
                myHttpWebResponse.Close();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        public static string PutResult(string url, string accessToken, string jSon)
        {
            string result = "";
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.ServicePoint.ConnectionLimit = 100;
                WebHeaderCollection headers = myHttpWebRequest.Headers;
                myHttpWebRequest.Method = "PUT";
                headers.Set("Authorization", "OAuth " + accessToken);

                // PUT data
                string postData = jSon;
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postData);

                // Data type
                myHttpWebRequest.ContentType = "application/json; charset=utf-8";

                // Data length
                myHttpWebRequest.ContentLength = data.Length;

                Stream newStream = myHttpWebRequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream receiveStream = myHttpWebResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                result = readStream.ReadToEnd();
                myHttpWebResponse.Close();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
