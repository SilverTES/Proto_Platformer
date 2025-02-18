﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Retro2D;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proto_00
{
    public enum Status
    {
        NONE,
        IS_JUMP,
        IS_FALL,
        IS_LAND,
        IS_GRAB,
        IS_CLIMB,
    }

    public enum HSpots
    {
        UL, UR, // Collide TOP
        DL, DR, // Collide BOTTOM
        LU, LD, // Collide LEFT
        RU, RD, // Collide RIGHT
        EL, ER, // Edge detection
    }

    // Use for List Contact Point Detection
    public class ContactPoint
    {
        public TileMap _tile;
        public Vector2[] _polygon; // points are relative not absolute position
        public Line _lineContact = new Line();
        public Line _firstline = new Line();
        public Vector2 _raycast = new Vector2();
        public Vector2 _pointContact = new Vector2();

        public Vector2 _offset;  //  offset for polygon real  postion  in  world
        public Vector2 _pos = new Vector2();
        public Vector2 _hotPoint = new Vector2();
        
        // Pos map where is the point in map
        public int _mapX;
        public int _mapY;
        
        public bool _isContact  =  false;
        public bool _successRaycast = false;

        public ContactPoint(float x = 0, float y = 0, bool isContact = false, TileMap tile = null)
        {
            _pos.X = x;
            _pos.Y = y;
            _isContact = isContact;
            _tile = tile;
        }
        public void SetRayCast(float dx, float dy)
        {
            _raycast.X = dx;
            _raycast.Y = dy;
        }
        public Line GetLineContact(Vector2 offset, Vector2[] polygon)
        {
            _successRaycast = false;
            _lineContact = new Line(polygon[0] + offset, polygon[1] + offset);
            _firstline = new Line(polygon[0] + offset, polygon[1] + offset);
            _pointContact = _pos;

            bool isIntersect;

            for(int  i=0; i<polygon.Length-1; ++i)
            {
                _lineContact.A = polygon[i] + offset;
                _lineContact.B = polygon[i + 1] + offset;

                //if (Collision2D.LineSegment(_lineContact.A, _lineContact.B, _pos - _raycast, _pos + _raycast))
                if (Collision2D.SegmentSegmentIntersection(_lineContact, new Line(_pos - _raycast, _pos + _raycast), out isIntersect) != Vector2.Zero)
                {
                    _pointContact = Collision2D.SegmentSegmentIntersection(_lineContact, new Line(_pos - _raycast, _pos + _raycast), out isIntersect);
                    //_pointContact = Collision2D.LineLineIntersection(_lineContact.A, _lineContact.B,_pos - _raycast, _pos + _raycast);
                    _successRaycast = true;
                    break;
                }
            }

            return _lineContact;
        }

    }


    public class MapActor : MapObject
    {
        #region Attributes

        protected ContactPoint[] _cPoints;

        protected Vector2 _oldPosition = new Vector2();

        protected Vector2 _angleMove = new Vector2(Geo.RAD_R, Geo.RAD_U);

        protected Vector2 _speed = new Vector2();
        protected Vector2 _speedMax = new Vector2(6f, 16f);
        protected Vector2 _acceleration = new Vector2(.6f , 1f);
        protected Vector2 _deceleration = new Vector2(.6f , 1f);

        protected Vector2 _velocity = new Vector2();
        public Vector2 Velocity => _velocity;

        protected Vector2 _sumVector = new Vector2();

        protected Vector2 _finalVector = new Vector2();
        public Vector2 FinalVector => _finalVector;

        protected bool _isMove = false;
        protected bool _onMove = false;
        //protected bool _offMove = false;
        
        protected bool _onJump = false;
        //protected bool _offJump = false;

        protected bool _onFall = false;
        //protected bool _offFall = false;

        protected bool _onLand = false;
        //protected bool _offLand = false;

        protected bool _onGrab = false;
        //protected bool _offGrab = false;

        protected bool _onClimb = false;
        //protected bool _offClimb = false;

        protected bool _onCanWallJump = false;
        //protected bool _offCanWallJump = false;

        protected Status _status;
        protected Status _oldStatus;

        // Jump Helper
        protected bool _isJustFall = false;
        protected int _ticJustFall = 0;
        protected int _maxTicJustFall = 4;

        // Ledge controls
        //protected bool _isLedgeL = false;
        //protected bool _isLedgeR = false;

        protected bool AUTO_UP = false;
        protected bool AUTO_MOVE = false;

        protected int _ticAutoUp = 0;
        protected int _ticAutoMove = 0;
        protected int _directionAutoMove = 0;

        // Grab -> Climb Helper
        //protected bool _isJustGrab = false;
        protected int _ticGrab = 0;
        protected int _tempoGrab = 4;
        protected bool _readyToClimb = false;

        // Tempo Push Left or Right
        protected bool _isPush = false;

        // Point ideal de collision !
        protected float _landY_L;
        protected float _landY_R;
        protected float _landY;

        protected float _contactX;
        protected float _contactY;
        protected ContactPoint _contactPointDown;

        protected float _topY_L;
        protected float _topY_R;
        protected float _topY;
        
        protected float _sideL;
        protected float _sideR;
        protected float _grabY;
        
        protected Vector2 _ledgeGrip = new Vector2();

        protected bool MOVE_UP = false;
        protected bool MOVE_DOWN = false;
        protected bool MOVE_LEFT = false;
        protected bool MOVE_RIGHT = false;
        protected bool MOVE_JUMP = false;
        protected bool MOVE_FALL = false;

        // contrainte mouvement
        public bool CAN_MOVE_UP = true;
        public bool CAN_MOVE_DOWN = true;
        public bool CAN_MOVE_LEFT = true;
        public bool CAN_MOVE_RIGHT = true;

        public bool CAN_GRAB_L = false;
        public bool CAN_GRAB_R = false;

        public bool IS_GRAB_L = false;
        public bool IS_GRAB_R = false;

        public bool CAN_CLIMB_L = false;
        public bool CAN_CLIMB_R = false;

        public bool CAN_WALL_JUMP_L = false;
        public bool CAN_WALL_JUMP_R = false;
        //public bool CAN_WALL_JUMP = false;

        // collision delta
        protected float _oldX = 0;
        protected float _oldY = 0;
        protected float _deltaX = 0;
        protected float _deltaY = 0;

        protected float _preCollisionRayCastX = 0;
        protected float _preCollisionRayCastY = 0;

        protected bool _CollidePointInPolygon = false;
        protected Vector2 _pointLandCollision = new Vector2();
        protected Vector2 _pointLandCollision_L = new Vector2();
        protected Vector2 _pointLandCollision_R = new Vector2();
        
        protected Vector2 _landLineA = Vector2.Zero;
        protected Vector2 _landLineA_L = Vector2.Zero;
        protected Vector2 _landLineA_R = Vector2.Zero;
        
        protected Vector2 _landLineB = Vector2.Zero;
        protected Vector2 _landLineB_L = Vector2.Zero;
        protected Vector2 _landLineB_R = Vector2.Zero;

        protected float _landRadian = 0f;
        protected float _landRadian_L = 0f;
        protected float _landRadian_R = 0f;

        public int PASS_LEVEL = 2;

        // Collide points
        public Dictionary<int, Vector2> _hSpots = new Dictionary<int, Vector2>();

        #endregion

        public MapActor(TileMapLayer tileMapLayer) : base(tileMapLayer)
        {
            _cPoints = new ContactPoint[Enum.GetNames(typeof(HSpots)).Length];

            for (int i=0; i<_cPoints.Length; ++i)
            {
                _cPoints[i] = new ContactPoint();
            }

        }

        public void AddPoint(int position, Vector2 collideP)
        {
            _hSpots[position] = collideP;

        }
        public Vector2 GetPoint(int position)
        {
            if (_hSpots.ContainsKey(position))
                return _hSpots[position];

            return Vector2.Zero;
        }
        public void SetStatus(Status status)
        {
            _oldStatus = _status;
            _status = status;

            //Console.WriteLine($"Status {_oldStatus} --> { _status} ");
        }

        public void MoveU()
        {
            MOVE_UP = true;
        }
        public void MoveD()
        {
            MOVE_DOWN = true;
        }
        public void MoveL()
        {
            MOVE_LEFT = true;

            if (CAN_MOVE_LEFT && _status != Status.IS_GRAB)
            {
                _isMove = true;

                //_angleMove.X = Geo.RAD_L;

                if (_status != Status.IS_LAND)
                    _angleMove.X = Geo.RAD_L;
                else
                    _angleMove.X = _landRadian + (float)Math.PI;

                AccelerateX(); 

            }
        }
        public void MoveR()
        {
            MOVE_RIGHT = true;

            if (CAN_MOVE_RIGHT && _status != Status.IS_GRAB)
            {
                _isMove = true;

                //_angleMove.X = Geo.RAD_R;

                if (_status != Status.IS_LAND)
                    _angleMove.X = Geo.RAD_R;
                else
                    _angleMove.X = _landRadian;

                AccelerateX();
            }
        }
        public void AccelerateX(float factor = 1f)
        {
            //if (_oldStatus  ==  Status.IS_LAND)
            //{
            //    if (_status == Status.IS_JUMP) 
            //        factor = factor * 2f;
            //    else  if (_status == Status.IS_FALL) 
            //        factor = factor * .5f;
            //}

            _velocity.X += _acceleration.X * factor;

            if (_velocity.X >= _speedMax.X) 
                _velocity.X = _speedMax.X;
        }
        public void DecelerateX(float factor = 1f)
        {
            _velocity.X -= _deceleration.X * factor;

            if (_velocity.X <= 0)
                _velocity.X = 0;
        }
        public bool Jump(float jumpPower) // Return true if can jump
        {
            MOVE_JUMP = true;

            if (_status == Status.IS_LAND || (_status == Status.IS_GRAB && !_isPush))
            {
                SetStatus(Status.IS_JUMP);

                _onJump = true;

                _angleMove.Y = Geo.RAD_U;
                _velocity.Y = jumpPower;
                return true;
            }
            else if (MOVE_JUMP) // Wall Jump
            {
                if (((CAN_WALL_JUMP_R && !_cPoints[(int)HSpots.ER]._isContact) || (CAN_WALL_JUMP_L && !_cPoints[(int)HSpots.EL]._isContact)) && !_isPush)
                {
                    
                    Console.WriteLine("Wall Jump !");

                    //SetStatus(Status.IS_GRAB);
                    SetStatus(Status.IS_JUMP);

                    _onJump = true;

                    _angleMove.Y = Geo.RAD_U;
                    _velocity.Y = jumpPower + 1;
                    
                    _velocity.X = _speedMax.X;


                    CAN_WALL_JUMP_L = false;
                    CAN_WALL_JUMP_R = false;
                    
                    return true;
                }
            }

            return false;
        }
        public void Fall()
        {
            MOVE_FALL = true;

            SetStatus(Status.IS_FALL);

            _onFall = true;

        }
        public bool Land()
        {
            if (_status == Status.IS_FALL)
            {
                SetStatus(Status.IS_LAND);

                _onLand = true;
            }
            else
                return false;

            return true;
        }
        public bool Grab(ref bool isGrab)
        {

            if (_status == Status.IS_FALL) // || _status == Status.IS_JUMP)
            {
                isGrab = true;

                //CAN_GRAB_L = false;
                //CAN_GRAB_R = false;

                SetStatus(Status.IS_GRAB);
                _ticGrab = 0;
                _onGrab = true;
            }
            else
                return false;

            return true;
        }
        public void Climb(int direction)
        {
            if (_readyToClimb)
            {
                CAN_CLIMB_L = false;
                CAN_CLIMB_R = false;

                IS_GRAB_L = false;
                IS_GRAB_R = false;

                Console.WriteLine("Climb()");
                SetStatus(Status.IS_CLIMB); 

                _onClimb = true; 
                _readyToClimb = false; 
                AUTO_UP = true; 

                _ticAutoUp = 0;
                _ticGrab = 0;
                _directionAutoMove = direction;
            }
        }

        public override Node Init()
        {

            return base.Init();
        }
        public override Node Update(GameTime gameTime)
        {
            

            CAN_MOVE_UP = true;
            CAN_MOVE_DOWN = true;
            CAN_MOVE_LEFT = true;
            CAN_MOVE_RIGHT = true;

            CAN_GRAB_L = false;
            CAN_GRAB_R = false;

            //CAN_WALL_JUMP = false;

            _deltaX = _x - _oldX;
            _deltaY = _y - _oldY;

            UpdateRect();

            if (!_isMove)
            {
                if (_status == Status.IS_LAND)
                    DecelerateX(2f);
                else
                    DecelerateX(2f);
            }

            // Lateral Move State
            _isPush = false;
            _isMove = false;
            _onMove = false;

            // Jump Move State
            //_onJump = false;
            //_onFall = false;



            //_sumVector.X = Geo.GetVector(_angleMove.X).X;
            //_sumVector.Y = Geo.GetVector(_angleMove.Y).Y;

            _sumVector = Geo.GetVector(_angleMove.X) * _velocity.X + Geo.GetVector(_angleMove.Y) * _velocity.Y;

            if (MOVE_LEFT && MOVE_RIGHT) _sumVector.X = 0f;

            //_finalVector.X = _sumVector.X * _velocity.X;
            //_finalVector.Y = _sumVector.Y * _velocity.Y;

            _finalVector = _sumVector;

            //_finalVector.X = Geo.GetVector(_angleMove.X).X * _velocity.X;
            //_finalVector.Y = Geo.GetVector(_angleMove.Y).Y * _velocity.Y;

            #region Collisions Test

            if (_hSpots.Count > 0)
            {

                // Reset all collide point to false !
                for (int i = 0; i < _hSpots.Count; i++)
                {
                    _cPoints[i]._isContact = false;

                    _cPoints[i]._hotPoint = _hSpots[i];

                    _cPoints[i]._pos = XY + _hSpots[i] + _finalVector;

                    _cPoints[i]._mapX = (int)(_cPoints[i]._pos.X / _tileW);
                    _cPoints[i]._mapY = (int)(_cPoints[i]._pos.Y / _tileH);

                    _cPoints[i]._offset = new Vector2(_cPoints[i]._mapX * _tileW, _cPoints[i]._mapY * _tileH);

                    _cPoints[i]._tile = _map2D.Get(_cPoints[i]._mapX, _cPoints[i]._mapY);

                    //_CollidePointInPolygon = false;

                    if (null != _cPoints[i]._tile)
                    {
                        // check the tileset if collide in polygon
                        if (_tileMapLayer._tileSets.ContainsKey(_cPoints[i]._tile._id))
                        {
                            TileSet tileset = _tileMapLayer._tileSets[_cPoints[i]._tile._id];

                            _cPoints[i]._polygon = tileset._polygonCollision;
                            
                            if (_cPoints[i]._polygon != null)
                            {
                                // get polygon[] in the tile where is the contactPoint of the global Tileset
                                _cPoints[i].GetLineContact(_cPoints[i]._offset, _cPoints[i]._polygon);

                                _cPoints[i]._isContact =
                                    Collision2D.PointInPolygon(_cPoints[i]._pos, _cPoints[i]._polygon, _cPoints[i]._polygon.Length, _cPoints[i]._offset) &&
                                    _cPoints[i]._tile._isCollidable &&
                                    _cPoints[i]._tile._passLevel > PASS_LEVEL; // && _contactPoints[i]._successRaycast;

                            }

                        }
                    }
                }

                // Point Test

                // Up Top
                #region Contact : UP / TOP 
                if (_cPoints[(int)HSpots.UL]._isContact && _status == Status.IS_JUMP)
                {
                    CAN_MOVE_UP = false;
                    //_topY = _topY_L = _contactPoints[(int)HotPoints.UL]._mapY * _tileH + _tileH + _rect.Height / 2 + 2;
                    _topY = _topY_L = _cPoints[(int)HSpots.UL]._pointContact.Y + _rect.Height / 2 + 2;
                }
                if (_cPoints[(int)HSpots.UR]._isContact && _status == Status.IS_JUMP)
                {
                    CAN_MOVE_UP = false;
                    //_topY = _topY_R = _contactPoints[(int)HotPoints.UR]._mapY * _tileH + _tileH + _rect.Height / 2 + 2;
                    _topY = _topY_R = _cPoints[(int)HSpots.UR]._pointContact.Y + _rect.Height / 2 + 2;
                }

                if (_cPoints[(int)HSpots.UL]._isContact && _cPoints[(int)HSpots.UR]._isContact && _status == Status.IS_JUMP)
                {
                    if (_topY_L > _topY_R)
                        _topY = _topY_L;
                    else
                        _topY = _topY_R;
                }
                #endregion

                #region Contact : DOWN / BOTTOM
                if (_cPoints[(int)HSpots.DL]._isContact)
                {
                    CAN_MOVE_DOWN = false;

                    _landLineA = _landLineA_L =_cPoints[(int)HSpots.DL]._polygon[0] + _cPoints[(int)HSpots.DL]._offset;
                    _landLineB = _landLineB_L = _cPoints[(int)HSpots.DL]._polygon[1] + _cPoints[(int)HSpots.DL]._offset;

                    _landRadian = _landRadian_L = Geo.GetRadian(_landLineA, _landLineB);

                    //_pointLandCollision = _pointLandCollision_L = Collision2D.LineLineIntersection(_landLineA, _landLineB, _contactPoints[(int)HotPoints.DL]._pos, _contactPoints[(int)HotPoints.DL]._pos + _contactPoints[(int)HotPoints.DL]._raycast);
                    _pointLandCollision = _pointLandCollision_L = _cPoints[(int)HSpots.DL]._pointContact;

                    _landY = _landY_L = _pointLandCollision_L.Y - _rect.Height / 2 - 2;
                }


                if (_cPoints[(int)HSpots.DR]._isContact)
                {
                    CAN_MOVE_DOWN = false;

                    _landLineA = _landLineA_R =_cPoints[(int)HSpots.DR]._polygon[0] + _cPoints[(int)HSpots.DR]._offset;
                    _landLineB = _landLineB_R = _cPoints[(int)HSpots.DR]._polygon[1] + _cPoints[(int)HSpots.DR]._offset;

                    _landRadian = _landRadian_R = Geo.GetRadian(_landLineA, _landLineB);

                    //_pointLandCollision = _pointLandCollision_R = Collision2D.LineLineIntersection(_landLineA, _landLineB, _contactPoints[(int)HotPoints.DR]._pos, _contactPoints[(int)HotPoints.DR]._pos + _contactPoints[(int)HotPoints.DR]._raycast);
                    _pointLandCollision = _pointLandCollision_R = _cPoints[(int)HSpots.DR]._pointContact;

                    _landY = _landY_R = _pointLandCollision_R.Y - _rect.Height / 2 - 2;
                }
                #endregion

                #region Contact : LEFT & RIGHT
                // Limit Side Move
                // LEFT
                if (_cPoints[(int)HSpots.LU]._isContact && MOVE_LEFT)
                {
                    CAN_MOVE_LEFT = false;
                    //_sideL = _contactPoints[(int)HotPoints.LU]._mapX * _tileW + _tileW + _rect.Width / 2 + 2;
                    _sideL = _cPoints[(int)HSpots.LU]._pointContact.X + _rect.Width / 2 + 2;
                }
                if (_cPoints[(int)HSpots.LD]._isContact && MOVE_LEFT)
                {
                    CAN_MOVE_LEFT = false;
                    //_sideL = _contactPoints[(int)HotPoints.LD]._mapX * _tileW + _tileW + _rect.Width / 2 + 2;
                    _sideL = _cPoints[(int)HSpots.LD]._pointContact.X + _rect.Width / 2 + 2;
                }
                // RIGHT
                if (_cPoints[(int)HSpots.RU]._isContact && MOVE_RIGHT)
                {
                    CAN_MOVE_RIGHT = false;
                    //_sideR = _contactPoints[(int)HotPoints.RU]._mapX * _tileW - _rect.Width / 2 - 2;
                    _sideR = _cPoints[(int)HSpots.RU]._pointContact.X - _rect.Width / 2 - 2;
                }
                if (_cPoints[(int)HSpots.RD]._isContact && MOVE_RIGHT)
                {
                    CAN_MOVE_RIGHT = false;
                    //_sideR = _contactPoints[(int)HotPoints.RD]._mapX * _tileW - _rect.Width / 2 - 2;
                    _sideR = _cPoints[(int)HSpots.RD]._pointContact.X - _rect.Width / 2 - 2;
                }

                // Type Slope = 3
                if (_cPoints[(int)HSpots.LU]._tile != null)
                    if (!_cPoints[(int)HSpots.LU]._isContact && _cPoints[(int)HSpots.LU]._tile._type == 3 && MOVE_LEFT) // && _contactPoints[(int)CPoint.DL]._isContact
                    {
                        //CAN_MOVE_DOWN = false;

                        _landLineA = _landLineA_L = _cPoints[(int)HSpots.LU]._polygon[0] + _cPoints[(int)HSpots.LU]._offset;
                        _landLineB = _landLineB_L = _cPoints[(int)HSpots.LU]._polygon[1] + _cPoints[(int)HSpots.LU]._offset;

                        _landRadian = _landRadian_L = Geo.GetRadian(_landLineA, _landLineB);

                        if (_cPoints[(int)HSpots.DL]._isContact)
                        {
                            CAN_MOVE_DOWN = false;

                            _pointLandCollision = _pointLandCollision_L = Collision2D.LineLineIntersection(_landLineA, _landLineB, _cPoints[(int)HSpots.DL]._pos, _cPoints[(int)HSpots.DL]._pos + _cPoints[(int)HSpots.DL]._raycast);
                            //_pointLandCollision = _pointLandCollision_L = _cPoints[(int)HSpots.DL]._pointContact;
                            _landY = _landY_L = _pointLandCollision_L.Y - _rect.Height / 2 - 2;
                        }
                    }

                if (_cPoints[(int)HSpots.RU]._tile != null)
                    if (!_cPoints[(int)HSpots.RU]._isContact && _cPoints[(int)HSpots.RU]._tile._type == 3 && MOVE_RIGHT) // && _contactPoints[(int)CPoint.DL]._isContact
                    {
                        //CAN_MOVE_DOWN = false;

                        _landLineA = _landLineA_R = _cPoints[(int)HSpots.RU]._polygon[0] + _cPoints[(int)HSpots.RU]._offset;
                        _landLineB = _landLineB_R = _cPoints[(int)HSpots.RU]._polygon[1] + _cPoints[(int)HSpots.RU]._offset;

                        _landRadian = _landRadian_R = Geo.GetRadian(_landLineA, _landLineB);

                        if (_cPoints[(int)HSpots.DR]._isContact)
                        {
                            CAN_MOVE_DOWN = false;

                            _pointLandCollision = _pointLandCollision_R = Collision2D.LineLineIntersection(_landLineA, _landLineB, _cPoints[(int)HSpots.DR]._pos, _cPoints[(int)HSpots.DR]._pos + _cPoints[(int)HSpots.DR]._raycast);
                            //_pointLandCollision = _pointLandCollision_R = _cPoints[(int)HSpots.DR]._pointContact;
                            _landY = _landY_R = _pointLandCollision_R.Y - _rect.Height / 2 - 2;
                        }
                    }

                // Choose best LandY Left or Right
                //if (MOVE_LEFT && _contactPoints[(int)CPoint.DL]._isContact)
                if (_cPoints[(int)HSpots.DL]._isContact && _cPoints[(int)HSpots.DR]._isContact)
                {
                    //Console.Write("< BOTH LAND L & R >");

                    if (_pointLandCollision_L.Y < _pointLandCollision_R.Y)
                    //if (_landY_L < _landY_R)
                    {
                        _pointLandCollision = _pointLandCollision_L;

                        _landLineA = _landLineA_L;
                        _landLineB = _landLineB_L;
                        _landRadian = _landRadian_L;
                        //_landY_R = _landY = _landY_L;
                        _landY = _landY_L;

                        _contactPointDown = _cPoints[(int)HSpots.DL];
                    }
                    else //if (MOVE_RIGHT && _contactPoints[(int)CPoint.DR]._isContact)
                    if (_pointLandCollision_L.Y > _pointLandCollision_R.Y)
                    {
                        _pointLandCollision = _pointLandCollision_R;

                        _landLineA = _landLineA_R;
                        _landLineB = _landLineB_R;
                        _landRadian = _landRadian_R;
                        //_landY_L = _landY = _landY_R;
                        _landY = _landY_R;

                        _contactPointDown = _cPoints[(int)HSpots.DR];
                    }
                }

                #endregion

                #region Contact : GRAB LEDGE

                if (CAN_MOVE_UP && CAN_MOVE_DOWN && !CAN_MOVE_LEFT &&
                    _cPoints[(int)HSpots.EL]._isContact &&
                    Math.Abs(_cPoints[(int)HSpots.EL]._pos.Y - _cPoints[(int)HSpots.EL]._firstline.B.Y) <= 8)
                {
                    
                    TileMap tileU = _map2D.Get(_cPoints[(int)HSpots.EL]._mapX, _cPoints[(int)HSpots.EL]._mapY-1);

                    bool climbable = false;

                    if (tileU == null)
                    {
                        climbable = true;
                    }
                    else if (tileU._id != Const.NoIndex) // test is tile at up is empty or not, if not then test is climbable on Left
                    {
                        if (tileU._tileSet._properties.ContainsKey("ClimbR"))
                        {
                            climbable = true;
                        }
                    }
                    else
                    {
                        climbable = true;
                    }

                    if (climbable && !CAN_GRAB_L)
                    {
                        CAN_GRAB_L = true;

                        _grabY = _cPoints[(int)HSpots.EL]._firstline.B.Y - _hSpots[(int)HSpots.EL].Y;
                        
                        _ledgeGrip.X = _cPoints[(int)HSpots.EL]._firstline.B.X;
                        _ledgeGrip.Y = _cPoints[(int)HSpots.EL]._firstline.B.Y;
                        
                    }

                }

                if (CAN_MOVE_UP && CAN_MOVE_DOWN && !CAN_MOVE_RIGHT &&
                    _cPoints[(int)HSpots.ER]._isContact &&
                    Math.Abs(_cPoints[(int)HSpots.ER]._pos.Y - _cPoints[(int)HSpots.ER]._firstline.A.Y) <= 8)
                {

                    TileMap tileU = _map2D.Get(_cPoints[(int)HSpots.ER]._mapX, _cPoints[(int)HSpots.ER]._mapY - 1);

                    bool climbable = false;

                    if (tileU == null)
                    {
                        climbable = true;
                    }
                    else if (tileU._id != Const.NoIndex) // test is tile at up is empty or not, if not then test is climbable on Left
                    {
                        if (tileU._tileSet._properties.ContainsKey("ClimbL"))
                        {
                            climbable = true;
                        }
                    }
                    else
                    {
                        climbable = true;
                    }

                    if (climbable && !CAN_GRAB_R)
                    {
                        CAN_GRAB_R = true;

                        _grabY = _cPoints[(int)HSpots.ER]._firstline.A.Y - _hSpots[(int)HSpots.ER].Y;

                        _ledgeGrip.X = _cPoints[(int)HSpots.ER]._firstline.A.X;
                        _ledgeGrip.Y = _cPoints[(int)HSpots.ER]._firstline.A.Y;

                    }

                }
                #endregion

            }

            #endregion

            #region CAN MOVE TEST & Pre Collision Raycast

            if (!CAN_MOVE_DOWN)
            {
                if (_status != Status.IS_LAND)
                {
                    _preCollisionRayCastY = (_y + _finalVector.Y) - _landY;
                    _finalVector.Y -= _preCollisionRayCastY;

                    //_y = _landY ;
                    //_finalVector.Y = 0;

                    Land();
                }

                
            }
            if (!CAN_MOVE_UP)
            {

                Console.WriteLine("!! Touch Top !!");
                _preCollisionRayCastY = (_y + _finalVector.Y) - _topY;
                _finalVector.Y -= _preCollisionRayCastY;

            }

            if (!CAN_MOVE_LEFT)
            //if (_contactPoints[(int)HotPoints.LU]._isContact || _contactPoints[(int)HotPoints.LD]._isContact)
            {
                _preCollisionRayCastX = (_x + _finalVector.X) - _sideL;
                _finalVector.X -= _preCollisionRayCastX;

                if (MOVE_LEFT)
                {
                    _isPush = true;

                    if (!CAN_WALL_JUMP_R && _status == Status.IS_FALL && CAN_MOVE_UP && _cPoints[(int)HSpots.EL]._isContact)
                    {
                        Console.WriteLine("CAN WALL JUMP R");

                        _onCanWallJump = true;
                        CAN_WALL_JUMP_R = true;
                    }
                }
            }
            if (!CAN_MOVE_RIGHT)
            //if (_contactPoints[(int)HotPoints.RU]._isContact || _contactPoints[(int)HotPoints.RD]._isContact)
            {
                _preCollisionRayCastX = (_x + _finalVector.X) - _sideR;
                _finalVector.X -= _preCollisionRayCastX;

                if (MOVE_RIGHT)
                {
                    _isPush = true;

                    if (!CAN_WALL_JUMP_L && _status == Status.IS_FALL && CAN_MOVE_UP && _cPoints[(int)HSpots.ER]._isContact)
                    {
                        Console.WriteLine("CAN WALL JUMP L");

                        _onCanWallJump = true;
                        CAN_WALL_JUMP_L = true;
                    }
                }
            }


            if  (((CAN_GRAB_L && !CAN_MOVE_LEFT)|| (CAN_GRAB_R && !CAN_MOVE_RIGHT)) && _isPush && _status == Status.IS_FALL)
            {
                _preCollisionRayCastY = (_y + _finalVector.Y) - _grabY;
                _finalVector.Y -= _preCollisionRayCastY;

                //Console.WriteLine($"---------------------> _grabY = {_grabY} _preCollisionRayCastY = {_preCollisionRayCastY}");
                if (CAN_GRAB_L) Grab(ref IS_GRAB_L);
                if (CAN_GRAB_R) Grab(ref IS_GRAB_R);
            }


            #endregion

            #region Status

            if (_status == Status.IS_FALL)
            {
                float factor = 1f;

                if (((CAN_WALL_JUMP_L && _cPoints[(int)HSpots.ER]._isContact) || (CAN_WALL_JUMP_R && _cPoints[(int)HSpots.EL]._isContact) ) && _isPush && CAN_MOVE_UP)
                {
                    factor = .05f; // Slow down when grip wall
                }

                _velocity.Y += _acceleration.Y * factor;

                if (_velocity.Y >= _speedMax.Y) _velocity.Y = _speedMax.Y;

            }

            if (_status == Status.IS_LAND && _velocity.Y == 0)
            {
                if (CAN_MOVE_DOWN)
                    Fall();

                //_contactX = _x - _contactDown._mapX * _tileW; _contactY = _contactDown._mapY * _tileH;

                //_contactY = -((_contactY - _landLineA.Y) + (_landLineB.Y - _landLineA.Y) / (_landLineA.X - _landLineB.X) * _contactX) ;

                //_contactY = _contactY + _contactDown._mapY * _tileH;

                //Console.WriteLine($"_contactX = {_contactX} : {_contactY}");

                //if (_contactDown._isContact)
                //    _y = _contactY - _rect.Height / 2 - 2;

            }

            if (_status == Status.IS_GRAB)
            {

                ++_ticGrab;

                if (_ticGrab >= _tempoGrab)
                {
                    _ticGrab = _tempoGrab;

                    if (CAN_GRAB_L) {CAN_GRAB_L = false; CAN_CLIMB_L = true;}
                    if (CAN_GRAB_R) {CAN_GRAB_R = false; CAN_CLIMB_R = true;}


                    if (!_isPush) // When release button push then readyToClimb !
                    {
                        _readyToClimb = true;

                        if (MOVE_UP)
                        {
                            Console.WriteLine($"Climb < {IS_GRAB_L} - {IS_GRAB_R} > : MOVE_UP");

                            if (IS_GRAB_L) Climb(-1);
                            if (IS_GRAB_R) Climb(1);
                        }

                        if (MOVE_DOWN)
                        {
                            Fall();
                        }

                    }
                    else if (MOVE_JUMP)
                    {
                        Console.WriteLine($"Climb < {IS_GRAB_L} - {IS_GRAB_R} > : MOVE_JUMP");

                        _readyToClimb = true;

                        if (IS_GRAB_L) Climb(-1);
                        if (IS_GRAB_R) Climb(1);
                    }

                }

            }

            if (_status == Status.IS_CLIMB)
            {
                if (AUTO_UP)
                {
                    _y += -4f;

                    ++_ticAutoUp;
                    if (_ticAutoUp > _rect.Height / 4)
                    {
                        _ticAutoUp = 0;
                        _ticAutoMove = 0;
                        AUTO_UP = false;
                        AUTO_MOVE = true;
                    }

                }
                if (AUTO_MOVE)
                {
                    _y += -.5f;

                    ++_ticAutoMove;
                    if (_ticAutoMove > _rect.Width/4)
                    {
                        AUTO_MOVE = false;
                        _directionAutoMove = 0;

                        //Fall();
                        SetStatus(Status.IS_LAND);
                    }

                    if (_directionAutoMove < 0)
                    {
                        _landRadian = Geo.GetRadian(_cPoints[(int)HSpots.EL]._firstline.B, _cPoints[(int)HSpots.EL]._firstline.A);
                        MoveL();
                    }
                    if (_directionAutoMove > 0)
                    {
                        _landRadian = Geo.GetRadian(_cPoints[(int)HSpots.ER]._firstline.A, _cPoints[(int)HSpots.ER]._firstline.B);
                        MoveR();
                    }
                }

            }

            if (_status == Status.IS_JUMP)
            {
                _velocity.Y += -_deceleration.Y;

                if (_velocity.Y <= 0 || !CAN_MOVE_UP)
                    Fall();
            }

            #endregion

            #region Jump Helper when left Land or Grab

            if (_onFall && _status == Status.IS_FALL && (_oldStatus == Status.IS_LAND || (_status == Status.IS_GRAB && !_isPush)))
            {
                _isJustFall = true;
                _ticJustFall = 0;
                //Console.Write("<Just Fall from land >");
            }

            if (_isJustFall)
            {
                //if (!CAN_MOVE_UP) 
                //{
                //    _isJustFall = false; 
                //    SetStatus(Status.IS_FALL); Fall(); 
                //}

                if (MOVE_JUMP && !_onJump) // second chance to jump
                {
                    Console.WriteLine("Helper Jump !");

                    SetStatus(Status.IS_LAND);
                    Jump(_speedMax.Y);
                }

                ++_ticJustFall;
                if (_ticJustFall > _maxTicJustFall)
                {
                    //Console.Write("< Stop jump helper >");
                    _isJustFall = false;
                    _ticJustFall = 0;
                }
            }


            if (_onCanWallJump)
            {
                Console.WriteLine("ON Can Wall Jump !");
                _onCanWallJump = false;

                _velocity.Y = 0f;
            }


            #endregion

            #region Trigger

            if (_onLand)
            {
                _onLand = false;

                _velocity.Y = 0;
                _velocity.X = 0;
            }

            if (_onFall)
            {
                _onFall = false;
                _angleMove.Y = Geo.RAD_D;

                _velocity.X = 0;
                _velocity.Y = 0;
            }

            if (_onJump)
            {
                _onJump = false;
            }

            if (_onGrab)
            {
                _onGrab = false;
                //_isJustGrab = true;
                _velocity.Y = 0;
            }

            if (_onClimb)
            {
                _onClimb = false;
                //System.Console.Write("< ON_CLIMB >");
            }

            #endregion

            _oldX = _x;
            _oldY = _y;

            _x += _finalVector.X; //* (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            _y += _finalVector.Y; //* (float)gameTime.ElapsedGameTime.TotalSeconds * 100;

            MOVE_UP = false;
            MOVE_DOWN = false;
            MOVE_LEFT = false;
            MOVE_RIGHT = false;

            MOVE_JUMP = false;
            MOVE_FALL = false;

            return base.Update(gameTime);
        }


        public override Node Render(SpriteBatch batch)
        {

            //Draw.TopCenterString(batch, Game1._fontMain, _deltaX + "," + _deltaY, AbsX, AbsY - 32, Color.Magenta);

            return base.Render(batch);
        }

    }
}
