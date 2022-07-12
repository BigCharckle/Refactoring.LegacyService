using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Refactoring.LegacyService
{
    public interface ICandidateService
    {
        public Task<bool> AddCandidate(string firname, string surname, string email, DateTime dateOfBirth, int positionid);
    }
}
