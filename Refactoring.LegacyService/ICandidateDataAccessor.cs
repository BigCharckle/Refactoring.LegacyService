using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Refactoring.LegacyService
{
    public interface ICandidateDataAccessor
    {
        public Task<bool> AddCandidate(Candidate candidate);
    }
}
