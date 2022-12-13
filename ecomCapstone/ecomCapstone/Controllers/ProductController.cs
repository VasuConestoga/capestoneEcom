using ecomCapstone.Models;
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



        /// <summary>
        /// space
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

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
                    "INNER JOIN CategoryMaster CM on CM.ID = PCM.CategoryId where  PM.isActive = 1 ";

                if (!String.IsNullOrEmpty(input.searchtext))
                {
                    query = query + " AND PM.Name like '%" + input.searchtext + "%'";
                }

                if (!String.IsNullOrEmpty(input.category))
                {
                    query = query + " AND CM.Name = '" + input.category + "'";
                }

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow item in dt.Rows)
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
                }
                else
                {
                    op.Success = false;
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
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_AddProductwithCategorymapping";
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = product.name;
                cmd.Parameters.Add("@descr", SqlDbType.VarChar).Value = product.description;
                cmd.Parameters.Add("@img", SqlDbType.VarChar).Value = product.imageUrl;
                cmd.Parameters.Add("@Qnty", SqlDbType.Int).Value = product.quantity;
                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = product.price;
                cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = product.category;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                op.Success = true;
                op.Message = "Product Added Successfully";
            }

            catch (Exception ex)
            {
                throw ex;
                op.Success = false;
                op.Message = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
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


        [HttpPost]
        [Route("AddCategory")]
        public Response AddCategory(CategoryInput category)
        {
            Response op = new Response();
            try
            {
                bool active = true;
                DateTime createdOn = DateTime.UtcNow;

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());
                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[CategoryMaster] ([Name],[IsActive],[CreatedOn])" +
                    "VALUES ('" + category.name + "','"
                                + active + "','"
                                + createdOn + "')", con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    op.Success = true;
                    op.Message = "Category Added Successfully";
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
        [Route("UpdateCategory")]
        public Response UpdateCategory(CategoryInput category)
        {
            Response op = new Response();
            try
            {
                DateTime UpdatedOn = DateTime.UtcNow;

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[CategoryMaster]  SET [Name] = '" + category.name
                    + "',[UpdatedOn] ='" + UpdatedOn
                    + "' WHERE Id = '" + category.ID + "'", con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    op.Success = true;
                    op.Message = "Category Updated Successfully";
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
        [Route("DeleteCategory")]
        public Response DeleteCategory(CategoryInput category)
        {
            Response op = new Response();
            try
            {
                bool active = false;
                DateTime UpdatedOn = DateTime.UtcNow;

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[CategoryMaster]  SET [isActive] = '" + active
                    + "',[UpdatedOn] ='" + UpdatedOn
                    + "' WHERE Id = '" + category.ID + "'", con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    op.Success = true;
                    op.Message = "Catgeory Deleted Successfully";
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


        [HttpGet]
        [Route("GetCategoryData")]
        public ProductOutput<CategoryInput> GetCategoryData()
        {
            ProductOutput<CategoryInput> op = new ProductOutput<CategoryInput>();
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());

                string query = string.Empty;
                query = "select Id,Name from [dbo].[CategoryMaster] where 1=1 and [isActive] = 1 ";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow item in dt.Rows)
                {
                    op.Data.Add(new CategoryInput()
                    {

                        ID = Convert.ToInt32(item["Id"].ToString()),
                        name = item["Name"].ToString(),
                    });
                }

                if (dt.Rows.Count > 0)
                {
                    op.Success = true;
                }
                else
                {
                    op.Success = false;
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
