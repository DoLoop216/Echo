using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AR.ARWebAuthorization;

namespace Echo
{
    public class Networking
    {
        public static bool isAdmin(HttpRequest Request)
        {
            ARWebAuthorizationUser u = ARWebAuthorization.GetUser(Request.Cookies["h"]);

            if (u != null && u.LocalUserClass != null)
                if ((u.LocalUserClass as AR.ARNews.User).Type == AR.ARNews.UserType.Administrator)
                    return true;
            return false;
        }

        public static int GetID(HttpRequest Request)
        {
            ARWebAuthorizationUser u = ARWebAuthorization.GetUser(Request.Cookies["h"]);

            if (u != null && u.LocalUserClass != null)
                return (u.LocalUserClass as AR.ARNews.User).UserID;

            return -1;
        }
    }
}
