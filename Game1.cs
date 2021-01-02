using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Retro2D;

using static Retro2D.Node;

namespace Proto_00
{
    public partial class Game1 : Game
    {

        public Game1()
        {
            LoadConfig();

            _window.Setup(this, Mode.RETRO, "Proto_00", _screenW, _screenH, 2, 0, false, true, false);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();

            _window.Init(_fontMain);
            _window.SetFinalScreenSize(_finalScreenW, _finalScreenH);
            _window.SetScale(3);

            Player1 = _players[PLAYER1] = new Player(0, "Mugen");
            Player2 = _players[PLAYER2] = new Player(0, "Silver");
            Player3 = _players[PLAYER3] = new Player(0, "Zero");
            Player4 = _players[PLAYER4] = new Player(0, "Alpha");

            _controllers[PLAYER1] = new Controller()
                .AsMainController(Player1);

            new Controller()
                .SetButton(new Button((int)SNES.BUTTONS.UP, (int)Keys.Up))
                .SetButton(new Button((int)SNES.BUTTONS.DOWN, (int)Keys.Down))
                .SetButton(new Button((int)SNES.BUTTONS.LEFT, (int)Keys.Left))
                .SetButton(new Button((int)SNES.BUTTONS.RIGHT, (int)Keys.Right))

                .SetButton(new Button((int)SNES.BUTTONS.A, (int)Keys.RightAlt))
                .SetButton(new Button((int)SNES.BUTTONS.B, (int)Keys.Space))

                .SetButton(new Button((int)SNES.BUTTONS.START, (int)Keys.Enter))
                .SetButton(new Button((int)SNES.BUTTONS.SELECT, (int)Keys.Back))
                .AppendTo(_players[PLAYER1]);

            _controllers[PLAYER2] = new Controller()
                .AsMainController(Player2);

            new Controller()
                .SetButton(new Button((int)SNES.BUTTONS.UP, (int)Keys.Z))
                .SetButton(new Button((int)SNES.BUTTONS.DOWN, (int)Keys.S))
                .SetButton(new Button((int)SNES.BUTTONS.LEFT, (int)Keys.Q))
                .SetButton(new Button((int)SNES.BUTTONS.RIGHT, (int)Keys.D))

                .SetButton(new Button((int)SNES.BUTTONS.A, (int)Keys.D1))
                .SetButton(new Button((int)SNES.BUTTONS.B, (int)Keys.D2))
                .AppendTo(_players[PLAYER2]);

            _controllers[PLAYER3] =
            new Controller()
                .SetButton(new Button((int)SNES.BUTTONS.UP, (int)Keys.O))
                .SetButton(new Button((int)SNES.BUTTONS.DOWN, (int)Keys.L))
                .SetButton(new Button((int)SNES.BUTTONS.LEFT, (int)Keys.K))
                .SetButton(new Button((int)SNES.BUTTONS.RIGHT, (int)Keys.M))

                .SetButton(new Button((int)SNES.BUTTONS.A, (int)Keys.NumPad4))
                .SetButton(new Button((int)SNES.BUTTONS.B, (int)Keys.Space))
                //.AppendTo(_players[PLAYER3]);
                .AsMainController(Player3);

            _controllers[PLAYER4] =
            new Controller()
                .SetButton(new Button((int)SNES.BUTTONS.UP, (int)Keys.T))
                .SetButton(new Button((int)SNES.BUTTONS.DOWN, (int)Keys.G))
                .SetButton(new Button((int)SNES.BUTTONS.LEFT, (int)Keys.F))
                .SetButton(new Button((int)SNES.BUTTONS.RIGHT, (int)Keys.H))

                .SetButton(new Button((int)SNES.BUTTONS.A, (int)Keys.NumPad6))
                .SetButton(new Button((int)SNES.BUTTONS.B, (int)Keys.Space))
                //.AppendTo(_players[PLAYER4]);
                .AsMainController(Player4);


            // Load GamePad Setup from file
            LoadGamePadSetupFromFile(_pathGamePadSetup);


            _screenPlay = (ScreenPlay)new ScreenPlay(Content).Init();
            Screen.Init(_screenPlay);

            _camera.Position = new Vector2(_screenW/2, _screenH/2);
            _camera.Zoom = 1.0f;
            //cam.Zoom = 2.0f;
            //cam.Zoom = 0.5f;

            //_window._graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;

        }

        protected override void Update(GameTime gameTime)
        {
            _window.GetMouse(ref _relMouseX, ref _relMouseY, ref _mouseState);

            _mouseInput.Update(_relMouseX, _relMouseY, _mouseState.LeftButton == ButtonState.Pressed ? 1 : 0, _mouseState.ScrollWheelValue);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _window.UpdateStdWindowControl();

            if (Input.Button.OnePress("natif", Keyboard.GetState().IsKeyDown(Keys.D0))) _window.SetFinalScreenSize(_screenW, _screenH);
            if (Input.Button.OnePress("160x90", Keyboard.GetState().IsKeyDown(Keys.D1))) _window.SetFinalScreenSize(160, 90);
            if (Input.Button.OnePress("320x180", Keyboard.GetState().IsKeyDown(Keys.D2))) _window.SetFinalScreenSize(320, 180);
            if (Input.Button.OnePress("640x360", Keyboard.GetState().IsKeyDown(Keys.D3))) _window.SetFinalScreenSize(640, 360);
            if (Input.Button.OnePress("960x540", Keyboard.GetState().IsKeyDown(Keys.D4))) _window.SetFinalScreenSize(960, 540);
            if (Input.Button.OnePress("1280x720", Keyboard.GetState().IsKeyDown(Keys.D5))) _window.SetFinalScreenSize(1280, 720);
            if (Input.Button.OnePress("1600x900", Keyboard.GetState().IsKeyDown(Keys.D6))) _window.SetFinalScreenSize(1600, 900);
            if (Input.Button.OnePress("1920x1080", Keyboard.GetState().IsKeyDown(Keys.D7))) _window.SetFinalScreenSize(1920, 1080);

            _frameCounter.Update(gameTime);
               
            Window.Title = "Starter FPS :" + _frameCounter.Fps() + " FinalScreen : " + _window.FinalScreenW + " x " + _window.FinalScreenH + " Zoom X " + _window.Scale + "  Ratio :" + (float)_window.ScreenW / (float)_window.FinalScreenW;

            Screen.Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {

            // Set Target to MainRenderTarget !
            _window.SetRenderTarget(_window.NativeRenderTarget);
            _window.Batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, _camera.GetTransformation(_window._graphics.GraphicsDevice)); //, DepthStencilState.None, RasterizerState.CullCounterClockwise );
            // Draw something here !
            Screen.Render(_window.Batch);
            Retro2D.Draw.RightTopString(_window.Batch, Game1._fontMain, Game1._frameCounter.Fps(), Game1._screenW - 2, 2, Color.Gold);
            _window.Batch.End();


            _window.Batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp); //, DepthStencilState.None, RasterizerState.CullCounterClockwise );
            // Draw something here !
            //Retro2D.Draw.CenterStringXY(_window.Batch, _fontMain, "- HUD TEST -", 100, 100, Color.Yellow);
            //Retro2D.Draw.Sight(_window.Batch, Game1._relMouseX, Game1._relMouseY, Game1._screenW, Game1._screenH, Color.RoyalBlue, 1);

            _window.Batch.End();

            // Render MainTarget in FinalTarget
            _window.SetRenderTarget(_window.FinalRenderTarget);
            _window.Batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            _window.RenderMainTarget(Color.White);

            _window.Batch.End();

            // Flip to FinalRenderTarget ! 
            _window.SetRenderTarget(null);
            _window.Batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap);

            _window.RenderFinalTarget(Color.White);

            _window.Batch.End();


            base.Draw(gameTime);
        }
    }
}