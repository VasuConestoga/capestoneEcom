﻿using ecomCapstone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ecomCapstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("GetProductData")]
        public ProductOutput<ProductList> GetProductData(ProductInput input)
        {
            ProductOutput<ProductList> op = new ProductOutput<ProductList>();
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());

                string query = string.Empty;
                query = "select PM.Id as ProductId , PM.Name as ProductName," +
                    "PM.Description as ProductDescription , PM.Quantity , PM.Price , PM.ImgUrl as ProductImage,CM.Name as CategoryName " +
                    "from ProductMaster PM " +
                    "INNER JOIN ProductCatgoryMapping PCM on PM.ID = PCM.ProductId " +
                    "INNER JOIN CategoryMaster CM on CM.ID = PCM.CategoryId where 1=1 and [isActive] = 1 ";

                if (!String.IsNullOrEmpty(input.searchtext) )
                {
                    query = query + " AND PM.Name like '%" + input.searchtext + "%'";
                }

                if (!String.IsNullOrEmpty(input.category))
                {
                    query = query + " AND CM.Name = '" + input.category + "'"  ;
                }

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach(DataRow item in dt.Rows)
                {
                    op.Data.Add(new ProductList()
                        { 

                    Id = Convert.ToInt32(item["ProductId"].ToString()),
                    name = item["ProductName"].ToString(),
                    description = item["ProductDescription"].ToString(),
                    category = item["CategoryName"].ToString(),
                    imageUrl = item["ProductImage"].ToString(),
                    price = Convert.ToDecimal(item["Price"].ToString()),
                    quantity = Convert.ToInt32(item["Quantity"].ToString()),

                });
            }


                if (dt.Rows.Count > 0)
                {
                    op.Success = true;
                    op.Message = "User login successful";
                }
                else
                {
                    op.Success = false;
                    op.Message = "invalid User";
                }
            }
            catch (Exception ex)
            {
                op.Success = false;
                op.Message = ex.Message.ToString();

            }
            return op;
        }

        [HttpPost]
        [Route("AddProduct")]
        public Response AddProduct(ProductList product)
        {
            Response op = new Response();
            try
            {
                bool active = true;
                DateTime createdOn = DateTime.UtcNow;

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());
                SqlCommand cmd = new SqlCommand("INSERT INTO[dbo].[ProductMaster] ([Name],[Description],[ImgUrl],[Quantity],[Price],[isActive],[CreatedOn])" +
                    "VALUES ('" + product.name + "','" 
                                + product.description + "','" 
                                + product.imageUrl + "','" 
                                + product.quantity + "','" 
                                + product.price + "','"
                                + active + "','"
                                + createdOn + "')", con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    op.Success = true;
                    op.Message = "Product Added Successfully";
                }
                else
                {
                    op.Success = false;
                    op.Message = "Error while saving";
                }
            }
            catch (Exception ex)
            {
                op.Success = false;
                op.Message = ex.Message.ToString();

            }
            return op;
        }

        [HttpPost]
        [Route("UpdateProduct")]
        public Response UpdateProduct(ProductList product)
        {
            Response op = new Response();
            try
            {
                DateTime UpdatedOn = DateTime.UtcNow;

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[ProductMaster]  SET [Name] = '" + product.name 
                    + "',[Description] = '" + product.description
                    + "',[ImgUrl] = '" + product.imageUrl 
                    + "',[Quantity] ='" + product.quantity 
                    + "',[Price] ='" + product.price 
                    + "',[UpdatedOn] ='" + UpdatedOn
                    + "' WHERE Id = '" + product.Id + "'", con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    op.Success = true;
                    op.Message = "Product Updated Successfully";
                }
                else
                {
                    op.Success = false;
                    op.Message = "Error while saving";
                }
            }
            catch (Exception ex)
            {
                op.Success = false;
                op.Message = ex.Message.ToString();

            }
            return op;
        }


        [HttpPost]
        [Route("DeleteProduct")]
        public Response DeleteProduct(ProductList product)
        {
            Response op = new Response();
            try
            {
                bool active = false;
                DateTime UpdatedOn = DateTime.UtcNow;

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[ProductMaster]  SET [isActive] = '" + active
                    + "',[UpdatedOn] ='" + UpdatedOn
                    + "' WHERE Id = '" + product.Id + "'", con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    op.Success = true;
                    op.Message = "Product Deleted Successfully";
                }
                else
                {
                    op.Success = false;
                    op.Message = "Error while saving";
                }
            }
            catch (Exception ex)
            {
                op.Success = false;
                op.Message = ex.Message.ToString();

            }
            return op;
        }

    }
}
