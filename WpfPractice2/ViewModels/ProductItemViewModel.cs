using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources.Extensions;
using System.Text;
using System.Threading.Tasks;
using WpfPractice2.Models;

namespace WpfPractice2.ViewModels
{
    public class ProductItemViewModel : ViewModelBase
    {
        private readonly Product model;
        public ProductItemViewModel(Product model)
        {
            this.model = model;
        }

        public int ArticleNumber
        {
            get { return model.ArticleNumber; }
            set
            {
                if (model.ArticleNumber != value)
                {
                    model.ArticleNumber = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Category
        {
            get { return model.Category; }
            set
            {
                if (model.Category != value)
                {
                    model.Category = value;
                    RaisePropertyChanged();
                }
            }
        }
        public decimal Price
        {
            get { return model.Price; }
            set
            {
                if (model.Price != value)
                {
                    model.Price = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
