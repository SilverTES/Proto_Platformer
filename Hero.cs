using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Retro2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto_00
{
    public class Hero : MapActor
    {

        bool _showContactPoint = true;

        public bool IS_CONTROLL = true;

        public bool IS_B_SELECT = false;
        public bool IS_B_START = false;

        public bool IS_B_UP = false;
        public bool IS_B_DOWN = false;
        public bool IS_B_LEFT = false;
        public bool IS_B_RIGHT = false;
        public bool IS_B_L = false;
        public bool IS_B_R = false;
        public bool IS_B_A = false;
        public bool IS_B_B = false;
        public bool IS_B_X = false;
        public bool IS_B_Y = false;

        public bool IS_PUSH_B_SELECT = false;
        public bool IS_PUSH_B_START = false;

        public bool IS_PUSH_B_UP = false;
        public bool IS_PUSH_B_DOWN = false;
        public bool IS_PUSH_B_LEFT = false;
        public bool IS_PUSH_B_RIGHT = false;
        public bool IS_PUSH_B_L = false;
        public bool IS_PUSH_B_R = false;
        public bool IS_PUSH_B_A = false;
        public bool IS_PUSH_B_B = false;
        public bool IS_PUSH_B_X = false;
        public bool IS_PUSH_B_Y = false;

        public bool ON_B_SELECT = false;
        public bool ON_B_START = false;

        public bool ON_B_UP = false;
        public bool ON_B_DOWN = false;
        public bool ON_B_LEFT = false;
        public bool ON_B_RIGHT = false;
        public bool ON_B_L = false;
        public bool ON_B_R = false;
        public bool ON_B_A = false;
        public bool ON_B_B = false;
        public bool ON_B_X = false;
        public bool ON_B_Y = false;

        // Button Jump
        bool IS_B_JUMP = false;
        bool ON_B_JUMP = false;

        Player _player;

        public Hero(Player player, TileMapLayer tileMapLayer) : base(tileMapLayer)
        {
            _player = player;

            SetSize(24, 32);
            SetPivot(12, 16);

            // Create Collide Point of Hero
            AddPoint((int)HotPoints.UL, new Vector2(-12, -20));
            AddPoint((int)HotPoints.UR, new Vector2(+12, -20));

            AddPoint((int)HotPoints.DL, new Vector2(-12, +20));
            AddPoint((int)HotPoints.DR, new Vector2(+12, +20));

            AddPoint((int)HotPoints.LU, new Vector2(-16, -10));
            AddPoint((int)HotPoints.LD, new Vector2(-16, +6));

            AddPoint((int)HotPoints.RU, new Vector2(+16, -10));
            AddPoint((int)HotPoints.RD, new Vector2(+16, +6));


            // Collide Point for Grip
            AddPoint((int)HotPoints.EL, new Vector2(-16, -20));
            AddPoint((int)HotPoints.ER, new Vector2(+16, -20));

        }

        public override Node Init()
        {
            _speedMax = new Vector2(4f, 12f);
            _acceleration = new Vector2(1f, 1f);
            _deceleration = new Vector2(1f, 1f);


            // Init Raycast vector for each contactPoints
            _contactPoints[(int)HotPoints.UL].SetRayCast(0, -_speedMax.Y);
            _contactPoints[(int)HotPoints.UR].SetRayCast(0, -_speedMax.Y);

            _contactPoints[(int)HotPoints.DL].SetRayCast(0, +_speedMax.Y);
            _contactPoints[(int)HotPoints.DR].SetRayCast(0, +_speedMax.Y);

            _contactPoints[(int)HotPoints.LU].SetRayCast(-_speedMax.X*2, 0);
            _contactPoints[(int)HotPoints.LD].SetRayCast(-_speedMax.X*2, 0);

            _contactPoints[(int)HotPoints.RU].SetRayCast(+_speedMax.X*2, 0);
            _contactPoints[(int)HotPoints.RD].SetRayCast(+_speedMax.X*2, 0);

            _contactPoints[(int)HotPoints.EL].SetRayCast(-_speedMax.X*2, 0);
            _contactPoints[(int)HotPoints.ER].SetRayCast(+_speedMax.X*2, 0);

            //_isLand = true;

            Fall();

            return base.Init();
        }

        public override Node Update(GameTime gameTime)
        {

            #region Button Events

            // Manage Button Status

            IS_B_UP = _player.GetButton((int)SNES.BUTTONS.UP) != 0 && IS_CONTROLL;
            IS_B_DOWN = _player.GetButton((int)SNES.BUTTONS.DOWN) != 0 && IS_CONTROLL;
            IS_B_LEFT = _player.GetButton((int)SNES.BUTTONS.LEFT) != 0 && IS_CONTROLL;
            IS_B_RIGHT = _player.GetButton((int)SNES.BUTTONS.RIGHT) != 0 && IS_CONTROLL;

            IS_B_L = _player.GetButton((int)SNES.BUTTONS.L) != 0 && IS_CONTROLL;
            IS_B_R = _player.GetButton((int)SNES.BUTTONS.R) != 0 && IS_CONTROLL;

            IS_B_A = _player.GetButton((int)SNES.BUTTONS.A) != 0 && IS_CONTROLL;
            IS_B_B = _player.GetButton((int)SNES.BUTTONS.B) != 0 && IS_CONTROLL;
            IS_B_X = _player.GetButton((int)SNES.BUTTONS.X) != 0 && IS_CONTROLL;
            IS_B_Y = _player.GetButton((int)SNES.BUTTONS.Y) != 0 && IS_CONTROLL;


            // Manage Button Trigger
            ON_B_UP = false; if (!IS_B_UP) IS_PUSH_B_UP = false; if (IS_B_UP && !IS_PUSH_B_UP) { IS_PUSH_B_UP = true; ON_B_UP = true; }
            ON_B_DOWN = false; if (!IS_B_DOWN) IS_PUSH_B_DOWN = false; if (IS_B_DOWN && !IS_PUSH_B_DOWN) { IS_PUSH_B_DOWN = true; ON_B_DOWN = true; }
            ON_B_LEFT = false; if (!IS_B_LEFT) IS_PUSH_B_LEFT = false; if (IS_B_LEFT && !IS_PUSH_B_LEFT) { IS_PUSH_B_LEFT = true; ON_B_LEFT = true; }
            ON_B_RIGHT = false; if (!IS_B_RIGHT) IS_PUSH_B_RIGHT = false; if (IS_B_RIGHT && !IS_PUSH_B_RIGHT) { IS_PUSH_B_RIGHT = true; ON_B_RIGHT = true; }

            ON_B_L = false; if (!IS_B_L) IS_PUSH_B_L = false; if (IS_B_L && !IS_PUSH_B_L) { IS_PUSH_B_L = true; ON_B_L = true; }
            ON_B_R = false; if (!IS_B_R) IS_PUSH_B_R = false; if (IS_B_R && !IS_PUSH_B_R) { IS_PUSH_B_R = true; ON_B_R = true; }

            ON_B_A = false; if (!IS_B_A) IS_PUSH_B_A = false; if (IS_B_A && !IS_PUSH_B_A) { IS_PUSH_B_A = true; ON_B_A = true; }
            ON_B_B = false; if (!IS_B_B) IS_PUSH_B_B = false; if (IS_B_B && !IS_PUSH_B_B) { IS_PUSH_B_B = true; ON_B_B = true; }
            ON_B_X = false; if (!IS_B_X) IS_PUSH_B_X = false; if (IS_B_X && !IS_PUSH_B_X) { IS_PUSH_B_X = true; ON_B_X = true; }
            ON_B_Y = false; if (!IS_B_Y) IS_PUSH_B_Y = false; if (IS_B_Y && !IS_PUSH_B_Y) { IS_PUSH_B_Y = true; ON_B_Y = true; }

            #endregion

            #region Controls

            ON_B_JUMP = ON_B_B; //|| ON_B_UP;
            IS_B_JUMP = IS_B_B; //|| IS_B_UP;

            // Movemment Up Down
            if (IS_B_UP)
            {
                MoveU();
            }
            if (IS_B_DOWN)
            {
                MoveD();
            }

            // Movement Left Right
            if (IS_B_LEFT) 
            { 
                MoveL(); 
            }
            if (IS_B_RIGHT)
            { 
                MoveR();
            }

            // Movement Jump up & down

            if (ON_B_JUMP)
            {
                //if (_status == Status.IS_FALL)
                //    Console.WriteLine("Too Late for jump !!!");

                Jump(_speedMax.Y);
            }

            
            if (!IS_B_JUMP && _status == Status.IS_JUMP) // Stop Jump when buttonUp jump
            {
                Fall();
            }

            #endregion

            if (Input.Button.OnePress("toggleShowDebug", ON_B_X))
                _showContactPoint = !_showContactPoint;


            return base.Update(gameTime);
        }

        public override Node Render(SpriteBatch batch)
        {

            //Draw.FillRectangleCentered(batch, AbsXY, new Vector2(24,32), Color.CadetBlue, 0f);
            //Draw.FillRectangleCentered(batch, AbsXY + new Vector2(0,14), new Vector2(18,12), Color.CadetBlue, 0f);

            Draw.FillRectangle(batch, AbsRect, Color.CadetBlue);
            Draw.Circle(batch, AbsXY, 8, 6, Color.Yellow, 2f);
            Draw.Rectangle(batch, AbsRect, Color.RoyalBlue,2f);

            //Draw.Line(batch, AbsXY, AbsXY + new Vector2(0, _tileMapLayer._map2D._tileH), Color.Red, 3);

            // Debug : Draw _collideP
            if (_showContactPoint)
            {
                for (int i=0; i< _contactPoints.Length; ++i)
                {
                    if (null != _contactPoints[i])
                    {

                        //if (i==(int)HotPoints.RU || i==(int)HotPoints.RD)
                        //    Draw.Line(batch, XY + _parent.AbsXY, _parent.AbsXY + _contactPoints[i]._pointContact, Color.LightSlateGray, 2f);

                        Color color = Color.GreenYellow;

                        // Raycast line
                        //Vector2 raycast = _finalVector * 2f;
                        //Draw.Line(batch, _contactPoints[i]._pos - raycast + _parent.AbsXY, _contactPoints[i]._pos + raycast + _parent.AbsXY, Color.MediumVioletRed, 1f);
                        Draw.Line(batch, _contactPoints[i]._pos - _contactPoints[i]._raycast + _parent.AbsXY, _contactPoints[i]._pos + _contactPoints[i]._raycast + _parent.AbsXY, Color.MediumVioletRed, 1f);

                        if (_contactPoints[i]._isContact)
                        {
                            color = Color.Red;

                            // Polygon of contact point
                            Draw.PolyLine(batch, _contactPoints[i]._polygon, Color.White, 1f, _parent.AbsXY + _contactPoints[i]._offset);

                            Line line = _contactPoints[i]._lineContact;
                            
                            // Line of contact point
                            Draw.Line(batch, line.A + _parent.AbsXY, line.B + _parent.AbsXY, Color.Aqua, 3f);

                            // Point of intersect raycast line & line Contact
                            Draw.Point(batch, _contactPoints[i]._pointContact + _parent.AbsXY, 3f, Color.Red);
                            
                            // Name of the point contact
                            //Draw.TopCenterString(batch, Game1._fontMain,((HotPoints)i).ToString() , _contactPoints[i]._pointContact.X + _parent.AbsX, _contactPoints[i]._pointContact.Y + _parent.AbsY, Color.White);

                            
                        }

                        // Contact point
                        Draw.Point(batch, _contactPoints[i]._pos + _parent.AbsXY, 1, color);
                    }


                    //Draw.Point(batch, new Vector2(_mapPosX * _tileW + _tileW/2, _mapPosY * _tileW +_tileH/2) + _parent.AbsXY, 4, Color.Gold);
                    //Draw.LeftTopString(batch, Game1._fontMain, TILE_AT_L.ToString(), _mapPosX * _tileW + _tileW/ 2 + _parent.AbsX, _mapPosY * _tileW +_tileH/2 + _parent.AbsY, Color.Goldenrod);
                }

                //Draw.TopCenterString(batch, Game1._fontMain, "oldStatus = " + _oldStatus, AbsX, AbsY + 48, Color.Gray);
                //Draw.TopCenterString(batch, Game1._fontMain, CAN_CLIMB_L + "<EDGE>" + CAN_CLIMB_R, AbsX, AbsY + 64, Color.GreenYellow);
                Draw.TopCenterString(batch, Game1._fontMain, "PUSH " + _isPush, AbsX, AbsY + 48, Color.Violet);
                
                if (_status == Status.IS_JUMP) Draw.TopCenterString(batch, Game1._fontMain, "JUMP", AbsX, AbsY-48, Color.Yellow);
                if (_status == Status.IS_FALL) Draw.TopCenterString(batch, Game1._fontMain, "FALL", AbsX, AbsY-48, Color.Red);
                if (_status == Status.IS_LAND) Draw.TopCenterString(batch, Game1._fontMain, "LAND", AbsX, AbsY-48, Color.Orange);
                if (_status == Status.IS_GRAB) Draw.TopCenterString(batch, Game1._fontMain, "GRAB "+ _ticGrab, AbsX, AbsY - 48, Color.Honeydew);
                if (_status == Status.IS_CLIMB) Draw.TopCenterString(batch, Game1._fontMain, "CLIMB"+_ticAutoMove, AbsX, AbsY - 48, Color.Honeydew);

                Draw.Line(batch, _landLineA + _parent.AbsXY, _landLineB + _parent.AbsXY, Color.Green, 2f);
                Draw.Point(batch, _pointLandCollision + _parent.AbsXY, 4f, Color.RoyalBlue);

                Draw.Line(batch, AbsXY, AbsXY - Geo.GetVector(_landRadian) * 40, Color.LimeGreen, 1f);
                Draw.Line(batch, AbsXY, AbsXY + Geo.GetVector(_landRadian) * 40, Color.LimeGreen, 1f);

                Draw.Line(batch, AbsXY, AbsXY + Geo.GetVector(_angleMove.X) * _velocity.X * 40, Color.MediumVioletRed, 2f);
                Draw.Line(batch, AbsXY, AbsXY + Geo.GetVector(_angleMove.Y) * _velocity.Y * 40, Color.DarkOliveGreen, 2f);
            
                Draw.TopCenterString(batch, Game1._fontMain, $"[{_x.ToString("0")} , { _y.ToString("0")}]", AbsX, AbsY - 64, Color.Yellow);

                Draw.TopCenterString(batch, Game1._fontMain, $"{ _sumVector.X.ToString("0.0")}|{ _sumVector.Y.ToString("0.0")}", AbsX, AbsY + 64, Color.Yellow);
                Draw.TopCenterString(batch, Game1._fontMain, $"{ _velocity.X.ToString("0.0")}|{ _velocity.Y.ToString("0.0")}", AbsX, AbsY + 80, Color.MonoGameOrange);
            
            }

            if (_status == Status.IS_GRAB)
                Draw.Point(batch, _ledgeGrip + _parent.AbsXY, 4f, Color.MonoGameOrange);


            return base.Render(batch);
        }
    }
}
