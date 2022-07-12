using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Refactoring.LegacyService
{
    public class CandidateDataAccessor: ICandidateDataAccessor
    {
        public async Task<bool> AddCandidate(Candidate candidate)
        {
            await CandidateDataAccess.AddCandidate(candidate);
            return true;
        }
    }
}
