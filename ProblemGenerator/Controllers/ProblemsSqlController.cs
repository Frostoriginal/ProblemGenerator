using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProblemGenerator.Data;

namespace ProblemGenerator.Controllers;


[Route("problemsSQL")]
[ApiController]
public class ProblemsSqlController : Controller
{
    private readonly ProblemSQLContext _db;

    public ProblemsSqlController(ProblemSQLContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProblemSQL>>> GetProblems()
    {
        return (await _db.Problems.ToListAsync()).ToList();
    }
}


