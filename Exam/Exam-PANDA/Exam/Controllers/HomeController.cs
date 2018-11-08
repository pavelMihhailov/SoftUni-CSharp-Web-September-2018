using System.Collections.Generic;
using System.Linq;
using Exam.Models.Enums;
using PANDA.ViewModels.Home;
using SIS.HTTP.Responses;

namespace PANDA.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            var user = Db.Users.FirstOrDefault(x => x.Username == User.Username);

            if (user != null)
            {
                var packages = Db.Packages.Where(x => x.RecipientId == user.Id).ToList();
                var viewModel = new HomeViewModel();

                viewModel.Pending = packages
                    .Where(x => x.Status == Status.Pending)
                    .Select(p => new BoxViewModel
                    {
                        Id = p.Id,
                        Name = p.Description
                    }).ToList();

                viewModel.Shipped = packages
                    .Where(x => x.Status == Status.Shipped)
                    .Select(p => new BoxViewModel
                    {
                        Id = p.Id,
                        Name = p.Description
                    }).ToList();

                viewModel.Delivered = packages
                    .Where(x => x.Status == Status.Delivered)
                    .Select(p => new BoxViewModel
                    {
                        Id = p.Id,
                        Name = p.Description
                    }).ToList();

                return this.View("Home/IndexLoggedIn", viewModel);
            }

            return this.View();
        }
    }
}
