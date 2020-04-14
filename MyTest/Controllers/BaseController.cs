using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Model;
using Newtonsoft.Json;
using MyTest.Extensions;
namespace MyTest.Controllers
{
    public class BaseController : Controller
    {
        private Sys_UserAccount _loginuser = new Sys_UserAccount();
        public Sys_UserAccount LoginUser
        {
            get
            {
                return _loginuser;
            }
        }
        public UserPermisstionsOperate userPermisstionsOperate { get; set; }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.Name != "")
            {
                FormsIdentity formsi = (FormsIdentity)filterContext.HttpContext.User.Identity;
                CustomIdentity cusIden = new CustomIdentity(formsi);
                if (cusIden != null)
                {

                    _loginuser = cusIden.User;

                    userPermisstionsOperate = new UserPermisstionsOperate(_loginuser.GUID);
                }
            }
            base.OnAuthorization(filterContext);
        }
    }
    public class CustomIdentity : System.Security.Principal.IIdentity
    {
        private FormsIdentity _identity;
        public CustomIdentity()
        {

        }
        public CustomIdentity(FormsIdentity identity)
        {
            _identity = identity;
        }

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return _identity.AuthenticationType; }
        }

        public bool IsAuthenticated
        {
            get { return _identity.IsAuthenticated; }
        }

        public string Name
        {
            get { return _identity.Name; }
        }

        #endregion

        public Sys_UserAccount User
        {
            get
            {
                return JsonConvert.DeserializeObject<Sys_UserAccount>(_identity.Ticket.UserData); ;
            }
        }
    }
}
