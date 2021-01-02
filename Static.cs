using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Retro2D;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Proto_00
{
    public partial class Game1  : Game
    {
        static public Camera2D _camera = new Camera2D();

        public const int PLAYER1 = 0;
        public const int PLAYER2 = 1;
        public const int PLAYER3 = 2;
        public const int PLAYER4 = 3;
        public const int MAX_PLAYER = 4;

        public static Player[] _players = new Player[MAX_PLAYER];
        // Alias for _players[]
        public static Player Player1 { get; private set; }
        public static Player Player2 { get; private set; }
        public static Player Player3 { get; private set; }
        public static Player Player4 { get; private set; }

        public static Controller[] _controllers = new Controller[MAX_PLAYER];

        public static string _pathGamePadSetup = "controllers_Setup.xml";


        Window _window = new Window();
        public static FrameCounter _frameCounter = new FrameCounter();
        public static MouseState _mouseState;

        static public Input.Mouse _mouseInput = new Input.Mouse();

        static public int _relMouseX;
        static public int _relMouseY;
        static public int _screenW = 1920;
        static public int _screenH = 1080;

        static public SpriteFont _fontMain;

        public int _finalScreenW = 1920;
        public int _finalScreenH = 1080;

        ScreenPlay _screenPlay;

        XmlReader _xmlReader;

        static public Style _style;

        void LoadConfig()
        {
            Console.WriteLine("READ XML FILE !");

            _xmlReader = XmlReader.Create("config.xml");

            while (_xmlReader.Read())
            {
                if (_xmlReader.NodeType == XmlNodeType.Element)
                {

                    if (_xmlReader.Name == "Screen")
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            string screenW = _xmlReader.GetAttribute("width");
                            string screenH = _xmlReader.GetAttribute("height");

                            Console.WriteLine("Screen = " + screenW + "," + screenH);

                            _finalScreenW = _screenW = int.Parse(screenW);
                            _finalScreenH = _screenH = int.Parse(screenH);

                        }

                    }

                }
            }
        }

        public static void SaveGamePadSetupToFile(string path)
        {
            List<Controller> controllers = new List<Controller>()
            {
                _controllers[PLAYER1],
                _controllers[PLAYER2],
                _controllers[PLAYER3],
                _controllers[PLAYER4]
            };

            FileIO.XmlSerialization.WriteToXmlFile(path, controllers);

            Console.WriteLine("GamePad File Setup Saved !");
        }

        public static void LoadGamePadSetupFromFile(string path)
        {
            List<Controller> controllers = FileIO.XmlSerialization.ReadFromXmlFile<List<Controller>>(path);

            for (int i = 0; i < MAX_PLAYER; i++)
            {
                _controllers[i].Copy(controllers[i]);
            }
            Console.WriteLine("GamePad File Setup Loaded !");
        }

        protected override void LoadContent()
        {
            _fontMain = Content.Load<SpriteFont>("Font/mainFont");

            _style = new Style() { _font = _fontMain, _color = Style.ColorValue.MakeColor(Color.BlueViolet), _horizontalAlign = Style.HorizontalAlign.Center, _verticalAlign = Style.VerticalAlign.Middle};
        }
        protected override void UnloadContent()
        {

        }
    }
}
