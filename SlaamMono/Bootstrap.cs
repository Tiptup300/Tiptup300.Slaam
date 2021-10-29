﻿using Microsoft.Xna.Framework.Graphics;
using SimpleInjector;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using SlaamMono.Menus;
using SlaamMono.PlayerProfiles;
using SlaamMono.Resources;
using SlaamMono.Resources.Loading;
using System.Collections.Generic;

namespace SlaamMono
{
    public class Bootstrap
    {
        private Container _container;

        public Container BuildContainer(IResolver resolver)
        {
            _container = new Container();

            _container.RegisterInstance<IResolver>(resolver);
            register();
            registerComponents();
            registerScreens();
            registerResources();
            registerGameplay();

            return _container;
        }

        private void register()
        {
            _container.Register<IApp, SlaamGameApp>(Lifestyle.Singleton);
            _container.Register<ISlaamGame, SlaamGame>(Lifestyle.Singleton);
            _container.Register<ILoggingDevice, TextFileLoggingDevice>(Lifestyle.Singleton);
            _container.Register<ILogger, Logger>(Lifestyle.Singleton);
        }

        public void registerComponents()
        {
            _container.Register<IRenderGraph, RenderGraphManager>(Lifestyle.Singleton);
        }

        private void registerScreens()
        {
            _container.Register<IMainMenuScreen, MainMenuScreen>();
            _container.Register<CreditsScreen>();
            _container.Register<HighScoreScreen>();
            _container.Register<ProfileEditScreen>();
            _container.Register<SurvivalCharSelectScreen>();
            _container.Register<ClassicCharSelectScreen>();
            _container.Register<IScreenManager, ScreenManager>(Lifestyle.Singleton);
            _container.Register<ILogoScreen, LogoScreen>();
            _container.Register<IScreenFactory, ScreenFactory>(Lifestyle.Singleton);
        }

        private void registerResources()
        {
            _container.RegisterInstance(new ResourcesState());
            _container.Register<IResources, ResourceManager>(Lifestyle.Singleton);
            _container.Register<IFileLoader<Texture2D>, Texture2DLoader>(Lifestyle.Singleton);
            _container.Register<IWhitePixelResolver, WhitePixelResolver>(Lifestyle.Singleton);
            _container.Register<IFileLoader<string[]>, CommentedTextLineLoader>(Lifestyle.Singleton);
            _container.Register<IFileLoader<SpriteFont>, FontLoader>(Lifestyle.Singleton);
            _container.Register<IFileLoader<CachedTexture>, CachedTextureLoader>(Lifestyle.Singleton);
            _container.Register<IResourceLoader, ResourceLoader>(Lifestyle.Singleton);
        }

        private void registerGameplay()
        {
            _container.Register<PlayerColorResolver>(Lifestyle.Singleton);
        }
    }
}
