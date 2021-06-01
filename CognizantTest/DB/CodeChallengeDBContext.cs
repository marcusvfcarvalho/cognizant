using CognizantTest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CognizantTest.Api.DB
{
    public class CodeChallengeDBContext : DbContext
    {
        public CodeChallengeDBContext(DbContextOptions<CodeChallengeDBContext> options) : base(options)
        {
        }

        public DbSet<ChallengeTask> ChallengeTasks { get; set; }
        public DbSet<ChallengeTaskSolution> ChallengeTaskSolutions { get; set; }
    }
}