using SEDC.Lamazon.DataAccess.Context;
using SEDC.Lamazon.DataAccess.Interfaces;
using SEDC.Lamazon.Domain.Entities;

namespace SEDC.Lamazon.DataAccess.Implementations;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly LamazonDbContext _dbContext;

    public ProductCategoryRepository(LamazonDbContext lamazonDbContext)
    {
        _dbContext = lamazonDbContext;
    }

    public void Delete(int id)
    {
        ProductCategory productCategory = _dbContext
            .ProductCategories
            .FirstOrDefault(pc => pc.Id == id);

        _dbContext.ProductCategories.Remove(productCategory);
        
        _dbContext.SaveChanges();
    }

    public ProductCategory Get(int id)
    {
        ProductCategory productCategory = _dbContext
           .ProductCategories
           .FirstOrDefault(pc => pc.Id == id);

        return productCategory;
    }

    public List<ProductCategory> GetAll()
    {
        List<ProductCategory> productCategories = _dbContext
            .ProductCategories
            .ToList();

        return productCategories;
    }

    public int Insert(ProductCategory productCategory)
    {
        _dbContext
            .ProductCategories
            .Add(productCategory);
        
        _dbContext.SaveChanges();

        return productCategory.Id;
    }

    public void Update(ProductCategory productCategory)
    {
        _dbContext
            .ProductCategories
            .Update(productCategory);

        _dbContext.SaveChanges();
    }
}