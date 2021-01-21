using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace ZBlade
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public partial class ZuneBlade : Microsoft.Xna.Framework.DrawableGameComponent
	{
		#region Private/Internal Const/Static

		private const int RepeatDelayUpDown = 30;
		private const int RepeatDelayLeftRight = 30;
		private const int RepeatIntervalUpDown = 10;
		private const int RepeatIntervalLeftRight = 5;
		private const int MaxItemsOnScreen = 5;

		internal static ZuneBlade instance;

		internal static SpriteFont Font10
		{
			get { return instance.font_10; }
		}

		internal static SpriteFont Font12
		{
			get { return instance.font_12; }
		}

		internal static SpriteFont Font14
		{
			get { return instance.font_14; }
		}

		internal static MenuItemTree CurrentMenu
		{
			get { return instance.currentMenu; }
			set { instance.currentMenu = value; }
		}

		internal static Texture2D WhitePixel
		{
			get { return instance.whitePixel; }
		}

		internal static Texture2D ProgressbarOverlay
		{
			get { return instance.progressbarOverlay; }
		}

		#endregion

		#region Public Static

		/// <summary>
		/// Gets a reference to the ZuneBlade instance.
		/// </summary>
		public static ZuneBlade Instance
		{
			get { return instance; }
		}

		#endregion

		#region Private Fields

		private ContentManager content = null;

		private SpriteFont font_10;
		private SpriteFont font_12;
		private SpriteFont font_14;

        private Texture2D whitePixel;
		private Texture2D bgGlossTex;
        private Texture2D bladeGloss;
        private Texture2D bladeBorder;
		private Texture2D menuChoiceTex;
		private Texture2D menuHighlightTex;
		private Texture2D menuArrowTex;
		private Texture2D progressbarOverlay;
		private MenuItemTree currentMenu;
		//private BladeOrientation orientation = BladeOrientation.Portrait;

		private MenuItemTree topMenu;
		private BladeStatus status = BladeStatus.In;
		private float opacity = 1f;

		private Vector2 inPosition = new Vector2(0, 179);
        private Vector2 outPosition = Vector2.Zero;
		private Vector2 hiddenPosition = new Vector2(0, 240);
		private Matrix transformMatrix = Matrix.Identity;
		private Transition currentTransition;
		private Transition selectionFade = new Transition(new Vector2(0), new Vector2(255), TimeSpan.FromSeconds(0.25));
		private ZunePadInput input = new ZunePadInput();
		private int menuTopIndex;
		private int menuBottomIndex = MaxItemsOnScreen;
		private SpriteBatch batch;
		private int holdTime = 0;

        private int screenWidth;

        public int ScreenWidth
        {
            get { return screenWidth; }
            set {
				screenWidth = value;
				CurrentBlade.Width = screenWidth;
			}
        }

        private int screenHeight;

        public int ScreenHeight
        {
            get { return screenHeight; }
            set { 
				screenHeight = value; 
			}
        }


		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets information which shows in the bar at the bottom of the menu.
		/// </summary>
		public GameInfo CurrentGameInfo { get; set; }

        public Color BladeColor = new Color(61, 61, 61);
        public Color MenuColor = new Color(61, 61, 61);
        public Color ProgressBarColor = new Color(30, 86, 143);

        public IBlade CurrentBlade = new InfoBlade();

		/// <summary>
		/// Gets whether the menu is currently up or not.
		/// </summary>
		public bool IsActive
		{
			get { return status == BladeStatus.Out; }
		}

		/// <summary>
		/// Gets or sets the top level menu for the blade.
		/// </summary>
		public MenuItemTree TopMenu
		{
			get { return topMenu; }
			set
			{
				topMenu = value;
				currentMenu = topMenu;
			}
		}

		/// <summary>
		/// Gets/Sets whether the user can close the menu themselves with the "play" button.
		/// The default value is true.
		/// </summary>
		public bool UserCanCloseMenu { get; set; }

		/// <summary>
		/// Gets/Sets whether the user can move around the menu using the dpad.
		/// The default value is true.
		/// </summary>
		public bool UserCanNavigateMenu { get; set; }

		/// <summary>
		/// Gets or sets the desired closed state of the blade. Used for choosing
		/// whether the blade only gets smaller (the In state) or completely leaves
		/// the screen (the Hidden state). The default value is In.
		/// </summary>
		public BladeStatus ClosedStatus { get; set; }

		/// <summary>
		/// Gets or sets the status of the blade.
		/// </summary>
		public BladeStatus Status
		{
			get { return status; }
			set
			{
				status = value;
				switch (status)
				{
					case BladeStatus.Hidden:
						currentTransition = new Transition(
							currentTransition.Position,
							hiddenPosition,
							TimeSpan.FromSeconds(0.4));
						break;
					case BladeStatus.In:
						currentTransition = new Transition(
							currentTransition.Position,
							inPosition,
							TimeSpan.FromSeconds(0.4));
                        break;
                    case BladeStatus.Out:
                        currentTransition = new Transition(
                            currentTransition.Position,
                            outPosition,
                            TimeSpan.FromSeconds(0.4));
                        break;
                    case BladeStatus.KeyOut:
                        currentTransition = new Transition(
                            currentTransition.Position,
                            outPosition,
                            TimeSpan.FromSeconds(0.4));
                        break;

				}
			}
		}

		/// <summary>
		/// Gets or sets the opacity in the range of [0, 1].
		/// </summary>
		public float Opacity
		{
			get { return opacity; }
			set { opacity = MathHelper.Clamp(value, 0f, 1f); }
		}

		/// <summary>
		/// Gets whether the current transition has completed.
		/// </summary>
		public bool FinishedSlide 
		{ 
			get { return currentTransition.IsFinished(); } 
		}

		#endregion

		#region Private Properties

		

		private Vector3 BladeOffset
		{
			get
			{
				if (currentTransition == null)
					return Vector3.Zero;
				return new Vector3(currentTransition.Position.X, currentTransition.Position.Y, 0f);
			}
		}

		private int CurrentMenuItem
		{
			get { return currentMenu.CurrentIndex; }
			set { currentMenu.CurrentIndex = value; }
		}

		#endregion

		#region Constructor

		public ZuneBlade(Game game)
			: base(game)
		{
			if (instance != null)
				throw new Exception(
					"Only one ZuneBlade can be created per game. " + 
					"Please use the static Instance property if you " + 
					"want to access to the existing ZuneBlade instance.");

			instance = this;

			UserCanCloseMenu = false;
			UserCanNavigateMenu = true;

			ClosedStatus = BladeStatus.In;

			InfoBlade.BladeOutSetup = new BladeSetup("Back", "Close", "Select");
            InfoBlade.BladeInSetup = new BladeSetup("", "Menu", "");
            InfoBlade.BladeHiddenSetup = new BladeSetup("", "", "");
            InfoBlade.BladeKeyOutSetup = new BladeSetup("", "", "");

			currentTransition = new Transition(hiddenPosition, inPosition, TimeSpan.FromSeconds(1));
		}

		#endregion

		#region LoadContent

		protected override void LoadContent()
		{
#if ZUNE
			// todo
			//content = new ResourceContentManager(Game.Services, Properties.Resources__Zune_.ResourceManager);
#else
				// TODO 
			//content = new ResourceContentManager(Game.Services, Properties.Resources__Windows_.ResourceManager);
#endif
			content = Game.Content;

			font_10 = content.Load<SpriteFont>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\Segoe UI-10");
			font_12 = content.Load<SpriteFont>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\Segoe UI-12");
			font_14 = content.Load<SpriteFont>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\Segoe UI-14");

            bgGlossTex = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\bgBottom");
            bladeGloss = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\bladeGloss");
            bladeBorder = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\bladeBorder");
			menuChoiceTex = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\menuchoice");
			menuHighlightTex = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\menuhighlight");
			menuArrowTex = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\menuArrow");
			progressbarOverlay = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\progressbarOverlay");

            whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData<uint>(new uint[] {0xffffffff });

            Key1 = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\key1");
            Key2 = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\key2");
            Key6 = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\key6");
            TextBox = content.Load<Texture2D>(Directory.GetCurrentDirectory() + "\\Content\\bin\\DesktopGL\\textbox");

			batch = new SpriteBatch(GraphicsDevice);
			CalcMatrix();

			base.LoadContent();
		}

		#endregion

		#region Update

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			input.Update(gameTime);

            CurrentBlade.Update(gameTime);

           /* if (status == BladeStatus.In && UserCanOpenMenu && input.IsPressed(ZuneButtons.PlayPause))
                Status = BladeStatus.Out;

            else*/ if (status == BladeStatus.Out && currentMenu != null && currentMenu.Nodes.Count > 0)
            {
                UpdateMenus(gameTime);
                selectionFade.Update(gameTime.ElapsedGameTime);
            }
            else if (status == BladeStatus.KeyOut)
                UpdateKeyboard(gameTime);

                   if (currentTransition.Update(gameTime.ElapsedGameTime))
                       CalcMatrix();

		}

		private void UpdateMenus(GameTime gameTime)
		{
			bool nextItem = false;
			bool prevItem = false;

			if (UserCanNavigateMenu && FinishedSlide)
			{
				if (input.IsPressed(ZuneButtons.DPadUp))
				{
					holdTime = 0;
					prevItem = true;
					selectionFade.Reset();
				}
				else if (input.IsPressing(ZuneButtons.DPadUp))
				{
					holdTime++;
					if (holdTime > RepeatDelayUpDown && holdTime % RepeatIntervalUpDown == 0)
						prevItem = true;
				}

				else if (input.IsPressed(ZuneButtons.DPadDown))
				{
					holdTime = 0;
					nextItem = true;
					selectionFade.Reset();
				}
				else if (input.IsPressing(ZuneButtons.DPadDown))
				{
					holdTime++;
					if (holdTime > RepeatDelayUpDown && holdTime % RepeatIntervalUpDown == 0)
						nextItem = true;
				}

				else if (input.IsPressed(ZuneButtons.DPadLeft))
				{
					holdTime = 0;
					if (currentMenu.Nodes[CurrentMenuItem].DetectInput(ZuneButtons.DPadLeft))
						selectionFade.Reset();
				}
				else if (input.IsPressing(ZuneButtons.DPadLeft))
				{
					holdTime++;
					if (holdTime > RepeatDelayLeftRight && holdTime % RepeatIntervalLeftRight == 0)
						if (currentMenu.Nodes[CurrentMenuItem].DetectInput(ZuneButtons.DPadLeft))
							selectionFade.Reset();
				}

				else if (input.IsPressed(ZuneButtons.DPadRight))
				{
					holdTime = 0;
					if (currentMenu.Nodes[CurrentMenuItem].DetectInput(ZuneButtons.DPadRight))
						selectionFade.Reset();
				}
				else if (input.IsPressing(ZuneButtons.DPadRight))
				{
					holdTime++;
					if (holdTime > RepeatDelayLeftRight && holdTime % RepeatIntervalLeftRight == 0)
						if (currentMenu.Nodes[CurrentMenuItem].DetectInput(ZuneButtons.DPadRight))
							selectionFade.Reset();
				}

				else if (input.IsPressed(ZuneButtons.PadCenter))
				{

                    if (currentMenu.Nodes[CurrentMenuItem].DetectInput(ZuneButtons.PadCenter))
                        selectionFade.Reset();

                    menuTopIndex = 0;
                    FixMenu(1);
				}

				else
					holdTime = 0;
			}

			if (input.IsPressed(ZuneButtons.PlayPause) && UserCanCloseMenu)
			{
                Status = ClosedStatus;
			}
			
			if (input.IsPressed(ZuneButtons.Back))
			{
                if (currentMenu.Parent != null)
                {
                    GoBack();
                }
                else if (UserCanCloseMenu)
                    Status = ClosedStatus;
			}

			int change = (prevItem) ? -1 : (nextItem) ? 1 : 0;

			if (change != 0)
			{
                FixMenu(change);
			}

			for (int x = 0; x < currentMenu.Nodes.Count; x++)
			{
				currentMenu.Nodes[x].Update(gameTime, (x == CurrentMenuItem));
			}
		}

        public void GoBack()
        {
            currentMenu = currentMenu.Parent;
            FixMenu(0);
        }

        private void FixMenu(int change)
        {
            do
            {
                CurrentMenuItem += change;

                if (CurrentMenuItem < 0)
                {
                    CurrentMenuItem = currentMenu.Nodes.Count - 1;
                    menuTopIndex = CurrentMenuItem - MaxItemsOnScreen;
                    menuBottomIndex = menuTopIndex + MaxItemsOnScreen;
                }
                else if (CurrentMenuItem >= currentMenu.Nodes.Count)
                {
                    CurrentMenuItem = 0;
                    menuTopIndex = 0;
                    menuBottomIndex = MaxItemsOnScreen;
                }

                while (CurrentMenuItem >= menuBottomIndex)
                {
                    menuTopIndex++;
                    menuBottomIndex++;
                }

                while (CurrentMenuItem < menuTopIndex)
                {
                    menuTopIndex--;
                    menuBottomIndex--;
                }

                if (menuTopIndex < 0)
                {
                    menuTopIndex = 0;
                    menuBottomIndex = MaxItemsOnScreen;
                }
            } while (!CurrentMenu.Nodes[CurrentMenuItem].IsEnabled);
        }

		#endregion

		#region Draw

		public override void Draw(GameTime gameTime)
		{

			batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, transformMatrix);

			if ((status == BladeStatus.Out || !currentTransition.IsFinished()) && status != BladeStatus.KeyOut)
            {
                DrawBackground();
				DrawMenu();
				DrawGameInfo();
			}

            if (status == BladeStatus.KeyOut)
            {
                DrawBackground();
                DrawKeyboard();
            }

            DrawBlade();

			batch.End();
		}

		private void DrawBackground()
		{
			batch.Draw(
				whitePixel,
				new Rectangle(0,0,ScreenWidth,ScreenHeight),
				/*new Color(
                    (byte)(BladeColor.R * 0.63f),
                    (byte)(BladeColor.G * 0.63f),
                    (byte)(BladeColor.B * 0.63f),
                    (byte)(opacity * 255)));*/
                new Color(
                    (byte)(BladeColor.R),
                    (byte)(BladeColor.G),
                    (byte)(BladeColor.B),
                    (byte)(opacity * 255)));

            batch.Draw(
                bgGlossTex,
                new Vector2(0, ScreenHeight - bgGlossTex.Height),
                Color.White);
		}

        private void DrawBlade()
        {
            batch.Draw(
                whitePixel,
                new Rectangle(0, -CurrentBlade.Height, CurrentBlade.Width, CurrentBlade.Height),
                BladeColor);

            //batch.Draw(
            //    bladeBorder,
            //    new Vector2(0, 0),
            //    Color.White);

			batch.Draw(bladeBorder, new Rectangle(0, -CurrentBlade.Height, CurrentBlade.Width, CurrentBlade.Height), 
				null, Color.White,0f,Vector2.Zero,SpriteEffects.FlipVertically,0);


    //        batch.Draw(
    //            bladeBorder,
				//new Vector2(0,-CurrentBlade.Height-bladeBorder.Height),
    //            null,
    //            Color.White,
    //            0,
    //            Vector2.Zero,
    //            1f,
    //            SpriteEffects.FlipVertically,
    //            0);
            

            CurrentBlade.Draw(batch, new Vector2(0, -CurrentBlade.Height));

			//batch.Draw(
			//   bladeGloss, 
			//   new Vector2(0, -CurrentBlade.Height),
			//   Color.White);

			batch.Draw(bladeGloss, new Rectangle(0, -CurrentBlade.Height, CurrentBlade.Width, CurrentBlade.Height), Color.White);
        }

		private void DrawMenu()
		{
			if (currentMenu != null && currentMenu.Nodes.Count > 0)
			{
				for (int x = menuTopIndex; x < currentMenu.Nodes.Count && x < menuBottomIndex; x++)
				{
					int y = x - menuTopIndex;
                    Vector2 drawPos = new Vector2(0, -3 - (y * 6)) + (new Vector2(0, menuChoiceTex.Height) * y);
					batch.Draw(menuChoiceTex, drawPos, Color.White);

					currentMenu.Nodes[x].Draw(batch, drawPos + new Vector2(120, 14), (x == CurrentMenuItem));

					if (x == CurrentMenuItem)
						batch.Draw(
							menuHighlightTex,
							drawPos + new Vector2(0, 0),
							new Color((byte)255, (byte)255, (byte)255, (byte)selectionFade.Position.X));
				}

                    if (menuTopIndex > 0)
                    batch.Draw(menuArrowTex, new Vector2(198, 120), Color.White);
				else
                    batch.Draw(menuArrowTex, new Vector2(198, 120), Color.Black);


                    if (menuBottomIndex < currentMenu.Nodes.Count)
					batch.Draw(
						menuArrowTex,
                        new Vector2(214, ScreenHeight/2),
						null,
						Color.White,
						0f,
						Vector2.Zero,
						1f,
						SpriteEffects.FlipVertically,
						0);
				else
					batch.Draw(
						menuArrowTex,
                        new Vector2(214, ScreenHeight / 2),
						null,
						Color.Black,
						0f,
						Vector2.Zero,
						1f,
						SpriteEffects.FlipVertically,
						0);
			}
		}

		private void DrawGameInfo()
		{
			if (CurrentGameInfo != null)
			{
				Helpers.DrawString(
					batch,
					Font12,
					CurrentGameInfo.GameName,
					new Vector2(90, ScreenHeight-36),
					Vector2.Zero);

				Helpers.DrawString(
					batch,
					font_10,
					CurrentGameInfo.GameAuthor,
					new Vector2(90, ScreenHeight-20),
					Vector2.Zero);

				if (CurrentGameInfo.GameIcon != null)
				{
					batch.Draw(CurrentGameInfo.GameIcon, new Rectangle(64, 150, 23, 23), Color.Black);
					batch.Draw(CurrentGameInfo.GameIcon, new Rectangle(63, 149, 23, 23), Color.White);
				}
			}
		}

		#endregion

		#region Private Helper Methods

		private void CalcMatrix()
		{
			/*if (orientation == BladeOrientation.Portrait)
			{*/
				transformMatrix = Matrix.CreateTranslation(new Vector3(0, BladeOffset.Y + 140, 0));
			/*}
			else
			{
				transformMatrix = 
					Matrix.CreateRotationZ(MathHelper.PiOver2) * 
					Matrix.CreateTranslation(new Vector3(-BladeOffset.Y + 240, 0, 0));
			}*/
		}

		#endregion

        
	}
}