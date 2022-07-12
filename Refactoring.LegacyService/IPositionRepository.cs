using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring.LegacyService
{
    public interface IPositionRepository
    {
        Position GetById(int id);
    }
}
