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
    public class OrderController : ControllerBase
    {
        private readonly OrderDBContext _context;

        public OrderController(OrderDBContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public IEnumerable<Orders> GetOrders()
        {
            string sql = string.Format(@"SELECT
	                so.[RECID]
                    ,[SALESID]
	                ,[PURCHID]
                    ,so.[STMSTOREID] as StoreId
	                ,store.STMSTORENAME as StoreName
	                ,[SALESNAME] as CustName
	                ,[SALESPOOLID] as [Pool]
	                ,[SALESQTY] as Qty
	                ,[SALESAMOUNT] as Amount
                    ,[SALESDATE] as [Date]
                    ,[DUEDATE]
                    ,[CONFIRMDATE]
	                ,(SELECT COUNT(RECID) FROM dbo.STMSALESSODAILYLINE WHERE SALESORDERDAILY = so.RECID) LINECOUNT
                    , 0 AS IMAGECOUNT --,(SELECT COUNT(RECID) FROM dbo.STMSALESIMAGE WHERE REFRECID = so.RECID) IMAGECOUNT
                FROM[dbo].[STMSALESSODAILY] so
	                LEFT JOIN [dbo].[STMSALESSTORE] store
		                ON store.STMSTOREID = so.STMSTOREID
	            WHERE SALES = CASE WHEN {1} = 1 THEN N'{0}' ELSE '' END
			            OR
			            SALESMANAGER = CASE WHEN {1} = 2 THEN N'{0}' ELSE '' END
			            OR
			            AREAMANAGER = CASE WHEN {1} = 3 THEN N'{0}' ELSE '' END
			            OR
			            KEYACMANAGER = CASE WHEN {1} = 4 THEN N'{0}' ELSE '' END
			            OR
			            1 = CASE WHEN {1} = 0 THEN 1 ELSE '' END
                ORDER BY so.SALESDATE DESC", "ชุติมา", 4);

            return _context.Orders.FromSql(sql).ToList();
        }

        [HttpGet("{name}:{type}")]
        public IEnumerable<Orders> GetOrders(string name, int type)
        {
            var nameParm = new SqlParameter("@n", name);
            var typeParm = new SqlParameter("@t", type);

            return _context.Orders.FromSql(@"DECLARE @Name NVARCHAR(50) SET @Name =@n
                                            DECLARE @Type int SET @Type = @t

                                            SELECT
	                                                so.[RECID]
                                                    ,[SALESID]
	                                                ,[PURCHID]
                                                    ,so.[STMSTOREID] as StoreId
	                                                ,store.STMSTORENAME as StoreName
	                                                ,[SALESNAME] as CustName
	                                                ,[SALESPOOLID] as [Pool]
	                                                ,[SALESQTY] as Qty
	                                                ,[SALESAMOUNT] as Amount
                                                    ,[SALESDATE] as [Date]
                                                    ,[DUEDATE]
                                                    ,[CONFIRMDATE]
	                                                ,(SELECT COUNT(RECID) FROM dbo.STMSALESSODAILYLINE WHERE SALESORDERDAILY = so.RECID) LINECOUNT
                                                    , 0 AS IMAGECOUNT --,(SELECT COUNT(RECID) FROM dbo.STMSALESIMAGE WHERE REFRECID = so.RECID) IMAGECOUNT
                                                FROM[dbo].[STMSALESSODAILY] so
	                                                LEFT JOIN [dbo].[STMSALESSTORE] store
		                                                ON store.STMSTOREID = so.STMSTOREID
	                                            WHERE SALES = CASE WHEN @Type = 1 THEN @Name ELSE '' END
			                                            OR
			                                            SALESMANAGER = CASE WHEN @Type = 2 THEN @Name ELSE '' END
			                                            OR
			                                            AREAMANAGER = CASE WHEN @Type = 3 THEN @Name ELSE '' END
			                                            OR
			                                            KEYACMANAGER = CASE WHEN @Type = 4 THEN @Name ELSE '' END
			                                            OR
			                                            1 = CASE WHEN @Type = 0 THEN 1 ELSE '' END
                                                ORDER BY so.SALESDATE DESC", nameParm, typeParm).ToList();
        }

        // GET: api/Pool
        [HttpGet("Pool")]
        public IEnumerable<Pool> GetPool()
        {
            return _context.Pools.FromSql(@"SELECT [SALESPOOLID] AS PoolId, [NAME] FROM [dbo].[SALESPOOL]").ToList();
        }

        // GET: api/Store
        [HttpGet("Store/{name}:{type}")]
        public IEnumerable<Store> GetStore(string name, int type)
        {
            var nameParm = new SqlParameter("@n", name);
            var typeParm = new SqlParameter("@t", type);
            return _context.Stores.FromSql(@"DECLARE @Name NVARCHAR(50) SET @Name = @n
                                            DECLARE @Type int SET @Type = @t

                                            SELECT RECID
                                                ,[STMSTOREID] as StoreId
                                                ,[STMSTORENAME] as [Name]
                                            FROM [dbo].[STMSALESSTORE] store
                                            WHERE SALES = CASE WHEN @Type = 1 THEN @Name ELSE '' END
	                                            OR
	                                            SALESMANAGER = CASE WHEN @Type = 2 THEN @Name ELSE '' END
	                                            OR
	                                            AREAMANAGER = CASE WHEN @Type = 3 THEN @Name ELSE '' END
	                                            OR
	                                            KEYACMANAGER = CASE WHEN @Type = 4 THEN @Name ELSE '' END
                                                OR
	                                                1 = CASE WHEN @Type = 0 THEN 1 ELSE '' END
	                                            ", nameParm, typeParm).ToList();
        }

        [HttpGet("Line/{recid}")]
        public IEnumerable<OrderLine> GetOrderLine(string recid)
        {
            var idParm = new SqlParameter("@id", recid);
 
            return _context.OrdersLine.FromSql(@"SELECT [RECID]
                                                    ,[MODEL]
                                                    ,[SALESAMOUNT] as Amount
                                                    ,[SALESDATE] as [Date]
                                                    ,[SALESQTY] as Qty
                                                    ,[SINK]
                                                    ,[SERIES]
	                                                ,STMSTOREID as [Top]
                                                    ,[SALESORDERDAILY] as RecIdHeader
                                                FROM [dbo].[STMSALESSODAILYLINE]
                                              WHERE SALESORDERDAILY = @id", idParm).ToList();
        }

        [HttpGet("Series/{pool}")]
        public IEnumerable<Serie> GetSeries(string pool)
        {
            var poolParm = new SqlParameter("@pool", pool);

            return _context.Series.FromSql(@"SELECT DISTINCT [SERIES]
                                          FROM [dbo].[STMPRODUCTSERIES]
                                          WHERE SALESPOOLID = @pool
                                          ORDER BY SERIES", poolParm).ToList();
        }

        [HttpGet("Models/{series}")]
        public IEnumerable<Model> GetModels(string series)
        {
            var seriesParm = new SqlParameter("@serie", series);

            return _context.Models.FromSql(@"SELECT DISTINCT [MODEL] as Models
                                          FROM [dbo].[STMPRODUCTSERIES]
                                          WHERE SERIES = @serie
                                          ORDER BY MODEL", seriesParm).ToList();
        }

        [HttpGet("Image/{refRecId}")]
        public IEnumerable<Image> GetModels(Int64 refRecId)
        {
            var refRecIdParm = new SqlParameter("@refRecId", refRecId);

            return _context.Images.FromSql(@"SELECT [RECID]
                                                  ,[IMAGE]
                                                  ,[NAME]
                                                  ,[SALESSODAILY]
                                                  ,[CREATEDATETIME]
                                              FROM [dbo].[STMSALESSODAILYIMAGES]
                                              WHERE SALESSODAILY = @refRecId
                                              ORDER BY [NAME]", refRecIdParm).ToList();
        }


        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrders(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orders = await _context.Orders.FindAsync(id);

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // PUT: api/Order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrders([FromRoute] int id, [FromBody] Orders orders)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orders.RecId)
            {
                return BadRequest();
            }

            _context.Entry(orders).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> PostOrders(Orders orders)
        {
            string sql = @"DECLARE @recid bigint 
                            SET @recid = (SELECT  MAX(RECID) + 1 FROM [dbo].[STMSALESSODAILY])

                            INSERT INTO [dbo].[STMSALESSODAILY]
                                       (RECID
		                               ,[SALESAMOUNT]
                                       ,[SALESDATE]
                                       ,[SALESPOOLID]
                                       ,[SALESQTY]
                                       ,[STMSTOREID]
                                       ,[CREATEDDATETIME]
                                       ,[CREATEDBY]
                                       ,[PURCHID]
                                       ,[SALESID]
                                       ,[SALESNAME])
                                 VALUES
                                       (@recid
		                               ,@AMOUNT
                                       ,@DATE
                                       ,@POOLID
                                       ,@QTY
                                       ,@STOREID
                                       ,GETDATE()
                                       ,@CREATEDBY
                                       ,@PURCHID
                                       ,@SALESID
                                       ,@SALESNAME
                            )";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Orders.Add(orders);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrders", new { id = orders.RecId }, orders);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrders([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();

            return Ok(orders);
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.RecId == id);
        }
    }
}