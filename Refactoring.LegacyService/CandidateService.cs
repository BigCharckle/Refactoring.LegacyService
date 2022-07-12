using System.Collections.Generic;
using System.Threading.Tasks;
namespace Refactoring.LegacyService
{
    using System;

    public class CandidateService: ICandidateService
    {
        private IPositionRepository _positionRepository;
        private ICandidateCreditService _candidateCreditService;
        private ICandidateDataAccessor _candidateDataAccessor;
        public CandidateService(IPositionRepository positionRepository, ICandidateCreditService candidateCreditService, ICandidateDataAccessor candidateDataAccessor)
        {
            _positionRepository = positionRepository;
            _candidateCreditService = candidateCreditService;
            _candidateDataAccessor = candidateDataAccessor;
        }

        public async Task<bool> AddCandidate(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            if (age < 18)
            {
                return false;
            }

            //var positionRepo = new PositionRepository();
            var position = _positionRepository.GetById(positionid);

            var candidate = new Candidate
            {
                Position = position,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            if (position.Name == "SecuritySpecialist")
            {
                // Do credit check and half credit
                candidate.RequireCreditCheck = true;

                var credit = _candidateCreditService.GetCredit(candidate.Firstname, candidate.Surname, candidate.DateOfBirth);
                credit = credit / 2;
                candidate.Credit = credit;

            }
            else if (position.Name == "FeatureDeveloper")
            {
                // Do credit check
                candidate.RequireCreditCheck = true;
                var credit = _candidateCreditService.GetCredit(candidate.Firstname, candidate.Surname, candidate.DateOfBirth);
                candidate.Credit = credit;
            }
            else
            {
                // No credit check
                candidate.RequireCreditCheck = false;
            }

            if (candidate.RequireCreditCheck && candidate.Credit < 500)
            {
                return false;
            }

            await _candidateDataAccessor.AddCandidate(candidate);

            return true;
        }
    }
}
