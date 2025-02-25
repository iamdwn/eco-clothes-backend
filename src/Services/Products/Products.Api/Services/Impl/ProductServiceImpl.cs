﻿using DataAccess.Base;
using DataAccess.Models;
using Products.Api.Dtos.Request;
using System.Text.RegularExpressions;

namespace Products.Api.Services.Impl
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISizeService _sizeService;
        private readonly ICategoryService _categoryService;

        public ProductServiceImpl(IUnitOfWork unitOfWork, ISizeService sizeService, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _sizeService = sizeService;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return _unitOfWork.ProductRepository.Get(
                includeProperties: "SizeProducts,ProductCategories"
                ).ToList();
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var existingProduct = _unitOfWork.ProductRepository.Get(
                filter: p => p.ProductId.Equals(id),
                includeProperties: "SizeProducts,ProductCategories"
                ).FirstOrDefault();

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }
            return existingProduct;
        }

        public async Task<Product> CreateProductAsync(RequestProduct product)
        {
            try
            {
                var existingProduct = _unitOfWork.ProductRepository.Get(
                    filter: p => p.ProductName.Equals(product.ProductName)
                    ).FirstOrDefault();

                if (existingProduct != null)
                {
                    throw new KeyNotFoundException($"Product with name {product.ProductName} is exist.");
                }

                var insertProduct = new Product()
                {
                    ProductName = product.ProductName,
                    OldPrice = product.OldPrice,
                    NewPrice = product.NewPrice,
                    NumberOfSold = 0,
                    Amount = 0,
                    ImgUrl = product.ImgUrl,
                    Description = product.Description,
                    Slug = GenerateSlug(product.ProductName)
                };

                _unitOfWork.ProductRepository.Insert(insertProduct);
                _unitOfWork.Save();

                int amount = 0;

                foreach (var item in product.Sizes)
                {
                    await _sizeService.InsertSize(item, insertProduct.ProductId);
                    amount += item.SizeQuantity;
                };

                foreach (var item in product.Categories)
                {
                    await _categoryService.InsertCategory(item, insertProduct.ProductId);
                };

                insertProduct = _unitOfWork.ProductRepository.GetByID(insertProduct.ProductId);
                insertProduct.Amount = amount;
                _unitOfWork.ProductRepository.Update(insertProduct);
                _unitOfWork.Save();

                return insertProduct;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync(RequestProduct product)
        {
            try
            {
                var existingProduct = _unitOfWork.ProductRepository.GetByID(product.ProductId);

                if (existingProduct == null)
                {
                    throw new KeyNotFoundException($"Product with id {product.ProductId} not found.");
                }

                var newAmount = product.Sizes.Sum(s => s.SizeQuantity);

                await _sizeService.UpdateSize(product.Sizes, existingProduct.ProductId);
                await _categoryService.UpdateCategory(product.Categories, existingProduct.ProductId);

                existingProduct.ProductName = product.ProductName ?? existingProduct.ProductName;
                existingProduct.OldPrice = product.OldPrice ?? existingProduct.OldPrice;
                existingProduct.NewPrice = product.NewPrice ?? existingProduct.NewPrice;
                existingProduct.ImgUrl = product.ImgUrl ?? existingProduct.ImgUrl;
                existingProduct.Description = product.Description ?? existingProduct.Description;
                existingProduct.Amount = newAmount;

                _unitOfWork.ProductRepository.Update(existingProduct);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task DeleteProductAsync(Guid id)
        {
            var existingProduct = _unitOfWork.ProductRepository.GetByID(id);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            await _sizeService.DeleteSize(existingProduct.ProductId);
            await _categoryService.DeleteCategory(existingProduct.ProductId);

            _unitOfWork.ProductRepository.Delete(id);
            _unitOfWork.Save();
        }

        private bool ProductExists(Guid id)
        {
            return _unitOfWork.ProductRepository.GetByID(id) != null ? true : false;
        }

        public async Task<IEnumerable<Product>> GetProductBySellerIdAsync(Guid userId)
        {
            var existingProduct = _unitOfWork.ProductRepository.Get(
                filter: p => p.UserId.Equals(userId),
                includeProperties: "SizeProducts"
                ).ToList();

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Products of user has ID is {userId} not found.");
            }
            return existingProduct;
        }

        public async Task<Product> GetProductBySlugAsync(string slug)
        {
            var existingProduct = _unitOfWork.ProductRepository.Get(
                    filter: p => p.Slug.Equals(slug),
                    includeProperties: "SizeProducts"
                    ).FirstOrDefault();

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with Slug {slug} not found.");
            }
            return existingProduct;
        }

        public string GenerateSlug(string productName)
        {
            //var normalizedString = productName.Normalize(NormalizationForm.FormD);
            //var stringBuilder = new StringBuilder();

            //foreach (var c in normalizedString)
            //{
            //    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            //    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            //    {
            //        stringBuilder.Append(c);
            //    }
            //}

            //var slug = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            string[] vietnameseSigns = new string[] {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                    productName = productName.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
            }

            var slug = productName;

            slug = slug.ToLower();

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"-+", "-");

            slug = slug.Trim('-');

            return slug;
        }
    }
}
