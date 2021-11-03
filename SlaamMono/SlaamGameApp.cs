using Microsoft.Xna.Framework;
using SlaamMono.Library;
using SlaamMono.Library.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono
{
    public class SlaamGameApp : IApp
    {
        private readonly ILogger _logger;
        private readonly IGraphicsConfigurer _graphicsConfigurer;
        private readonly ISlaamGame _slaamGame;

        public SlaamGameApp(ISlaamGame slaamGame, ILogger logger, IGraphicsConfigurer graphicsConfigurer)
        {
            _slaamGame = slaamGame;
            _logger = logger;
            _graphicsConfigurer = graphicsConfigurer;
        }

        public void Run()
        {
            startLog();
            configureGraphics();
            addComponentsToGame();
            runGame();
        }

        private void configureGraphics()
        {
            _graphicsConfigurer.ConfigureGraphics();
        }

        private void addComponentsToGame()
        {
        }

        private void runGame()
        {
            try
            {
                _slaamGame.Run();
            }
            catch (Exception e)
            {
                logError(e);
            }
            finally
            {
                _logger.End();
            }
        }

        private void startLog()
        {
            _logger.Begin();
            _logger.Log("=======================================");
            _logger.Log("Slaam! - Logfile (for errors)");
            _logger.Log(" Created by Tiptup300");
            _logger.Log("=======================================");
            _logger.Log("");
            _logger.Log("Program executed;");
            _logger.Log("XNA Starting...");
        }

        private void logError(Exception e)
        {
            _logger.Log("Game Failed With Exit Message: ");
            _logger.Log(e.Message);
            if (e.InnerException != null)
            {
                _logger.Log(" --- INNER Exception --- ");
                _logger.Log(e.InnerException.ToString());
            }
            _logger.Log(" --- STACK TRACE --- ");
            _logger.Log(e.StackTrace);
        }
    }
}
