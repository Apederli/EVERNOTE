using Myevernote.Common;
using MyEvernote.Entitie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsurname()
        {
            if (HttpContext.Current.Session["login"] !=null)
            {
                EvernoteUser user = new EvernoteUser();
                user = HttpContext.Current.Session["login"] as EvernoteUser;
                return user.Username;
            }
            return "system";
        }
    }
}