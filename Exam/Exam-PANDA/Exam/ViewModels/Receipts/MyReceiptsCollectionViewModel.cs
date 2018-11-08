using System.Collections.Generic;

namespace PANDA.ViewModels.Receipts
{
    public class MyReceiptsCollectionViewModel
    {
        public ICollection<MyReceiptSingleViewModel> Receipts { get; set; }
    }
}
