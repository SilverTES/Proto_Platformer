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
    public class Enemy0 : MapActor
    {
        int _direction = 1;

        Color _colorA;
        Color _colorB;

        public Enemy0(TileMapLayer tileMapLayer) : base(tileMapLayer)
        {

            _type = UID.Get<Enemy0>();

            SetSize(24, 32);
            SetPivot(12, 16);

            // Create Collide Point of Hero
            AddPoint((int)HSpots.UL, new Vector2(-10, -20));
            AddPoint((int)HSpots.UR, new Vector2(+10, -20));

            AddPoint((int)HSpots.DL, new Vector2(-10, +20));
            AddPoint((int)HSpots.DR, new Vector2(+10, +20));

            AddPoint((int)HSpots.LU, new Vector2(-16, -8));
            AddPoint((int)HSpots.LD, new Vector2(-16, +6));

            AddPoint((int)HSpots.RU, new Vector2(+16, -8));
            AddPoint((int)HSpots.RD, new Vector2(+16, +6));


            // Collide Point for Grip
            AddPoint((int)HSpots.EL, new Vector2(-16, -16));
            AddPoint((int)HSpots.ER, new Vector2(+16, -16));

            _colorA = new Color(Misc.Rng.Next(100, 250), Misc.Rng.Next(50, 250), Misc.Rng.Next(50, 250));
            _colorB = new Color(Misc.Rng.Next(100, 250), Misc.Rng.Next(50, 250), Misc.Rng.Next(50, 250));

        }

        public override Node Init()
        {
            _speedMax = new Vector2(1f, 12f);
            _acceleration = new Vector2(1f, .5f);
            _deceleration = new Vector2(1f, .5f);


            // Init Raycast vector for each contactPoints
            _cPoints[(int)HSpots.UL].SetRayCast(0, -_speedMax.Y);
            _cPoints[(int)HSpots.UR].SetRayCast(0, -_speedMax.Y);

            _cPoints[(int)HSpots.DL].SetRayCast(0, +_speedMax.Y);
            _cPoints[(int)HSpots.DR].SetRayCast(0, +_speedMax.Y);

            _cPoints[(int)HSpots.LU].SetRayCast(-_speedMax.X * 4, 0);
            _cPoints[(int)HSpots.LD].SetRayCast(-_speedMax.X * 4, 0);

            _cPoints[(int)HSpots.RU].SetRayCast(+_speedMax.X * 4, 0);
            _cPoints[(int)HSpots.RD].SetRayCast(+_speedMax.X * 4, 0);

            _cPoints[(int)HSpots.EL].SetRayCast(-_speedMax.X * 4, 0);
            _cPoints[(int)HSpots.ER].SetRayCast(+_speedMax.X * 4, 0);

            Fall();

            return base.Init();
        }

        public override Node Update(GameTime gameTime)
        {

            if (!CAN_MOVE_LEFT) _direction = 1;
            if (!CAN_MOVE_RIGHT) _direction = -1;



            if (_direction > 0)
                MoveR();
            if (_direction < 0)
                MoveL();


            return base.Update(gameTime);
        }


        public override Node Render(SpriteBatch batch)
        {

            Draw.Point(batch, AbsXY, 16, _colorA);
            Draw.Circle(batch, AbsXY, 16, 16, _colorB, 5f);

            return base.Render(batch);
        }

    }
}
