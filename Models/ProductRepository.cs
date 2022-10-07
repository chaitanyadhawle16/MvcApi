using System.Net.Http;
using MySql.Data.MySqlClient;
using Dapper;

namespace myApp.Models{

    public class ProductRepository{
        private static string connectionString = "Server=localhost; Port=3306; Database=products_schema; Uid=root; Pwd=root;";
        static MySqlConnection connection = new MySqlConnection(connectionString);

        public static List<String> getImageList(int id){
            
            String sQuery = $"SELECT * FROM ( SELECT image1 AS col FROM image where imageId={id} UNION SELECT image2 AS col FROM image where imageId={id} UNION SELECT image3 AS col FROM image where imageId={id} UNION SELECT image4 AS col FROM image where imageId={id} UNION SELECT image5 AS col FROM image where imageId={id} UNION SELECT image6 AS col FROM image where imageId={id}) image WHERE col IS NOT NULL";
            
            List<String> imageList = connection.Query<String>(sQuery).ToList();
            return imageList;
        }
        //fetch all products
        public static List<ProductDetails> getProductList(){
            string sQuery = "SELECT * FROM product";
            List<ProductDetails> productList = connection.Query<ProductDetails>(sQuery).ToList();
            foreach(var product in productList){
                product.Images = getImageList(product.Id);
            }
            return productList;
        }

        // fetch products in a single page
        public static List<ProductDetails> getProductList(string brand, string category, int minPrice, int maxPrice){
            string sQuery = "SELECT * FROM product WHERE ";
            
            if(brand!=""){
                sQuery += $"Brand IN ({brand}) ";
                if(category!=""){
                    sQuery+= $"AND Category IN ({category}) ";
                }
                if(minPrice != 0 && maxPrice!=0){
                    sQuery += $"AND Price BETWEEN {minPrice} AND {maxPrice};";
                }
            }
            else if(category!=""){
                sQuery+= $"Category IN ({category}) ";
                if(minPrice != 0 && maxPrice!=0){
                    sQuery += $"AND Price BETWEEN {minPrice} AND {maxPrice};";
                }
            }
            else if(minPrice != 0 && maxPrice!=0){
                    sQuery += $"Price BETWEEN {minPrice} AND {maxPrice};";
            }
            else{
                sQuery = "SELECT * FROM product";
            }

            
            List<ProductDetails> productList = connection.Query<ProductDetails>(sQuery).ToList();

            foreach(var product in productList){
                product.Images = getImageList(product.Id);
            }
            return productList;
        }    
    }

}