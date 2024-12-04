namespace FlorutMaraLab7;
using FlorutMaraLab7.Models;
public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
       this.BindingContext)
        {
            BindingContext = new Product()
        });

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        Product p;
        if (listView.SelectedItem != null)
        {
            p = listView.SelectedItem as Product;

            var shopl = (ShopList)BindingContext;

            var listProduct = await App.Database.GetListProductByProductIdAndShopListIdAsync(p.ID, shopl.ID);

            if (listProduct != null)
            {
                await App.Database.DeleteListProductAsync(listProduct);

                listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
            }
            else
            {
                await DisplayAlert("Error", "The selected item could not be found in the database.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Please select an item to delete.", "OK");
        }
    }





}