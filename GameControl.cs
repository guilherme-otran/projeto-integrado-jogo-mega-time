using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_PI
{
    class GameControl
    {
        private BetNumberRepository betRepo = new BetNumberRepository();
        private GameRepository gameRepo;
        
        public GameValidator gameValidator { get; private set; }
        public Game game { get; private set; }

        public class GameNotFound : System.NullReferenceException { };

        public GameControl()
        {
            // Use same betRepo repository for everything
            // Prevent equality problems.
            // Prevent large memory usage.
            this.gameRepo = new GameRepository(betRepo);

            this.game = new Game();
            this.gameValidator = new GameValidator(game);
        }
        
        public void AddBet(string betNumber)
        {
            BetNumber bet = betRepo.find(betNumber);
            game.AddBetNumber(bet);
        }

        public void RemoveBet(string betNumber)
        {
            BetNumber bet = betRepo.find(betNumber);
            game.RemoveBetNumber(bet);
        }

        public bool BetIsChoiced(string betNumber)
        {
            return game.bets.Any(bet => bet.number.Equals(betNumber));
        }

        public void Reset()
        {
            game.Reset();
        }

        public bool storeGame()
        {
            if (!gameValidator.valid())
                return false;
            
            gameRepo.storeGame(game);
            return true;
        }

        public IEnumerable<string> gameErrors()
        {
            gameValidator.valid();
            return gameValidator.errors;
        }

        public Decimal gamePrice()
        {
            return game.Price();
        }

        public void LoadGame(string protocolStr)
        {
            long protocol;
            Reset();

            if (!long.TryParse(protocolStr, out protocol))
            {
                throw new GameControl.GameNotFound();
            }

            try
            {
                gameRepo.loadToGame(game, protocol);
            }
            catch (GameRepository.GameNotFound)
            {
                throw new GameControl.GameNotFound();
            }
        }
    }
}
