using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    [Authorize(Roles = "Admin")]
   // [RequireHttps]
    public class AdminBaseController:Controller
    {
         
    }
}