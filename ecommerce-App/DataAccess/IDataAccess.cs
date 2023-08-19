using ecommerce_App.Models;

namespace ecommerce_App.DataAccess
{
    public interface IDataAccess
    {
        List<ProductCategory> GetProductCategories();

        ProductCategory GetProductCategory(int id);
        Offer GetOffer(int id);
        List<Product> GetProducts(string category, string subcategory, int count);
    }
}
