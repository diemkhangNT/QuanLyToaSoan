using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tourist_VietripInsum_2023.Models;

namespace Tourist_VietripInsum_2023.App_Start
{
    public class AdminAuthorize: AuthorizeAttribute
    {
        public string maCV { set; get; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            NhanVien nvSession = (NhanVien)HttpContext.Current.Session["user"];

            if(nvSession!=null)
            {
                QuanLyToaSoanEntities db = new QuanLyToaSoanEntities();

                var count = db.NhanViens.Count(m => m.MaNV == nvSession.MaNV && m.MaCV == maCV);

                if (count != 0)
                {
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "LoginStaff", action = "Login" }));
                }

                base.OnAuthorization(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult
                    (new RouteValueDictionary
                    (new { controller = "LoginStaff", action = "Login" }));
            }
            return;
            
        }
    }
}