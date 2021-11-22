﻿namespace MASA.EShop.Web.Client.Pages.Catalog;

public partial class Catalog : EShopPageBase
{

    private CatalogData _catalogViewModel = new();
    private CatalogOptinsModel _catalogOptinsModel = new() { Type = -1, Brand = -1 };
    private List<CatalogBrand> _brands = new();
    private List<CatalogType> _types = new();
    private string _wishListIcon = "mdi-heart-outline", _wishIconColor = "black";

    protected override string PageName { get; set; } = "Catalog";

    [Inject] //todo :change api open
    private CatalogService _catalogService { get; set; } = default!;

    [Inject]
    private BasketService _baksetService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Task.WhenAll(
            LoadBrandsAsync(),
            LoadTypesAsync(),
            LoadItemsAsync()
        );
    }

    private async Task LoadBrandsAsync()
    {
        var brands = new List<CatalogBrand>
            {
                new CatalogBrand(-1,"全部")
            };

        try
        {
            brands.AddRange(await _catalogService.GetBrandsAsync());
        }
        catch
        {
            // If call fails, we'll just have the 'All' selection.
        }

        _brands = brands;
    }

    private async Task LoadTypesAsync()
    {
        var types = new List<CatalogType>
            {
                new CatalogType(-1,"全部")
            };

        try
        {
            types.AddRange(await _catalogService.GetTypesAsync());
        }
        catch
        {
            // If call fails, we'll just have the 'All' selection.
        }

        _types = types;
    }

    private async Task OnPageIndexChangedAsync(int newPageIndex)
    {
        _catalogOptinsModel.PageIndex = newPageIndex;
        await LoadItemsAsync();
    }

    private async Task LoadItemsAsync()
    {
        try
        {
            _catalogViewModel = await _catalogService.GetCatalogItemsAsync(_catalogOptinsModel.PageIndex, 9, _catalogOptinsModel.Brand, _catalogOptinsModel.Type);
        }
        catch (Exception ex)
        {
            Message(ex.Message, BlazorComponent.AlertTypes.Error);
        }
    }

    private async void AddToCart(CatalogItem item)
    {
        if (IsAuthenticated)
        {
            await _baksetService.AddItemToBasketAsync("masa", item.Id);
            Navigation("basket");
        }
        else
        {
            Navigation("/");
        }
    }

    /// <summary>
    /// simulate add wishlist ui effect(bad with all element handle not specify element)
    /// </summary>
    private void AddWishList()
    {
        if (_wishIconColor == "black")
        {
            _wishListIcon = "mdi-heart";
            _wishIconColor = "red";
        }
        else
        {
            _wishListIcon = "mdi-heart-outline";
            _wishIconColor = "black";
        }
    }
}

