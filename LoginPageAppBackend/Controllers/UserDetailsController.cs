using LoginPageAppBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace LoginPageAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserDetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("signup")]
        public object signup(UserDetails userDetails)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("UserDetailsConnection").ToString());
            SqlCommand cmd = new SqlCommand("insert into userdetails(username,password) values('" + userDetails.username+"','" + userDetails.password+"');", con);
            
            try
            {
                con.Open();
                int noOfRowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                if (noOfRowsAffected > 0)
                {
                    return new { prop = "Success" };
                }
                else
                {
                    return new { prop = "Error" };
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        [Route("login")]
        public object login(UserDetails userDetails)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("UserDetailsConnection").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM userdetails where username = '" + userDetails.username + "' and password = '" + userDetails.password + "';", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return new { prop = "success" };
            }
            else
            {
                return new { prop = "error" };
            }
        }
    }
}
