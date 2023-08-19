using ecommerce_App.Models;
using System.Data.SqlClient;

namespace ecommerce_App.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration configuration;
        private readonly string dbconnection;
        private readonly string dateformat;

        public DataAccess(IConfiguration configuration)
        {
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
        }
        public List<ProductCategory> GetProductCategories()
        {
            var productCategories = new List<ProductCategory>();
            using(SqlConnection connection = new (dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection, 
                };
                string query = "SELECT * FROM ProductCategories;";
                command.CommandText = query;    

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    var category = new ProductCategory()
                    {
                        Id = (int)reader["CategoryId"],
                        Category = (string)reader["Category"],
                        SubCategory = (string)reader["SubCategory"]
                    };
                    productCategories.Add(category);
                }
            }
            
            return productCategories;
        }

        public Offer GetOffer(int id)
        {
            var offer = new Offer();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection,
                };

                string query = "Select * from offers where OfferId =" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    offer.Id = (int)r["OfferId"];
                    offer.Title = (string)r["Title"];
                    offer.Discount = (int)r["Discount"];
                }
            }
                return offer;
        }

        public ProductCategory GetProductCategory(int id)
        {
            var productCategory = new ProductCategory();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection,
                };

                string query = "Select * from ProductCategories where CategoryId =" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    productCategory.Id = (int)r["CategoryId"];
                    productCategory.Category = (string)r["Category"];
                    productCategory.SubCategory = (string)r["SubCategory"];
                }
            }
            return productCategory;
        }

        public List<Product> GetProducts(string category, string subcategory, int count)
        {
            var products = new List<Product>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection,
                };

                string query = "Select Top " + count + " * from Products where CategoryId =(select CategoryId from ProductCategories where category = @c and subcategory = @s) Order by newid();";
                command.CommandText = query;
                command.Parameters.Add("@c", System.Data.SqlDbType.NVarChar).Value = category;
                command.Parameters.Add("@s", System.Data.SqlDbType.NVarChar).Value = subcategory;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        ImageName = (string)reader["ImageName"],
                    };

                    var categoryId = (int)reader["CategoryId"];
                    product.ProductCategory = GetProductCategory(categoryId);

                    var offerId = (int)reader["OfferId"];
                    product.Offer = GetOffer(offerId);

                    products.Add(product);
                }
            }
            return products;
        }
    }
}
