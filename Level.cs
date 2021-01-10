using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Retro2D;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Proto_00
{
    public class Level : Node
    {
        TileMap2D _tileMap2D;
        TileMapLayer _layer0;

        //TileMap2D _tileMap2DX;
        //TileMapLayer _layerX;

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

        static public List<Point> DrawLine(SpriteBatch batch, int x0, int y0, int x1, int y1, int tileW, int tileH, Color color, Point offset)
        {
            List<Point> listTile = new List<Point>();

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;

            int oldMapX = 0;
            int oldMapY = 0;

            int mapX = 0;
            int mapY = 0;

            int nbTile = 0;

            for (; ; )
            {
                Draw.PutPixel(batch, x0 + offset.X, y0 + offset.Y, color);

                oldMapX = mapX;
                oldMapY = mapY;
                
                mapX = (x0/ tileW) * tileW; 
                mapY = (y0/ tileH) * tileH;

                if (oldMapX != mapX || oldMapY != mapY)
                {
                    listTile.Add(new Point(mapX, mapY));

                    Draw.Rectangle(batch, new Rectangle(mapX + offset.X, mapY + offset.Y, tileW, tileH), color);
                    Draw.CenterStringXY(batch, Game1._fontMain, nbTile.ToString(), mapX + tileW/2 + offset.X, mapY + tileH/2 + offset.Y, color);

                    ++nbTile;
                }


                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }


            return listTile;
        }

        public Level(ContentManager content)
        {
            SetSize(Game1._screenW, Game1._screenH);

            _tmxMap = new TmxMap("Content/test_slope2D.tmx");
            _tileSet = content.Load<Texture2D>("Image/" + "TileSet_Slope2D");

            //_tileMap2DX = new TileMap2D().Setup(new Rectangle(0, 0, Game1._screenW, Game1._screenH), _tmxMap.Width, _tmxMap.Height, _tmxMap.TileWidth, _tmxMap.TileHeight, 0, 0);
            //_layerX = _tileMap2DX.CreateLayer(null);

            _tileMap2D = new TileMap2D().Setup(new Rectangle(0, 0, Game1._screenW, Game1._screenH), _tmxMap.Width, _tmxMap.Height, _tmxMap.TileWidth, _tmxMap.TileHeight);

             _arenaSizeW = _tmxMap.Width * _tmxMap.TileWidth;
             _arenaSizeH = _tmxMap.Height * _tmxMap.TileHeight;

            _layer0 = TileMap2D.CreateLayer(_tmxMap, _tmxMap.Layers["Tile Layer 1"], _tileSet);

            System.Console.WriteLine("Layer0._map2D = " + _layer0._map2D);


            //System.Console.WriteLine("------> "+_tmxMap.Tilesets[0].Tiles[2].ObjectGroups[0].Objects[0].Points);


            // debug
            //GenerateMap();

            _camera = new Camera();

            _camera.SetView(new RectangleF(0, 0, Game1._screenW, Game1._screenH));
            _camera.SetZone(new RectangleF(0, 0, Game1._screenW / 20, Game1._screenH / 40));

            //_camera.SetLimit(new RectangleF(0, 0, _arenaSizeW, _arenaSizeH));
            _camera.SetLimit(new RectangleF(0, 0, _arenaSizeW, _arenaSizeH));
            _camera.SetPosition(new Vector2(_arenaSizeW / 2 + _layer0._map2D._tileW / 2, _arenaSizeH / 2 + _layer0._map2D._tileH * 2));


            //for (int i = 0; i < 400; ++i)
            //{
            //    new Enemy0(_layer0).SetPosition(Misc.Rng.Next(80, (int)_arenaSizeW - 80), 80).AppendTo(this);
            //}

            _hero = (Hero)new Hero(Game1.Player1, _layer0, _mouseLevel).Init().AppendTo(this).SetPosition(80,80);
            //_hero2 = (Hero)new Hero(Game1.Player2, _layer0._map2D).Init().AppendTo(this);
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

            //_hero.SetX(80).SetY(80);
            //_hero2.SetX(Position.CENTER).SetY(200);

            Game1._camera._position.X = Game1._screenW / 2;
            Game1._camera._position.Y = Game1._screenH / 2;
            Game1._camera.Rotation = 0f;
            Game1._camera.Zoom = 1f;


            foreach(var enemy in GroupOf(UID.Get<Enemy0>()))
            {
                enemy.SetPosition(Misc.Rng.Next(80, (int)_arenaSizeW - 80), 80);
            }

            return base.Init();
        }

        public override Node Update(GameTime gameTime)
        {
            // Debug test
            if (_hero._x < 1920)
                _camera.SetLimit(new RectangleF(0, 0, 1920, _arenaSizeH));
            else
                _camera.SetLimit(new RectangleF(1920-48, 0, 640, _arenaSizeH));


            _camera.Update(_hero._x, _hero._y, 10, 10);
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
            batch.GraphicsDevice.Clear(new Color(0, 20, 20));

            Draw.Grid(batch, _camera.X, _camera.Y, _layer0._map2D._mapW * _layer0._map2D._tileW, _layer0._map2D._mapH * _layer0._map2D._tileH, _layer0._map2D._tileW, _layer0._map2D._tileH, Color.Green * .6f);

            _tileMap2D.Render(batch, _layer0, Color.White, (int)_camera.X, (int)_camera.Y, 1f, false);

            //SuperCoverLine(batch, new Point((int)_hero._x, (int)_hero._y), new Point(_hero.MapX, _hero.MapY), new Point(_mouseMapX, _mouseMapY), _layer0._map2D._tileW, _layer0._map2D._tileH, Color.BlueViolet);

            // Debug
            //_tileMap2D.RenderPolygonCollision(batch, _layer0, Color.MonoGameOrange, 1f, (int)_camera.X, (int)_camera.Y, 2f, default, 2f);

            //Draw.Rectangle(batch, _rectSelect, Color.Yellow);
            //Draw.LeftTopString(batch, Game1._fontMain, _layer0._map2D.Get(_mouseMapX, _mouseMapY).ToString(), _rectSelect.X, _rectSelect.Y, Color.MonoGameOrange);
            //Draw.Polygon(batch, new Vector2[] { new Vector2(100, 40), new Vector2(140, 50), new Vector2(140, 80), new Vector2(100, 80) }, Color.White, 2);

            //Draw.Line(batch, A, B, Color.LightBlue, 2f);
            //Draw.Line(batch, C, new Vector2(C.X + 100, C.Y), Color.OrangeRed, 2f);
            //if  (ContactTest)
            //    Draw.Point(batch, D, 8f, Color.LightGreen);

            RenderChilds(batch);

            return base.Render(batch);
        }

    }
}
