namespace CognizantTest.Data.DTO
{
    public record ChallengeTaskDto
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }

        public string TestInputParameter { get; init; }

        public string TestOutputParameter { get; init; }
    }
}