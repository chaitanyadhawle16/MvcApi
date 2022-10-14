using System.Net.Http;
using MySql.Data.MySqlClient;
using Dapper;

namespace myApp.Models{

    public class ProductRepository{
        private static string connectionString = "Server=localhost; Port=3306; Database=products_schema; Uid=root; Pwd=root;";
        static MySqlConnection connection = new MySqlConnection(connectionString);

        public static  string applyFilters(String filter){
            String [] arr = filter.Split(',');
            for(int i=0; i<arr.Count(); i++)
            {   
                arr[i] = "\"" + arr[i] + "\"";
            }
            String newFilter = String.Join(", ", arr);
            return newFilter;
        }

        public static string buildQuery(string brand, string category, string minPrice, string maxPrice){
            String sQuery = "SELECT * FROM product WHERE ";
            if(brand!=""){
                sQuery += $"Brand in ({brand}) ";
            }
            if(category!=""){
                if(brand!=""){
                    sQuery+="AND ";
                }
                sQuery += $"Category in ({category}) ";
            }
            if(brand!="" || category!=""){
                sQuery += "AND ";
            }
            sQuery+= $"Price BETWEEN {minPrice} AND {maxPrice};";
            return sQuery;
        }

        public static List<String> getImageList(int id){
            String sQuery = $"SELECT imageURL FROM product_images WHERE productID = {id}";
            List<String> imageList = connection.Query<String>(sQuery).ToList();
            return imageList;
        }
        
        public static List<ProductDetails> getProductList(){
            string sQuery = "SELECT * FROM product";
            List<ProductDetails> productList = connection.Query<ProductDetails>(sQuery).ToList();
            foreach(var product in productList){
                product.Images = getImageList(product.Id);
            }
            return productList;
        }

        public static List<ProductDetails> getProductList(string brand, string category, string minPrice, string maxPrice){
            if(brand!="") brand = applyFilters(brand);
            if(category!="") category = applyFilters(category);
            string sQuery = buildQuery(brand,category,minPrice,maxPrice);
            List<ProductDetails> productList = connection.Query<ProductDetails>(sQuery).ToList();

            foreach(var product in productList){
                product.Images = getImageList(product.Id);
            }
            return productList;
        }    
    }

}