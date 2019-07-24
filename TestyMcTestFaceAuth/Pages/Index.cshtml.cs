using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using TestyMcTestFaceAuth.Models;


namespace TestyMcTestFaceAuth.Pages
{
    public class IndexModel : PageModel
    {
        private UserDBContext _dbcontext;

        public IndexModel(UserDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        public string GoogleLogin { get; set; }
        public string GoogleName { get; set; }
        public string GoogleId { get; set; }
        public string GoogleUrl { get; set; }


        public void OnGet()
        {
         //   GitHubUrl = "A";
            Setsessionifexists("client_id");
            Setsessionifexists("response_type");
            Setsessionifexists("state");
            Setsessionifexists("redirect_uri");

            if (User.Identity.IsAuthenticated)
            {
                //GitHubUrl += "B";
                GoogleName = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
                GoogleId = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

                if (GoogleId != null)
                {
                    HttpContext.Session.SetString("authuseremail", GoogleId); 
                    //GitHubUrl += "C";
                    UserAccount a = _dbcontext.UserAccounts.FirstOrDefault(aa => aa.Email == GoogleId);
                    if (a == null)
                    {
                        //GitHubUrl += "D";
                        a = new UserAccount();
                        a.Email = GoogleId;
                        a.UserName = GoogleName;
                        a.guid = (Guid.NewGuid().ToString()+Guid.NewGuid().ToString() + Guid.NewGuid().ToString()).Replace("-", "");
                        a.amazonclientid = HttpContext.Session.GetString("client_id");
                        a.amazonresponsetype = HttpContext.Session.GetString("response_type");
                        a.amazonredirecturi = HttpContext.Session.GetString("redirect_uri");
                        a.amazonstate = HttpContext.Session.GetString("state");
                        _dbcontext.UserAccounts.Add(a);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        //GitHubUrl += "E";

                        if (a.guid == null)
                        {
                            //GitHubUrl += "F";

                            a.guid = (Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString()).Replace("-", "");
                            a.amazonclientid = HttpContext.Session.GetString("client_id");
                            a.amazonresponsetype = HttpContext.Session.GetString("response_type");
                            a.amazonredirecturi = HttpContext.Session.GetString("redirect_uri");
                            a.amazonstate = HttpContext.Session.GetString("state");
                            _dbcontext.SaveChanges();
                        }
                    }
                    //GitHubUrl += "G";

                    a.amazonclientid = HttpContext.Session.GetString("client_id");
                    a.amazonresponsetype = HttpContext.Session.GetString("response_type");
                    a.amazonredirecturi = HttpContext.Session.GetString("redirect_uri");
                    a.amazonstate = HttpContext.Session.GetString("state");
                    _dbcontext.SaveChanges();


                    var redirUrl = HttpContext.Session.GetString("redirect_uri")
                        + "#state=" + HttpContext.Session.GetString("state") 
                        + "&access_token=" + a.guid + "&token_type=Bearer";
                    //GitHubUrl += "H  "+ redirUrl;

                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("redirect_uri")))
                    { 
                    Response.Redirect(redirUrl); 
                    }


                }

            }
        }


        private void Setsessionifexists(string s)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.Query[s].ToString()))
                {
                    HttpContext.Session.SetString(s, Request.Query[s].ToString());
                }
            }
            catch { }
        }



    }
}
