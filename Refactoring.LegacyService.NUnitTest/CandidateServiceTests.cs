using System;
using NUnit.Framework;
using Refactoring.LegacyService;
using Moq;
using System.Threading.Tasks;
namespace Refactoring.LegacyService.Tests
{
    [TestFixture]
    public class CandidateServiceTests
    {
        private CandidateService _service;
        private readonly Mock<ICandidateCreditService> _moqCandidateCreditService;
        private readonly Mock<IPositionRepository> _moqPositionRepository;
        private readonly Mock<ICandidateDataAccessor> _moqCandidateDataAccessor;
        public CandidateServiceTests()
        {
            _moqCandidateCreditService = new Mock<ICandidateCreditService>();
            _moqPositionRepository = new Mock<IPositionRepository>();
            _moqCandidateDataAccessor = new Mock<ICandidateDataAccessor>();
            _service = new CandidateService(_moqPositionRepository.Object, _moqCandidateCreditService.Object, _moqCandidateDataAccessor.Object);
            _moqPositionRepository.Setup(r => r.GetById(1)).Returns(new Position { Id = 1, Name = "SecuritySpecialist", Status = PositionStatus.none });
            _moqPositionRepository.Setup(r => r.GetById(2)).Returns(new Position { Id = 2, Name = "FeatureDeveloper", Status = PositionStatus.none });
            _moqPositionRepository.Setup(r => r.GetById(3)).Returns(new Position { Id = 3, Name = "", Status = PositionStatus.none });

            _moqCandidateCreditService.Setup(r => r.GetCredit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(2000);

            _moqCandidateDataAccessor.Setup(da => da.AddCandidate(It.IsAny<Candidate>())).Returns(Task.FromResult(true));

        }
        /// <summary>
        /// For valid data, the result is unpredicable, as Position/CreditInfo is a blackbox.
        /// We have to predict result as dynamic depend of real position form database.
        /// </summary>
        /// <param name="firname"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="positionid"></param>
        [TestCase("TestF1", "TestF2", "test@hotmail.com", "2002-7-9", 3)]
        [TestCase("TestF1", "TestF2", "test@hotmail.com", "2002-7-9", 3)]
        [TestCase("TestF1", "TestF2", "test@hotmail.com", "2002-7-9", 3)]
        public async Task AddCandidate_ValidData(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            var result = await _service.AddCandidate(firname, surname, email, dateOfBirth, positionid);
            Assert.IsTrue(result);
        }

        [TestCase("", "TestF2", "test@hotmail.com", "2002-7-9", 3)]
        [TestCase(null, "TestF2", "test@hotmail.com", "2002-7-9", 3)]
        [TestCase("TestF1", "", "test@hotmail.com", "2002-7-9", 3)]
        [TestCase("TestF1", null, "test@hotmail.com", "2002-7-9", 3)]
        public async Task AddCandidate_Invalid_Names(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            var result = await _service.AddCandidate(firname, surname, email, dateOfBirth, positionid);
            Assert.AreEqual(result, false);
        }


        [TestCase("TestF1", "TestF2", "test@hotmailcom", "2002-7-9", 3)]
        [TestCase("TestF1", "TestF2", "testhotmailcom", "2002-7-9", 3)]
        [TestCase("TestF1", "TestF2", "", "2002-7-9", 3)]
        public async Task AddCandidate_InvalidEmail(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            var result = await _service.AddCandidate(firname, surname, email, dateOfBirth, positionid);
            Assert.IsFalse(result);

        }

        [TestCase("TestF1", "TestF2", "test@hotmail.com", "2005-7-9", 3)]
        public async Task AddCandidate_InValid_Age(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            var result = await _service.AddCandidate(firname, surname, email, dateOfBirth, positionid);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// SecuritySpecialist with good credit
        /// </summary>
        /// <param name="firname"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="positionid"></param>
        [TestCase("TestF1", "TestF2", "test@hotmail.com", "2002-7-9", 1)]
        public async Task AddCandidate_Valid_SS(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            _moqCandidateCreditService.Setup(r => r.GetCredit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(2000);
            var result = await _service.AddCandidate(firname, surname, email, dateOfBirth, positionid);
            Assert.IsTrue(result);

        }

        /// <summary>
        /// SecuritySpecialist with bad credit
        /// </summary>
        /// <param name="firname"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="positionid"></param>
        [TestCase("TestF1", "TestF2", "test@hotmail.com", "2002-7-9", 1)]
        public async Task AddCandidate_InValid_SS(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            _moqCandidateCreditService.Setup(r => r.GetCredit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(100);

            var result = await _service.AddCandidate(firname, surname, email, dateOfBirth, positionid);
            Assert.IsFalse(result);

        }

        /// <summary>
        /// FeatureDeveloper with good credit
        /// </summary>
        /// <param name="firname"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="positionid"></param>
        [TestCase("TestF1", "TestF2", "test@hotmail.com", "2002-7-9", 2)]
        public async Task AddCandidate_Valid_FD(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            _moqCandidateCreditService.Setup(r => r.GetCredit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(600);
            var result = await _service.AddCandidate(firname, surname, email, dateOfBirth, positionid);
            Assert.IsTrue(result);

        }
        /// <summary>
        /// FeatureDeveloper with bad credit
        /// </summary>
        /// <param name="firname"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="positionid"></param>
        [TestCase("TestF1", "TestF2", "test@hotmail.com", "2002-7-9", 2)]
        public async Task AddCandidate_InValid_FD(string firname, string surname, string email, DateTime dateOfBirth, int positionid)
        {
            _moqCandidateCreditService.Setup(r => r.GetCredit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(300);
            var result = await _service.AddCandidate(firname, surname, email, dateOfBirth, positionid);
            Assert.IsFalse(result);

        }
    }
}
