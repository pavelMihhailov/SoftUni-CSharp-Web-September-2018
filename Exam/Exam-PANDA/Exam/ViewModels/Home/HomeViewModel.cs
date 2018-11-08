using System.Collections.Generic;

namespace PANDA.ViewModels.Home
{
    public class HomeViewModel  
    {
        public ICollection<BoxViewModel> Pending { get; set; }

        public ICollection<BoxViewModel> Shipped { get; set; }

        public ICollection<BoxViewModel> Delivered { get; set; }
    }
}
