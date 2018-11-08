using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exam.Models.Enums;
using PANDA.Models;
using PANDA.ViewModels.Receipts;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace PANDA.Controllers
{
    public class ReceiptsController : BaseController
    {
        [Authorize]
        public IHttpResponse Index()
        {
            var collectionOfReceipts = Db.Receipts.Where(x => x.Recipient.Username == User.Username)
                .Select(x => new MyReceiptSingleViewModel
                {
                    Id = x.Id,
                    IssuedOn = x.IssuedOn.ToShortDateString(),
                    Fee = x.Fee,
                    Recipient = x.Recipient.Username
                })
                .ToList();

            var viewModel = new MyReceiptsCollectionViewModel{ Receipts = collectionOfReceipts };

            return this.View(viewModel);
        }
        
        [Authorize]
        public IHttpResponse Acquire(int id)
        {
            var packageToAcquire = Db.Packages.FirstOrDefault(x => x.Id == id);
            var currentUser = User.Username;
            var dbUser = Db.Users.FirstOrDefault(x => x.Username == currentUser);

            if (dbUser.Id != packageToAcquire.Recipient.Id)
            {
                return BadRequestError("Cannot acquire package, because it`s not yours :)");
            }

            var user = Db.Users.FirstOrDefault(x => x.Username == currentUser);

            var receipt = new Receipt
            {
                PackageId = packageToAcquire.Id,
                RecipientId = user.Id,
                IssuedOn = DateTime.Now,
                Fee = Convert.ToDecimal(packageToAcquire.Weight * 2.67)
            };

            packageToAcquire.Status = Status.Acquired;
            Db.Receipts.Add(receipt);
            user.ReceiptsRecieved.Add(receipt);
            Db.SaveChanges();

            return this.Redirect("/Home/Index");
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var receipt = Db.Receipts.FirstOrDefault(x => x.Id == id);

            if (receipt == null)
            {
                return BadRequestError("Receipt doesn`t exists");
            }

            var viewModel = new ReceiptDetailsViewModel();
            var package = Db.Packages.FirstOrDefault(x => x.Id == receipt.PackageId);
            var recipient = Db.Users.FirstOrDefault(x => x.Id == receipt.RecipientId);

            viewModel.Id = id;
            viewModel.IssuedOn = receipt.IssuedOn.ToShortDateString();
            viewModel.DeliveryAddress = package.ShippingAddress;
            viewModel.PackageWeight = package.Weight;
            viewModel.PackageDescription = package.Description;
            viewModel.Recipient = recipient.Username;
            viewModel.Total = receipt.Fee; 

            return this.View(viewModel);
        }
    }
}
