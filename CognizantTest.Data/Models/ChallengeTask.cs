using System.ComponentModel.DataAnnotations;

namespace CognizantTest.Data.Models
{
    public class ChallengeTask
    {
        public int Id { get; init; }

        [MaxLength(25)]
        [Required]
        public string Name { get; init; }

        [MaxLength(1024)]
        [Required]
        public string Description { get; init; }

        [MaxLength(1024)]
        public string TestInputParameter { get; init; }

        [MaxLength(1024)]
        [Required]
        public string TestOutputParameter { get; init; }
    }
}