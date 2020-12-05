using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsScore.Services.Interfaces
{
    public interface IRandomService
    {
        public int RandomNumber(int min, int max);

        public string RandomPassword(int size);
    }
}
