using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_OrgaoRegulador;

namespace Projeto_PI
{
    class GroupedBetRepository
    {
        private EndPoint endpoint;

        public GroupedBetRepository()
        {
            endpoint = new EndPoint();
        }

        public class BetNotFound : System.NullReferenceException { };

        public long storeGroupedBet(GroupedBet gb)
        {
            string toApi = gb.betNumbers().Select(num => num.ToString()).Aggregate((src, acc) => src + "," + acc);
            long result = endpoint.gravarAposta(toApi);

            if (result == 0)
            {
                throw new System.Exception("API returned abnormal result");
            }

            return result;
        }

        public void loadGroupedBet(GroupedBet gb, long protocol)
        {
            string betsString = endpoint.obterTodasDezenasApostadas(protocol);

            if (betsString == null)
            {
                throw new BetNotFound();
            }

            string[] bets = betsString.Split(',');
            foreach (string betString in bets)
            {
                int betNum = int.Parse(betString);
                gb.toggleBet(betNum);
            }
        }
    }
}
