using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alanya.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Chat()
        {
            ViewBag.Message = "Chat Page";

            return View();
        }
        EngageService LocalEngageService = null;
        public JsonResult doAuth(string token)
        {

            string teyid = null;
            if (!String.IsNullOrEmpty(token))
            {
                string TokenValue = token;                                         //Get token returned from Engage
                string ApiKey = "0123536dbff8e0801df6f3bb28b513794090b04d";               //Unique Engage API Key
                string EngageAppURL = "https://jbtest.rpxnow.com/openid/v2/signin?token_url=";  //The Engage Application URL that you get from the Sign in Setup
                string EngageTokenURL = "http%3A%2F%2Flocalhost%2Fengagedotnet%2Fdefault.aspx";   //The URL Encoded token URL that you get from the Sign in Setup
                bool ShowContactsFlag = true;                                                    //To show the contacts for this user based on the network that they logged in with
                bool SendStatusUpdate = false;                                                    //To show an API level social publish using default "canned" data
                string ContactProviders = "Facebook,Google,LinkedIn";                               //This is the list of providers that you can get contacts from

                //EngageSignInLink.HRef = EngageAppURL + EngageTokenURL;                              //Full signin link to start the sign in process

                //For this starter, the Default.aspx page is the initiating page AND the token URL
                //So, we must check to see if there's a token that's being posted or not
                if (TokenValue != null)
                {
                    LocalEngageService = new EngageService(ApiKey);




                    //Get the user profile based on the returned token.            
                    EngageUser User = GetUserData(TokenValue);
                    //Show the user data
                    if (User != null)
                        //ShowUserData(User);

                        #region Social Publishing API
                        //Make an API level wall update.  Use with Facebook as best example!
                        //NOTE: This uses a pre-formatted JSON object pulled from documentation on rpxnow.com/docs#api_activity.  Clearly you would want to format
                        //this according to REAL data.
                        /*
                            {
                                "user_generated_content": "I thought you would appreciate my review.",
                                "title": "A Critique of Atomic Pizza",
                                "action_links": [
                                {
                                    "href": "http:\/\/example.com\/review\/write",
                                    "text": "Write a review"
                                }
                                ],
                                "action": "wrote a review of Atomic Pizza",
                                "url": "http:\/\/example.com\/reviews\/12345\/",
                                "media": [
                                {
                                    "href": "http:\/\/bit.ly\/3fkBwe",
                                    "src": "http:\/\/bit.ly\/1nmIX9",
                                    "type": "image"
                                }
                                ],
                                "description": "Atomic Pizza has a great atmosphere and great prices.",
                                "properties": {
                                "Location": {
                                    "href": "http:\/\/bit.ly\/3fkBwe",
                                    "text": "North Portland"
                                },
                                "Rating": "5 Stars"
                                }
                            }
                        */
                        #endregion

                        if (SendStatusUpdate)
                        {
                            string StatusMessage = "{\"user_generated_content\": \"I thought you would appreciate my review.\",\"title\": \"A Critique of Atomic Pizza\",\"action_links\": [{\"href\": \"http:\\/\\/example.com\\/review\\/write\",\"text\": \"Write a review\"}],\"action\": \"wrote a review of Atomic Pizza\",\"url\": \"http:\\/\\/example.com\\/reviews\\/12345\\/\",\"media\": [{\"href\": \"http:\\/\\/bit.ly\\/3fkBwe\",\"src\": \"http:\\/\\/bit.ly\\/1nmIX9\",\"type\": \"image\"}],\"description\": \"Atomic Pizza has a great atmosphere and great prices.\",\"properties\": {\"Location\": {\"href\": \"http:\\/\\/bit.ly\\/3fkBwe\",\"text\": \"North Portland\"},\"Rating\": \"5 Stars\"}}";
                            LocalEngageService.SetActivity(User.Identifier, StatusMessage);
                        }


                    //Get Contacts for the user based on page setup
                    if (ShowContactsFlag)
                    {
                        //If the provider in the list of providers matches the provider that the user logged in with then
                        //get the contacts for that provider
                        string[] providerList = ContactProviders.Split(',');
                        foreach (string specificProvider in providerList)
                        {
                            if (User.Provider == specificProvider)
                            {
                                //ShowContacts(GetUserContacts(User));
                                break;
                            }

                        }
                    }
                }
            }
            return new JsonResult
            {
                Data = teyid,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };


        }
        #region public pagelevel methods

        /// <summary>
        /// Create a service object and return the user data
        /// </summary>
        /// <param name="APIKey">String value representing Engage API Key</param>
        /// <param name="EngageToken">Sring value representing returned Engage Token</param>
        /// <returns>EngageUser object</returns>
        public EngageUser GetUserData(string EngageToken)
        {
            return LocalEngageService.AuthInfo(EngageToken);
        }


        #endregion

    }
}