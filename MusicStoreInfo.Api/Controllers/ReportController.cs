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

        //Итоговый запрос без условия(количество магазинов по типам собственности)
        [HttpGet]
        public async Task<IActionResult> StoresByOwnershipType()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT
                        ot.Name AS OwnershipTypeName,
                        COUNT(s.Id) AS StoreCount
                    FROM
                        dbo.Stores s
                        INNER JOIN dbo.OwnershipTypes ot ON s.OwnershipTypeId = ot.Id
                    GROUP BY
                        ot.Name";

                var result = await connection.QueryAsync(query);
                return View(result);
            }
        }


        //Итоговый запрос с условием на данные (Количество альбомов по датам с количеством песен больше указанного числа)
        [HttpGet]
        public IActionResult AlbumsBySongCount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AlbumsBySongCount(int minSongCount)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT
                YEAR(a.ReleaseDate) AS ReleaseYear,
                COUNT(a.Id) AS AlbumCount
            FROM
                Albums a
            WHERE
                a.SongsCount > @MinSongCount
            GROUP BY
                YEAR(a.ReleaseDate)
            ORDER BY
                ReleaseYear";

                var result = await connection.QueryAsync(query, new { MinSongCount = minSongCount });
                return View("AlbumsBySongCountResult", result);
            }
        }


        //Итоговый запрос с условием на группы( Группы с суммарным количеством песен за всё время больше указанного числа)
        [HttpGet]
        public IActionResult GroupsByTotalSongCount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GroupsByTotalSongCount(int minTotalSongs)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT
                        g.Name AS GroupName,
                        SUM(a.SongsCount) AS TotalSongs
                    FROM
                        Groups g
                        INNER JOIN Albums a ON g.Id = a.GroupId
                    GROUP BY
                        g.Name
                    HAVING
                        SUM(a.SongsCount) > @MinTotalSongs";

                var result = await connection.QueryAsync(query, new { MinTotalSongs = minTotalSongs });
                return View("GroupsByTotalSongCountResult", result);
            }
        }

        //Итоговый запрос с условием на группы и данные( Группы и данные с суммарным количеством песен за определённый промежуток времени больше указанного числа)
        [HttpGet]
        public IActionResult GroupsBySongCountAndDateRange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GroupsBySongCountAndDateRange(int minTotalSongs, DateTime startDate, DateTime finishDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT
                g.Name AS GroupName,
                SUM(a.SongsCount) AS TotalSongs
            FROM
                Groups g
                INNER JOIN Albums a ON g.Id = a.GroupId
            WHERE
                a.ReleaseDate BETWEEN @StartDate AND @FinishDate
            GROUP BY
                g.Name
            HAVING
                SUM(a.SongsCount) > @MinTotalSongs";

                var result = await connection.QueryAsync(query, new { MinTotalSongs = minTotalSongs, StartDate = startDate, FinishDate = finishDate });
                return View("GroupsBySongCountAndDateRangeResult", result);
            }
        }
        //Запрос на запросе по принципу итогового запроса (количество альбомов с минимальным количеством песен для каждой группы)
        [HttpGet]
        public async Task<IActionResult> AlbumsByMinSongs()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                SELECT
                    g.Name AS GroupName,
                    COUNT(*) AS AlbumsCount
                FROM
                    dbo.Albums a
                    INNER JOIN dbo.Groups g ON a.GroupId = g.Id
                WHERE
                    a.SongsCount = (
                        SELECT MIN(a2.SongsCount) AS MinSongsCount
                        FROM dbo.Albums a2
                        WHERE a2.GroupId = a.GroupId
                    )
                GROUP BY
                    g.Name";

                var result = await connection.QueryAsync(query);
                return View("AlbumsByMinSongs", result);
            }
        }

        //Запрос с подзапросом (группы, которые выпустили альбомы в определённом диапазоне дат)
        [HttpGet]
        public IActionResult GroupsByAlbumDateRange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GroupsByAlbumDateRange(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                SELECT
                    g.Name AS GroupName
                FROM
                    Groups g
                WHERE
                    g.Id IN (
                        SELECT a.GroupId
                        FROM Albums a
                        WHERE a.ReleaseDate BETWEEN @StartDate AND @EndDate
                    )";

                var result = await connection.QueryAsync(query, new { StartDate = startDate, EndDate = endDate });
                return View("GroupsByAlbumDateRangeResult", result);
            }
        }

    }
}
