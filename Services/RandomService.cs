using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsScore.Services.Interfaces;

namespace CsScore.Services
{
    public class RandomService : IRandomService
    {
        private readonly Random _random = new Random();

        private const string StrSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVQXYZabcdefghijklmnopqrstuvwxyz";

        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        public string RandomPassword(int size)
        {

            return Nanoid.Nanoid.Generate(StrSet, size);
        }
    }
}
