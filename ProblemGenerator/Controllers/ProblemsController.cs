using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProblemGenerator.Data;

namespace ProblemGenerator.Controllers;


[Route("problems")]
[ApiController]
public class ProblemsController : Controller
{
    private readonly ProblemContext _db;

    public ProblemsController(ProblemContext db)
    {
    _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Problem>>> GetProblems()
    {
        return (await _db.Problems.ToListAsync()).ToList();
    }
}


