using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exam.Models.Enums;
using PANDA.Models;
using PANDA.ViewModels.Packages;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace PANDA.Controllers
{
    public class PackagesController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse Details(int id)
        {
            var package = Db.Packages.FirstOrDefault(x => x.Id == id);

            if (package == null)
            {
                return BadRequestError("Package doesn`t exist");
            }

            var viewModel = new PackageDetailsViewModel();
            FillViewModel(package, viewModel);

            return this.View(viewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            var users = Db.Users.Select(x => x.Username).ToList();
            var viewModel = new CreateViewModel {Users = users};

            return this.View(viewModel);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(CreatePackageViewModel viewModel)
        {
            var recipient = Db.Users.FirstOrDefault(x => x.Username == viewModel.Recipient);
            
            var package = new Package
            {
                Description = viewModel.Description,
                RecipientId = recipient.Id,
                Weight = viewModel.Weight,
                ShippingAddress = viewModel.ShippingAddress,
                Status = Status.Pending,
            };
            Db.Packages.Add(package);
            Db.SaveChanges();

            return this.Redirect("/Home/Index");
        }

        [Authorize("Admin")]
        public IHttpResponse Pending()
        {
            var packages = Db.Packages.Where(x => x.Status == Status.Pending)
                .Select(x => new PendingViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    Recipient = x.Recipient.Username
                }).ToList();

            var viewModel = new PendingCollectionViewModel {PendingPackages = packages};

            return this.View(viewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Shipped()
        {
            var packages = Db.Packages.Where(x => x.Status == Status.Shipped)
                .Select(x => new ShippedViewModel()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Weight = x.Weight,
                    DeliveryDate = x.EstimatedDeliveryDate.Value.ToShortDateString(),
                    Recipient = x.Recipient.Username
                }).ToList();

            var viewModel = new ShippedCollectionViewModel() { ShippedPackages = packages};

            return this.View(viewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Delivered()
        {
            var packages = Db.Packages.Where(x => x.Status == Status.Delivered)
                .Select(x => new DeliveredViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    Recipient = x.Recipient.Username
                }).ToList();

            var viewModel = new DeliveredCollectionViewModel { DeliveredPackages = packages };

            return this.View(viewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Ship(int id)
        {
            var package = Db.Packages.FirstOrDefault(x => x.Id == id);
            package.Status = Status.Shipped;
            var random = new Random();
            var shippingDays = random.Next(20, 40);
            package.EstimatedDeliveryDate = DateTime.Now.AddDays(shippingDays);
            Db.SaveChanges();

            return this.Redirect("/Packages/Pending");
        }

        [Authorize("Admin")]
        public IHttpResponse Deliver(int id)
        {
            var package = Db.Packages.FirstOrDefault(x => x.Id == id);
            package.Status = Status.Delivered;
            package.EstimatedDeliveryDate = null;
            Db.SaveChanges();

            return this.Redirect("/Packages/Shipped");
        }

        private void FillViewModel(Models.Package package, PackageDetailsViewModel viewModel)
        {
            viewModel.ShippingAddress = package.ShippingAddress;
            viewModel.Status = package.Status;
            if (package.Status == Status.Pending)
            {
                viewModel.EstimatedDeliveryDate = "N/A";
            }
            if (package.Status == Status.Delivered)
            {
                viewModel.EstimatedDeliveryDate = "Delivered";
            }
            if (package.Status == Status.Shipped)
            {
                viewModel.EstimatedDeliveryDate = package.EstimatedDeliveryDate.ToString();
            }
            viewModel.Weight = package.Weight;
            var recipient = Db.Users.FirstOrDefault(x => x.Id == package.RecipientId);
            viewModel.Recipient = recipient != null ? recipient.Username : "N/A";
            viewModel.Description = package.Description;
        }
    }
}
