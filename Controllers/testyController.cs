using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.CustomerProfile;
using Alexa.NET.Reminders;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TestyMcTestfaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testyController : ControllerBase
    {


        [HttpPost("handler")]
        public async Task<SkillResponse> handler([FromBody] SkillRequest skillRequest)
        {
            mailme("Starting " + JsonConvert.SerializeObject(skillRequest));
            var requestType = skillRequest.GetRequestType();

            SkillResponse response = null;

            if (requestType == typeof(LaunchRequest))
            {
                response = ResponseBuilder.Tell("Welcome to Testy McTestface. How can we help you?");
                response.Response.ShouldEndSession = false;
                return response;
            }
            if (requestType == typeof(SessionEndedRequest))
            {
                response = ResponseBuilder.Tell("Goodbye");
                response.Response.ShouldEndSession = true;
                return response;
            }

            if (requestType == typeof(IntentRequest) || requestType == typeof(BuiltInIntent))
            {
                var intentRequest = skillRequest.Request as IntentRequest;
                if (intentRequest.Intent.Name == "AMAZON.StopIntent")
                {
                    response = ResponseBuilder.Tell("Goodbye");
                    response.Response.ShouldEndSession = true;
                    return response;
                }
                if (intentRequest.Intent.Name == "AMAZON.CancelIntent")
                {
                    response = ResponseBuilder.Tell("Goodbye");
                    response.Response.ShouldEndSession = true;
                    return response;
                }
                if (intentRequest.Intent.Name == "AMAZON.NavigateHomeIntent")
                {
                    response = ResponseBuilder.Tell("Goodbye");
                    response.Response.ShouldEndSession = true;
                    return response;
                }
                if (intentRequest.Intent.Name == "AMAZON.HelpIntent")
                {
                    response = ResponseBuilder.Tell("You can ask me to say something. How may we help you now?");
                    response.Response.ShouldEndSession = false;
                    return response;
                }

                if (intentRequest.Intent.Name == "IntentTalkToMe")
                {
                    FullAddress fullAddress = null;
                    try
                    {
                        CustomerProfileClient aclient = new CustomerProfileClient(skillRequest);
                        fullAddress = await aclient.FullAddress();
                    }
                    catch
                    {}
                    if (fullAddress == null)
                    {
                        response = ResponseBuilder.Tell("Sorry, but Testy McTestFace needs access to your address, you can allow this in the alexa app or website.");
                        response.Response.ShouldEndSession = true;
                        return response;
                    }
                    string addr = (fullAddress.AddressLine1 ?? "") + "," + (fullAddress.AddressLine2 ?? "") + "," +
                                  (fullAddress.AddressLine3 ?? "") + "," + (fullAddress.City ?? "");
                    return ResponseBuilder.Tell("Hi, I'm Testy McTestFace and your address is "+ addr);
                }


				if (intentRequest.Intent.Name == "WhatDoesSkyColourMean")
				{
                    string temp = "";
                    if (intentRequest.Intent.Slots != null)
                    {
                        foreach (KeyValuePair<string, Slot> item in intentRequest.Intent.Slots)
                        {
                            try
                            {
                                switch (item.Key)
                                {
                                    case "SkyColourSlot": temp = item.Value.Value.ToLower(); break;
                                    default: break;
                                }
                            }
                            catch { }
                        }
                    }
                    switch (temp)
                    {
                        case "blue": temp = "It means that refraction of light makes it seem so."; break;
                        case "red": temp = "If night time then shepards are happy allegedly"; break;
                        case "black": temp = "It either means it's night time or your eyes are closed"; break;
                        case "falling": temp = "It means you need to go tell chicken little quickly"; break;
                        default: temp = "I'm at a loss as to what that means."; break;
                    }
                    return ResponseBuilder.Tell(temp);

                }

                if (intentRequest.Intent.Name == "IntentSetReminder")
                {
                    try
                    {
                    var reminder = new Reminder
                    {
                        RequestTime = DateTime.UtcNow,
                        Trigger = new RelativeTrigger(12 * 60 * 60),
                        AlertInformation = new AlertInformation(new[] { new SpokenContent("Testy McTestface reminder", "en-GB") }),
                        PushNotification = PushNotification.Enabled
                    };
                    var rclient = new RemindersClient(skillRequest);
                    var alertDetail = await rclient.Create(reminder);

                    return ResponseBuilder.Tell("Reminder has been set");
                    }
                    catch (Exception e)
                    {
                        // This will normally be that the user has not authorised reminders so you need to tell them how.
                        return ResponseBuilder.Tell("Error :"+e.Message);
                    }

                }

            }


            return ResponseBuilder.Tell("Hi, something has went wrong");
        }

     
    }
}