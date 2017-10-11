using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenShift.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult LogIn()
        {
            return View();
        }

        public void Index()
        {
            Response.StatusCode = 301;
            Response.Status = "301 Moved Permanently";
            Response.AppendHeader("Location", "http://www.baidu.com");
            Response.AppendHeader("Cache-Control", "no-cache");  //这里很重要的一个设置， no-cache 表示不做本地缓存
            Response.End();

            //var s = "He{0}lo Worl{1}!";
            //var ss = string.Format("你{0}，{1}界","好","世");
            //var sss = string.Format("你好，世界", "好", "世");
            //return string.Format(s,"l","d");
        }
    }
}
