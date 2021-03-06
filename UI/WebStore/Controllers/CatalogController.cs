﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData ProductData) => _ProductData = ProductData;

        public IActionResult Shop(int? SectionId, int? BrandId)
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId
            });

            var model = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Order = p.Order,
                    Price = p.Price,
                    Brand = p.Brand?.Name ?? string.Empty
                }).OrderBy(p => p.Order).ToArray()
            };

            return View(model);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null)
                return NotFound();
            return View(new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                Price = product.Price,
                Brand = product.Brand?.Name ?? string.Empty
            });
        }
    }
}