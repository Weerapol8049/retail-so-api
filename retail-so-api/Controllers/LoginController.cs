using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using retail_so_api.Models;

namespace retail_so_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginDBContext _context;

        public LoginController(LoginDBContext context)
        {
            _context = context;
        }


        //GET: api/Order/Login/admin/_admin123
        [HttpGet("{user}/{password}")]

        public IEnumerable<Login> GetLogin(string user, string password)

        {
            var userParm = new SqlParameter("@u",user);
            var pwdParm = new SqlParameter("@p", password);

            return _context.Login.FromSql(@"DECLARE @UserName VARCHAR(50) SET @UserName = @u
                                                DECLARE @Password VARCHAR(50) SET @Password = @p

                                                IF (@UserName = 'admin' and @Password = '_admin123')
                                                BEGIN
                                                 SELECT 
                                                  0 as RECID
                                                  ,'Admin' as [Name]
                                                  ,'_admin123' as [Password]
                                                  ,0 as [Type]
                                                  ,'admin' as Username
                                                END
                                                ELSE
                                                BEGIN
                                                 SELECT 
                                                   RECID
                                                  ,[STMNAME] as [Name]
                                                  ,[STMPASSWORD] as [Password]
                                                  ,[STMSALESSTORETYPE] as [Type]
                                                  ,[STMUSERNAME] as Username
                                                 FROM [dbo].[STMSALESUSER]
                                                 WHERE STMUSERNAME =@UserName AND STMPASSWORD = @Password
                                                END
                                                ", userParm, pwdParm).ToList();
        }


       
    }
}