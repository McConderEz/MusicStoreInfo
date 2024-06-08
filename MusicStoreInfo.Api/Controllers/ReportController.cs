using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Api.Models;
using MusicStoreInfo.DAL;

namespace MusicStoreInfo.Api.Controllers
{
    public class ReportController : Controller
    {

        private readonly MusicStoreDbContext _context;
        private readonly string _connectionString = MusicStoreDbContext.CONNECTION_STRING;

        public ReportController(MusicStoreDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SymmetricInnerJoinWithCondition1()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SymmetricInnerJoinWithCondition1(int companyId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT a.Id AS AlbumId, a.Name AS AlbumName, a.ReleaseDate, c.Id AS CompanyId, c.Name AS CompanyName
            FROM Albums a
            INNER JOIN Companies c ON a.CompanyId = c.Id
            WHERE a.CompanyId = @CompanyId";

                var result = await connection.QueryAsync(query, new { CompanyId = companyId });
                return View("SymmetricInnerJoinWithCondition1Result", result);
            }
        }

        // Симметричное внутреннее соединение с условием (по внешнему ключу)
        [HttpGet]
        public IActionResult SymmetricInnerJoinWithCondition2()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SymmetricInnerJoinWithCondition2(int groupId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT a.Id AS AlbumId, a.Name AS AlbumName, g.Id AS GroupId, g.Name AS GroupName
            FROM Albums a
            INNER JOIN Groups g ON a.GroupId = g.Id
            WHERE a.GroupId = @GroupId";

                var result = await connection.QueryAsync(query, new { GroupId = groupId });
                return View("SymmetricInnerJoinWithCondition2Result", result);
            }
        }

        // Симметричное внутреннее соединение с условием (по дате)
        [HttpGet]
        public IActionResult SymmetricInnerJoinWithCondition3()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SymmetricInnerJoinWithCondition3(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT a.*, p.*
                    FROM Albums a
                    INNER JOIN Product p ON a.Id = p.AlbumId
                    WHERE a.ReleaseDate BETWEEN @StartDate AND @EndDate";

                var result = await connection.QueryAsync(query, new { StartDate = startDate, EndDate = endDate });
                return View("SymmetricInnerJoinWithCondition3Result", result);
            }
        }

        // Симметричное внутреннее соединение с условием (по дате)
        [HttpGet]
        public IActionResult SymmetricInnerJoinWithCondition4()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SymmetricInnerJoinWithCondition4(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var user = _context.Users.Include(u => u.Store).FirstOrDefault(u => u.UserName == User.Identity.Name);

                var query = @$"
            SELECT a.Id AS AlbumId, a.Name AS AlbumName, o.OrderDate, o.Quantity, s.Name AS StoreName
            FROM Albums a
            INNER JOIN Orders o ON a.Id = o.AlbumId
            INNER JOIN Stores s ON o.StoreId = s.Id
            WHERE o.OrderDate BETWEEN @StartDate AND @EndDate AND s.Name = '{user.Store.Name}'";

                var result = await connection.QueryAsync(query, new { StartDate = startDate, EndDate = endDate });
                return View("SymmetricInnerJoinWithCondition4Result", result);
            }
        }

        // Симметричное внутреннее соединение без условия
        [HttpGet]
        public async Task<IActionResult> SymmetricInnerJoinWithoutCondition1()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
    SELECT a.Id AS AlbumId, a.Name AS AlbumName, a.ReleaseDate, c.Id AS CompanyId, c.Name AS CompanyName
    FROM Albums a
    INNER JOIN Companies c ON a.CompanyId = c.Id";

                var result = await connection.QueryAsync(query);
                return View(result);
            }
        }

        // Симметричное внутреннее соединение без условия
        [HttpGet]
        public async Task<IActionResult> SymmetricInnerJoinWithoutCondition2()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT a.Id AS AlbumId, a.Name AS AlbumName, a.ReleaseDate, g.Id AS GroupId, g.Name AS GroupName
            FROM Albums a
            INNER JOIN Groups g ON a.GroupId = g.Id";

                var result = await connection.QueryAsync(query);
                return View(result);
            }
        }

        // Симметричное внутреннее соединение без условия
        [HttpGet]
        public async Task<IActionResult> SymmetricInnerJoinWithoutCondition3()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
    SELECT a.Id AS AlbumId, a.Name AS AlbumName, a.ReleaseDate, p.Price, s.Name AS StoreName
    FROM Albums a
    INNER JOIN Product p ON a.Id = p.AlbumId
    INNER JOIN Stores s ON p.StoreId = s.Id";

                var result = await connection.QueryAsync(query);
                return View(result);
            }
        }

        [HttpGet]
        public IActionResult LeftOuterJoin()
        {
            return View();
        }

        //левое внешнее соединение
        [HttpPost]
        public async Task<IActionResult> LeftOuterJoin(string companyName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT a.Id AS AlbumId, a.Name AS AlbumName, a.ReleaseDate, g.Id AS GroupId, g.Name AS GroupName, c.Id AS CompanyId, c.Name AS CompanyName
            FROM Albums a
            LEFT OUTER JOIN Groups g ON a.GroupId = g.Id
            LEFT OUTER JOIN Companies c ON a.CompanyId = c.Id
            WHERE (@CompanyName IS NULL OR c.Name LIKE '%' + @CompanyName + '%')";

                var result = await connection.QueryAsync(query, new { CompanyName = companyName });
                return View("LeftOuterJoinResult", result);
            }
        }

        // Правое внешнее соединение
        [HttpGet]
        public IActionResult RightOuterJoin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RightOuterJoin(string groupName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT a.Id AS AlbumId, a.Name AS AlbumName, a.ReleaseDate, g.Id AS GroupId, g.Name AS GroupName
            FROM Albums a
            RIGHT OUTER JOIN Groups g ON a.GroupId = g.Id
            WHERE (@GroupName IS NULL OR g.Name LIKE '%' + @GroupName + '%')";

                var result = await connection.QueryAsync(query, new { GroupName = groupName });
                return View("RightOuterJoinResult", result);
            }
        }

        [HttpGet]
        public IActionResult SubqueryLeftJoin()
        {
            return View();
        }

        // Запрос на запросе по принципу левого соединения
        [HttpPost]
        public async Task<IActionResult> SubqueryLeftJoin(DateTime orderDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT s.Id AS StoreId, s.Name AS StoreName
            FROM Stores s
            LEFT JOIN (
                SELECT DISTINCT p.StoreId
                FROM Orders o
                INNER JOIN Product p ON o.ProductId = p.Id
                WHERE CAST(o.OrderDate AS DATE) = CAST(@OrderDate AS DATE)
            ) op ON s.Id = op.StoreId
            WHERE op.StoreId IS NOT NULL";

                var result = await connection.QueryAsync(query, new { OrderDate = orderDate });
                return View("SubqueryLeftJoinResult", result);
            }
        }
    }
}
