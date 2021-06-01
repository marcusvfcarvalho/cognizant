using CognizantTest.Api.DB;
using CognizantTest.CompilerServices;
using CognizantTest.Data.DTO;
using CognizantTest.Data.Models;
using CognizantTest.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CognizantTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly CodeChallengeDBContext _codeChallengeDBContext;

        public TasksController(CodeChallengeDBContext codeChallengeDBContext)
        {
            _codeChallengeDBContext = codeChallengeDBContext;
        }

        // GET: api/<TasksController>
        [HttpGet]
        public async Task<IEnumerable<ChallengeTaskDto>> GetAsync()
        {
            return await _codeChallengeDBContext
                .ChallengeTasks
                .Select(task => new ChallengeTaskDto()
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    TestInputParameter = task.TestInputParameter,
                    TestOutputParameter = task.TestOutputParameter
                }).ToArrayAsync();
        }

        [HttpGet("top")]
        public async Task<IEnumerable<RankDto>> GetTopAsync()
        {
            return await _codeChallengeDBContext.ChallengeTaskSolutions
                 .GroupBy(c => c.Name)
                 .Select(cl => new RankDto
                 {
                     Name = cl.Key,
                     TestsTaken = cl.Count(),
                     SuccessfulTests = cl.Count(c => c.Success)
                 })
                 .OrderByDescending(x => x.SuccessfulTests)
                 .Take(3)
                 .ToArrayAsync();
        }

        [HttpPost("execute")]
        public async Task<IActionResult> PostTaskSolutionAsync([FromBody] ChallengeTaskSolutionViewModel taskSolution)
        {
            if (ModelState.IsValid)
            {
                var compiler = new Compiler();
                var challengeTask = _codeChallengeDBContext.ChallengeTasks.Find(taskSolution.TaskId);
                string[] inputParameters = null;
                if (challengeTask.TestInputParameter != null)
                {
                    inputParameters = challengeTask.TestInputParameter.Split(",");
                }

                var result = await compiler.ExecuteAsync(taskSolution.Code, inputParameters);

                if (result.Status == 0)
                {
                    bool finalResult = false;
                    if (result.Output == challengeTask.TestOutputParameter)
                    {
                        result = new ExecutionResult() { Status = ExecutionStatus.Success, Output = "SUCCESS!" };
                        finalResult = true;
                    }
                    else
                    {
                        result = new ExecutionResult() { Status = ExecutionStatus.ResultFailure, Output = "The code did not produce expected output." };
                    }

                    var challengeTaskSolution = new ChallengeTaskSolution()
                    {
                        TaskId = challengeTask.Id,
                        Name = taskSolution.Name,
                        Code = taskSolution.Code,
                        Success = finalResult
                    };

                    _codeChallengeDBContext.ChallengeTaskSolutions.Add(challengeTaskSolution);
                    _codeChallengeDBContext.SaveChanges();
                }

                return Ok(result);
            }
            else
            {
                return StatusCode(400);
            }
        }
    }
}