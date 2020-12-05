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

        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        public string RandomPassword(int size)
        {
            var builder = new StringBuilder(size);

            for (var i = 0; i < size; i++)
            {
                var offset = _random.Next(0, 2) != 0 ? 'a' : 'A';
                const int lettersOffset = 26;
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return builder.ToString();
        }
    }
}
