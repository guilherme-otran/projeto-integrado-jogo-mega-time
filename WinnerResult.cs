﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_PI
{
    public class WinnerResult
    {
        public IEnumerable<BetNumber> winnerNumbers;
        public Team winnerTeam;
        public IDictionary<Int32, Decimal> awardValues
        {
            get;
            private set;
        }

        public WinnerResult()
        {
            this.awardValues = new Dictionary<Int32, Decimal>();
        }
    }
}
