using ContradoChallengeAPI.DataAccessLayer;
using ContradoModel.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ContradoChallengeAPI.Repository
{
    public class ProductRepository : IDisposable
    {
        private readonly DLProductRepository productRepository;
        public ProductRepository()
        {
            productRepository = new DLProductRepository();
        }

        public async Task<List<GetAllProductsResponse>> GetAllProducts(GetAllProductsRequest getAllProductsRequest)
        {
            SqlParameter[] parameter = {
                    new SqlParameter("@PageIndex", getAllProductsRequest.PageIndex),
                    new SqlParameter("@PageSize", getAllProductsRequest.PageSize),
                };

            return await productRepository.GetAllProducts(parameter);
        }
        public async Task<GetAllProductsResponse> GetProductByID(GetProductByID getProductByIDRequest)
        {
            SqlParameter[] parameter = {
                    new SqlParameter("@ProductID", getProductByIDRequest.ProductID)
                };

            return await productRepository.GetProductByID(parameter);
        }
        public async Task<long> SaveProduct(ProductAPIModel saveProductRequest)
        {
            SqlParameter[] parameter = {
                    new SqlParameter("@ProductId", saveProductRequest.ProductId),
                    new SqlParameter("@ProdCatId", saveProductRequest.ProdCatId),
                    new SqlParameter("@AttributeId", saveProductRequest.AttributeId),
                    new SqlParameter("@ProdName", saveProductRequest.ProdName),
                    new SqlParameter("@ProdDescription", saveProductRequest.ProdDescription),
                    new SqlParameter("@AttibuteValue", saveProductRequest.AttributeValue)
                };

            var result = await productRepository.SaveProduct(parameter);
            return result;
        }

        public async Task DeleteProduct(GetProductByID getProductByIDRequest)
        {
            SqlParameter[] parameter = {
                    new SqlParameter("@ProductId", getProductByIDRequest.ProductID)
                };

            await productRepository.DeleteProduct(parameter);
        }

        public void Dispose()
        {
            
        }
    }
}
