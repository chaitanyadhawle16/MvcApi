using System.Net.Http;
using MySql.Data.MySqlClient;
using Dapper;

namespace myApp.Models{

    public class ProductRepository{
        private static string connectionString = "Server=localhost; Port=3306; Database=products_schema; Uid=root; Pwd=root;";
        static MySqlConnection connection = new MySqlConnection(connectionString);

        public static List<dynamic> getProductList(){
            string sQuery = "SELECT * FROM product JOIN image ON product.Id = image.imageId";
            var productList = connection.Query<dynamic>(sQuery);
            return productList.ToList();
        }

        public static List<dynamic> getProductList(string brand, string category, int minPrice, int maxPrice){
            string sQuery = "SELECT * FROM product JOIN image ON product.Id = image.imageId WHERE ";
            
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
                sQuery = "SELECT * FROM product JOIN image ON product.Id = image.imageId";
            }

            
            var productList = connection.Query<dynamic>(sQuery);
            return productList.ToList();
        }    

    }

}