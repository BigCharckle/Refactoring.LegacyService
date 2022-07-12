namespace Refactoring.LegacyService
{
    using System;

    public class Candidate
    {
        public Position Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public bool RequireCreditCheck { get; set; }
        public int Credit { get; set; }
    }
}
