using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Retro2D;
using System.Collections.Generic;
using TiledSharp;

namespace Proto_00
{
    public class Level : Node
    {
        TileMap2D _tileMap2D;
        TileMapLayer _layer0;

        TmxMap _tmxMap;
        Texture2D _tileSet;

        Hero _hero;
        //Hero _hero2;

        //int _tileSize = 48;

        RectangleF _rectSelect = new RectangleF();

        int _mouseMapX;
        int _mouseMapY;
        public RectangleF RectSelect => _rectSelect;
        public int MouseTileX => _mouseMapX;
        public int MouseTileY => _mouseMapY;

        #region Debug
        //Vector2[] _polygon;
        //Vector2 A = new Vector2(200,200);
        //Vector2 B = new Vector2(200,100);
        //Vector2 C = new Vector2();
        //Vector2 D = new Vector2();
        //bool ContactTest = false;
        #endregion

        Input.Mouse _mouseLevel = new Input.Mouse();

        float _arenaSizeW = 1920;
        float _arenaSizeH = 1080;

        Camera _camera; 

        public Level(ContentManager content)
        {
            SetSize(Game1._screenW, Game1._screenH);

            _tmxMap = new TmxMap("Content/test_slope2D.tmx");
            _tileSet = content.Load<Texture2D>("Image/" + "TileSet_Slope2D");

            //_tileMap2D = new TileMap2D().Setup(new Rectangle(0, 0, Game1._screenW, Game1._screenH), Game1._screenW/_tileSize, Game1._screenH/_tileSize, _tileSize, _tileSize, 0, 0);
            _tileMap2D = new TileMap2D().Setup(new Rectangle(0, 0, Game1._screenW, Game1._screenH), _tmxMap.Width, _tmxMap.Height, _tmxMap.TileWidth, _tmxMap.TileHeight);

             _arenaSizeW = _tmxMap.Width * _tmxMap.TileWidth;
             _arenaSizeH = _tmxMap.Height * _tmxMap.TileHeight;

            //_layer0 = _tileMap2D.CreateLayer(null);
            _layer0 = TileMap2D.CreateLayer(_tmxMap, _tmxMap.Layers["Tile Layer 1"], _tileSet);

            System.Console.WriteLine("Layer0._map2D = " + _layer0._map2D);

            _hero = (Hero)new Hero(Game1.Player1, _layer0).Init().AppendTo(this);
            //_hero2 = (Hero)new Hero(Game1.Player2, _layer0._map2D).Init().AppendTo(this);

            //System.Console.WriteLine("------> "+_tmxMap.Tilesets[0].Tiles[2].ObjectGroups[0].Objects[0].Points);


            // debug
            //GenerateMap();

            _camera = new Camera();

            _camera.SetView(new RectangleF(0, 0, Game1._screenW, Game1._screenH));
            _camera.SetZone(new RectangleF(0, 0, Game1._screenW / 6, Game1._screenH / 6));

            //_camera.SetLimit(new RectangleF(0, 0, _arenaSizeW, _arenaSizeH));
            _camera.SetLimit(new RectangleF(0, 0, _arenaSizeW, _arenaSizeH));
            _camera.SetPosition(new Vector2(_arenaSizeW / 2 + _layer0._map2D._tileW / 2, _arenaSizeH / 2 + _layer0._map2D._tileH * 2));
        }

        public void GenerateMap()
        {
            _layer0._map2D.FillObject2D<TileMap>();

            for (int i = 0; i < _layer0._map2D._mapW; i++)
            {
                SetTile(i, _layer0._map2D._mapH - 1);
                SetTile(i, 0);
            }
            for (int i = 0; i < _layer0._map2D._mapH; i++)
            {
                SetTile(_layer0._map2D._mapW - 1, i);
                SetTile(0, i);
            }
            for (int i = 0; i < 200; i++)
            {
                int x = Misc.Rng.Next(1, _layer0._map2D._mapW - 1);
                int y = Misc.Rng.Next(1, _layer0._map2D._mapH - 1);
                SetTile(x, y);
            }
        }

        public void SetTile(int x, int y)
        {
            Tile tm = _layer0._map2D.Get(x, y);

            if (null != tm)
            {
                tm._rect.Width = _layer0._map2D._tileW;
                tm._rect.Height = _layer0._map2D._tileH;
                tm._isCollidable = !tm._isCollidable;
                tm.SetPassLevel(3);
            }
        }

        public override Node Init()
        {

            InitChilds();

            _hero.SetX(80).SetY(80);
            //_hero2.SetX(Position.CENTER).SetY(200);

            Game1._camera._position.X = Game1._screenW / 2;
            Game1._camera._position.Y = Game1._screenH / 2;
            Game1._camera.Rotation = 0f;
            Game1._camera.Zoom = 1f;

            return base.Init();
        }

        public override Node Update(GameTime gameTime)
        {
            // Debug test
            if (_hero._x < 1920)
                _camera.SetLimit(new RectangleF(0, 0, 1920, _arenaSizeH));
            else
                _camera.SetLimit(new RectangleF(1920-48, 0, 640, _arenaSizeH));


            _camera.Update(_hero._x, _hero._y, 10);
            SetPosition(_camera.X, _camera.Y);

            _mouseLevel.Update(Game1._relMouseX, Game1._relMouseY, Game1._mouseState.LeftButton == ButtonState.Pressed ? 1 : 0, Game1._mouseState.ScrollWheelValue, (int)-_camera.X, (int)-_camera.Y);

            _mouseMapX = _mouseLevel._x / _layer0._map2D._tileW;
            _mouseMapY = _mouseLevel._y / _layer0._map2D._tileH;

            _rectSelect.X = _mouseMapX * _layer0._map2D._tileW;
            _rectSelect.Y = _mouseMapY * _layer0._map2D._tileH;
            _rectSelect.Width = _layer0._map2D._tileW;
            _rectSelect.Height = _layer0._map2D._tileH;


            if (Keyboard.GetState().IsKeyDown(Keys.I)) Game1._camera._position.Y += -2;
            if (Keyboard.GetState().IsKeyDown(Keys.K)) Game1._camera._position.Y += 2;
            if (Keyboard.GetState().IsKeyDown(Keys.J)) Game1._camera._position.X += -2;
            if (Keyboard.GetState().IsKeyDown(Keys.L)) Game1._camera._position.X += 2;

            if (Keyboard.GetState().IsKeyDown(Keys.Home)) Game1._camera.Zoom += .01f;
            if (Keyboard.GetState().IsKeyDown(Keys.End)) Game1._camera.Zoom += -.01f;

            if (Keyboard.GetState().IsKeyDown(Keys.PageUp)) Game1._camera.Rotation += .01f;
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown)) Game1._camera.Rotation += -.01f;

            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                //GenerateMap();
                Init();
            }

            UpdateChilds(gameTime);

            if (_mouseLevel._onClick)
                SetTile(_mouseMapX, _mouseMapY);


            //C.X = Game1._mouseInput.AbsX;
            //C.Y = Game1._mouseInput.AbsY;
            //D = Collision2D.LineLineIntersection(new Line(A, B), new Line(C, new Vector2(C.X + 100, C.Y)), out ContactTest );

            return base.Update(gameTime);
        }

        public override Node Render(SpriteBatch batch)
        {
            Draw.Grid(batch, _camera.X, _camera.Y, _layer0._map2D._mapW * _layer0._map2D._tileW, _layer0._map2D._mapH * _layer0._map2D._tileH, _layer0._map2D._tileW, _layer0._map2D._tileH, Color.Red * .6f);

            _tileMap2D.Render(batch, _layer0, Color.White, (int)_camera.X, (int)_camera.Y, 1f, false);

            // Debug
            //_tileMap2D.RenderPolygonCollision(batch, _layer0, Color.MonoGameOrange, 1f, (int)_camera.X, (int)_camera.Y, 2f, default, 2f);

            //Draw.Rectangle(batch, _rectSelect, Color.Yellow);
            //Draw.LeftTopString(batch, Game1._fontMain, _layer0._map2D.Get(_mouseMapX, _mouseMapY).ToString(), _rectSelect.X, _rectSelect.Y, Color.MonoGameOrange);
            //Draw.Polygon(batch, new Vector2[] { new Vector2(100, 40), new Vector2(140, 50), new Vector2(140, 80), new Vector2(100, 80) }, Color.White, 2);

            //Draw.Line(batch, A, B, Color.LightBlue, 2f);
            //Draw.Line(batch, C, new Vector2(C.X + 100, C.Y), Color.OrangeRed, 2f);
            //if  (ContactTest)
            //    Draw.Point(batch, D, 8f, Color.LightGreen);


            Draw.RightTopString(batch, Game1._fontMain, Game1._frameCounter.Fps(), Game1._screenW - 10, 10, Color.Red);

            RenderChilds(batch);

            return base.Render(batch);
        }

    }
}
