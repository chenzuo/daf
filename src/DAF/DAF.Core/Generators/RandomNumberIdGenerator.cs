using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Generators
{
    public class RandomNumberIdGenerator : IIdGenerator
    {
        private IRandomTextGenerator generator;

        public RandomNumberIdGenerator(IRandomTextGenerator generator)
        {
            this.generator = generator;
        }

        public string NewId()
        {
            return generator.Generate("1234567890", 8);
        }
    }
}
