﻿namespace SEDC.Lamazon.Services.ViewModels.Product;

public class CreateProductViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }

    public int ProductCategoryId { get; set; }
}
