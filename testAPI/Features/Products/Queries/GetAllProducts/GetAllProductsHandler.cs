﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using testAPI.Data;
using testAPI.Dtos.Product;
using testAPI.Interfaces;
using testAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace testAPI.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(ApplicationDBContext context, IMapper mapper,
        UserManager<AppUser> userManager, IProductRepository productRepository)
        {
            _productRepo = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepo.GetAllAsync(request.query);
            var productDto = products.Select(p => _mapper.Map<ProductDto>(p));
            return productDto;
        }
    }
}

 