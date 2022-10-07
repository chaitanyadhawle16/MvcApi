using Microsoft.AspNetCore.Mvc;
using myApp.Models;
using PagedList;

namespace myApp.Controllers{
public class ProductController : Controller{

    private readonly ILogger<ProductController> _logger;
    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }
    
    public List<ProductDetails> GetAllProducts(){
        return ProductRepository.getProductList();
    }

    public List<ProductDetails> GetProducts(string brand = "", string category = "", int minPrice = 0, int maxPrice = 0, int page = 1){
        int productsPerPage = 5;
        var filteredProducts = ProductRepository.getProductList(brand,category,minPrice,maxPrice);
        IPagedList<ProductDetails> productList = filteredProducts.ToPagedList(page, productsPerPage);
        return productList.ToList();
    }
}
}