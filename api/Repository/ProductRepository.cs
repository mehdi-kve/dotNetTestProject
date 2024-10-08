﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Dtos.Product;
using api.Helper;
using api.Interfaces;
using api.Models;

namespace api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;

        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync(QueryObject query)
        {
            var products = _context.Products.Include(a => a.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Username))
            {
                products = products.Where(p => p.AppUser.UserName.Contains(query.Username));
            }

            return await products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var products = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (products == null)
                return null;

            return products;
        }

        public async Task<Product> CreateAsync(Product productModel,string appUserId)
        {
            productModel.AppUserId = appUserId;
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<Product?> UpdateAsync(int id, UpdateProductDto productModel)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return null;

            product.Name = productModel.Name;
            product.ManufacturePhone = productModel.ManufacturePhone;
            product.ManufactureEmail = productModel.ManufactureEmail;
            product.ProduceDate = productModel.ProduceDate;
            product.IsAvailable = productModel.IsAvailable;

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return null;
            
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

    }
}
