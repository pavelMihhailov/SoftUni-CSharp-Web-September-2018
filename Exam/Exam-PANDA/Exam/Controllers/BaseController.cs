using PANDA.Data;
using SIS.MvcFramework;

namespace PANDA.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            Db = new ApplicationDbContext();
        }

        public ApplicationDbContext Db { get; set; }
    }
}
