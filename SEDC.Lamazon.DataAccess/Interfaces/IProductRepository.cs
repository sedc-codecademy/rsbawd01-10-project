using SEDC.Lamazon.Domain.Entities;

namespace SEDC.Lamazon.DataAccess.Interfaces;

public interface IProductRepository
{
    List<Product> GetAll();
    Product Get(int id);
    int Insert(Product productCategory);
    void Update(Product productCategory);
    void Delete(int id);
}
