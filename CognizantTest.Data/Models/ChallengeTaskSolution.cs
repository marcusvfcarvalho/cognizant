using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CognizantTest.Data.Models
{
    public class ChallengeTaskSolution
    {
        public int Id { get; set; }

        [MaxLength(25)]
        [Required]
        public string Name { get; set; }

        [Required]
        [ForeignKey("ChallengeTask")]
        public int TaskId { get; set; }

        public ChallengeTask ChallengeTask { get; set; }

        [Required]
        [MaxLength(20000)]
        public string Code { get; set; }

        [Required]
        public bool Success { get; set; }
    }
}