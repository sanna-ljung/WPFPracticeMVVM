using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using WpfPractice2.Command;
using WpfPractice2.Models;
using WpfPractice2.Services;

namespace WpfPractice2.ViewModels
{
    public class ProductViewModel: ViewModelBase
    {
        //private ObservableCollection<ProductItemViewModel> products = new();
        //private ProductItemViewModel? selectedProduct;

        //// Input-properties
        //private string inputArticleNumber = string.Empty;
        //private string inputCategory = string.Empty;
        //private string inputPrice = string.Empty;

        //public ObservableCollection<ProductItemViewModel> Products
        //{
        //    get { return products; }
        //    set
        //    {
        //        if (products != value)
        //        {
        //            products = value;
        //            RaisePropertyChanged();
        //        }
        //    }
        //}

        //public ProductItemViewModel? SelectedProduct
        //{
        //    get { return selectedProduct; }
        //    set
        //    {
        //        if (selectedProduct != value)
        //        {
        //            selectedProduct = value;
        //            RaisePropertyChanged();
        //            DeleteCommand?.RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        //public string InputArticleNumber
        //{
        //    get { return inputArticleNumber; }
        //    set
        //    {
        //        if (inputArticleNumber != value)
        //        {
        //            inputArticleNumber = value;
        //            RaisePropertyChanged();
        //            AddCommand?.RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        //public string InputCategory
        //{
        //    get { return inputCategory; }
        //    set
        //    {
        //        if (inputCategory != value)
        //        {
        //            inputCategory = value;
        //            RaisePropertyChanged();
        //            AddCommand?.RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        //public string InputPrice
        //{
        //    get { return inputPrice; }
        //    set
        //    {
        //        if (inputPrice != value)
        //        {
        //            inputPrice = value;
        //            RaisePropertyChanged();
        //            AddCommand?.RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        ////commands
        //public DelegateCommand AddCommand { get; private set; }
        //public DelegateCommand DeleteCommand { get; private set; }
        //public DelegateCommand ClearCommand { get; }

        //public ProductViewModel()
        //{
        //    AddCommand = new DelegateCommand(
        //        _ => AddProduct(),
        //        _ => CanAddProduct()
        //    );

        //    DeleteCommand = new DelegateCommand(
        //        _ => DeleteProduct(),
        //        _ => CanDeleteProduct()
        //    );

        //    ClearCommand = new DelegateCommand(_ => ClearInput());
        //}

        ////validering
        //private bool CanAddProduct()
        //{
        //    if (string.IsNullOrWhiteSpace(InputArticleNumber) || !int.TryParse(InputArticleNumber, out _))
        //        return false;

        //    if (string.IsNullOrWhiteSpace(InputCategory))
        //        return false;

        //    if (string.IsNullOrWhiteSpace(InputPrice) || !decimal.TryParse(InputPrice, out decimal price) || price < 0)
        //        return false;

        //    return true;
        //}

        ////kommandometoder
        //private void AddProduct()
        //{
        //    if (!CanAddProduct())
        //        return;

        //    int articleNumber = int.Parse(InputArticleNumber);
        //    decimal price = decimal.Parse(InputPrice);

        //    var newProduct = new ProductItemViewModel(new Models.Product
        //    {
        //        ArticleNumber = articleNumber,
        //        Category = InputCategory,
        //        Price = price
        //    });

        //    Products.Add(newProduct);
        //    SelectedProduct = newProduct;
        //    ClearInput();
        //}
        //private bool CanDeleteProduct()
        //{
        //    return SelectedProduct != null;
        //}

        //private void DeleteProduct()
        //{
        //    if (SelectedProduct != null)
        //    {
        //        Products.Remove(SelectedProduct);
        //        SelectedProduct = null;
        //    }
        //}

        //private void ClearInput()
        //{
        //    InputArticleNumber = string.Empty;
        //    InputCategory = string.Empty;
        //    InputPrice = string.Empty;
        //}

        //public async Task LoadAsync()
        //{
        //    if (Products.Any())
        //        return;
        //}

        private readonly IProductService _productService;
        private ObservableCollection<ProductItemViewModel> products = new();
        private ProductItemViewModel? selectedProduct;

        // Input-properties
        private string inputArticleNumber = string.Empty;
        private string inputCategory = string.Empty;
        private string inputPrice = string.Empty;
        private bool isLoading = false;

        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return products; }
            set
            {
                if (products != value)
                {
                    products = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ProductItemViewModel? SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                if (selectedProduct != value)
                {
                    selectedProduct = value;
                    RaisePropertyChanged();
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string InputArticleNumber
        {
            get { return inputArticleNumber; }
            set
            {
                if (inputArticleNumber != value)
                {
                    inputArticleNumber = value;
                    RaisePropertyChanged();
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string InputCategory
        {
            get { return inputCategory; }
            set
            {
                if (inputCategory != value)
                {
                    inputCategory = value;
                    RaisePropertyChanged();
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string InputPrice
        {
            get { return inputPrice; }
            set
            {
                if (inputPrice != value)
                {
                    inputPrice = value;
                    RaisePropertyChanged();
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand ClearCommand { get; }

        // Konstruktor,ta emot service från DI
        public ProductViewModel(IProductService productService)
        {
            _productService = productService;

            AddCommand = new DelegateCommand(
                _ => AddProductAsync(),
                _ => CanAddProduct()
            );

            DeleteCommand = new DelegateCommand(
                _ => DeleteProductAsync(),
                _ => CanDeleteProduct()
            );

            ClearCommand = new DelegateCommand(_ => ClearInput());
        }

        // Hämta produkter från databasen vid start
        public async Task LoadAsync()
        {
            try
            {
                IsLoading = true;
                var dbProducts = await _productService.GetAllProductsAsync();

                Products.Clear();
                foreach (var product in dbProducts)
                {
                    Products.Add(new ProductItemViewModel(product));
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Fel vid inladdning: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool CanAddProduct()
        {
            if (string.IsNullOrWhiteSpace(InputArticleNumber) || !int.TryParse(InputArticleNumber, out _))
                return false;

            if (string.IsNullOrWhiteSpace(InputCategory))
                return false;

            if (string.IsNullOrWhiteSpace(InputPrice) || !decimal.TryParse(InputPrice, out decimal price) || price < 0)
                return false;

            return true;
        }

        // Spara till databas (async)
        private async Task AddProductAsync()
        {
            if (!CanAddProduct())
                return;

            try
            {
                int articleNumber = int.Parse(InputArticleNumber);
                decimal price = decimal.Parse(InputPrice);

                var newProduct = new Product
                {
                    ArticleNumber = articleNumber,
                    Category = InputCategory,
                    Price = price
                };

                // Spara till databas
                await _productService.AddProductAsync(newProduct);

                // Uppdatera UI
                Products.Add(new ProductItemViewModel(newProduct));
                SelectedProduct = Products.Last();
                ClearInput();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Fel vid tillägg: {ex.Message}");
            }
        }

        private bool CanDeleteProduct()
        {
            return SelectedProduct != null;
        }

        // Ta bort från databas (async)
        private async Task DeleteProductAsync()
        {
            if (SelectedProduct == null)
                return;

            try
            {
                // Ta bort från databas
                await _productService.DeleteProductAsync(SelectedProduct.ArticleNumber);

                // Uppdatera UI
                Products.Remove(SelectedProduct);
                SelectedProduct = null;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Fel vid borttagning: {ex.Message}");
            }
        }

        private void ClearInput()
        {
            InputArticleNumber = string.Empty;
            InputCategory = string.Empty;
            InputPrice = string.Empty;
        }
    }
}
