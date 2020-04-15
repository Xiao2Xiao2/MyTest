using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Model;
using DHelper.Dapper;
using MyTest.Extensions;
using MyTest.Filters;
namespace MyTest.Controllers
{
    /// <summary>
    /// 跳过授权过滤器
    /// </summary>
    [AllowAnonymous]
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View(); 
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxLogin(string UserName, string PassWord)
        {
            AjaxResult json = new AjaxResult();
            PassWord = PassWord.ToMD5();

            Sys_UserAccount SystemUser = DapperCommand.SelectSingle<Sys_UserAccount>(new { UserName, PassWord }, " UserName=@UserName and PassWord=@PassWord");
           
            if (SystemUser == null)
            {
                json.Message = "- 用户名或密码不正确！";
                json.Code = 1;
                json.Result = false;
            }
            else if (SystemUser.Status == 1)
            {
                json.Message = "- 账户已锁定，请与管理员联系！";
                json.Code = 1;
                json.Result = false;
            }
            else
            {

                FormsAuthen(SystemUser, SystemUser.UserName);
                SystemUser.LastTime = DateTime.Now;
                SystemUser.Update();
                json.Message = "登录成功！";
                json.Code = 0;
                json.Result = true;
                try
                {
                    UserPermisstionsOperate userPermisstionsOperate  = new UserPermisstionsOperate(SystemUser.GUID);

                    userPermisstionsOperate.StoragePermissions();
                    if (SystemUser.IsAdmin!=1)
                    {
                        //判断是否有权限
                        if (userPermisstionsOperate.HasRightList().Count == 0)
                        {
                            json.Message = "此账号未分配权限！请与管理员联系!";
                            json.Code = 1;
                            json.Result = false;
                        }
                    }
                }
                catch (Exception)
                {

                    json.Message = "此账号未分配权限！请与管理员联系!";
                    json.Code = 1;
                    json.Result = false;
                }
            }
            return Content(json.ToJson());

        }

        public void FormsAuthen(Sys_UserAccount model, string UserName)
        {
            bool createPersistentCookie = false;

            //将票据写入到验证中去，表明已经登录
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(UserName, createPersistentCookie);
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(ticket.Version,
                ticket.Name,
                ticket.IssueDate,
                ticket.Expiration,
                ticket.IsPersistent,
                JsonConvert.SerializeObject(model)/*userId*/);
            authCookie.Value = FormsAuthentication.Encrypt(newTicket);
            Response.Cookies.Add(authCookie);
        }

      
    }
   
}
