using ecomCapstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ecomCapstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegistration : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public UserRegistration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("registerUser")]
        public Response RegisterUser(Registration register)
        {
            Response op = new Response();
            try
            {
                bool useractive = true;
                string usercreatedOn = "2022-12-13 05:03:22.000";

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());
                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Registration] ([Name] ," +
                    "[Email]" +
                    ",[Password]" +
                    ",[IsActive]" +
                    ",[CreatedOn]" +
                    ") " +
                    "VALUES ('" + register.name + "','" + register.email + "','" + register.password + "','" + useractive + "','" + usercreatedOn + "')", con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    op.Success = true;
                    op.Message = "User Registered Successfully";
                }
                else
                {
                    op.Success = false;
                    op.Message = "Error while saving";
                }
            }
            catch(Exception ex)
            {
                op.Success = false;
                op.Message = ex.Message.ToString();
            }
            return op;  
        }

        [HttpPost]
        [Route("loginUser")]
        public ResponseObj<useroutdata> loginUser(Registration register)
        {
           
            ResponseObj<useroutdata> op = new ResponseObj<useroutdata>();
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("EcomCon").ToString());
                SqlDataAdapter da =
                    new SqlDataAdapter("select * from [dbo].[Registration] where [Email] = '" + register.email +
                    "' and [Password] = '" + register.password + "'  and IsActive = 1  ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows[0]["IsAdmin"].ToString() == "True")
                {
                    op.Data.Add(new useroutdata()
                    {
                      isAdmin = true,
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
            catch(Exception ex)
            {
                op.Success = false;
                op.Message = ex.Message.ToString();

            }
            return op;
        }

    }
}
