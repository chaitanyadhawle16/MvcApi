using PagedList;
namespace myApp.Models;

public class ResponseModel{

    public IPagedList<ProductDetails> products {get; set;}
    public int total {get; set;}

}