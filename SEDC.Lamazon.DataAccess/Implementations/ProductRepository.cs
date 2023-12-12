using Microsoft.EntityFrameworkCore;
using SEDC.Lamazon.DataAccess.Context;
using SEDC.Lamazon.DataAccess.Interfaces;
using SEDC.Lamazon.Domain.Entities;

namespace SEDC.Lamazon.DataAccess.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly LamazonDbContext _lamazonDbContext;

    public ProductRepository(LamazonDbContext lamazonDbContext)
    {
        _lamazonDbContext = lamazonDbContext;
    }

    public void Delete(int id)
    {
        Product product = _lamazonDbContext
            .Products
            .Where(x => x.Id == id)
            .FirstOrDefault();
        
        _lamazonDbContext.Products.Remove(product);

        _lamazonDbContext.SaveChanges();
    }

    public Product Get(int id)
    {
        Product product = _lamazonDbContext
            .Products
            .Include(x => x.ProductCategory)
            .Where(x => x.Id == id)
            .FirstOrDefault();

        return product;
    }

    public List<Product> GetAll()
    {
        List<Product> products = _lamazonDbContext
            .Products
            .Include(p => p.ProductCategory)
            .ToList();

        return products;
    }

    public int Insert(Product product)
    {
        _lamazonDbContext.Products.Add(product);
        _lamazonDbContext.SaveChanges();

        return product.Id;
    }

    public void Update(Product product)
    {
        _lamazonDbContext.Products.Update(product);
        _lamazonDbContext.SaveChanges();
    }
}
