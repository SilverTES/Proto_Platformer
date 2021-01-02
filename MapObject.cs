using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Retro2D;


namespace Proto_00
{
    public class MapObject : Node
    {
        protected const float GRAVITY = 9.8f;

        protected int _tileW;
        protected int _tileH;

        protected int _mapPosX;
        protected int _mapPosY;

        //public TileMap TILE_AT;
        //public TileMap TILE_AT_L;
        //public TileMap TILE_AT_R;
        //public TileMap TILE_AT_U;
        //public TileMap TILE_AT_D;

        public TileMapLayer _tileMapLayer;
        // Map to collide 
        public Map2D<TileMap> _map2D;

        public MapObject(TileMapLayer tileMapLayer)
        {

            _tileMapLayer = tileMapLayer;
            _map2D = tileMapLayer._map2D;

            _tileW = _map2D._tileW;
            _tileH = _map2D._tileH;
        }

        public override Node Update(GameTime gameTime)
        {

            _mapPosX = (int)(_x / _tileW);
            _mapPosY = (int)(_y / _tileH);

            //TILE_AT = _map2D.Get(_mapPosX, _mapPosY);
            //TILE_AT_L = _map2D.Get(_mapPosX - 1, _mapPosY);
            //TILE_AT_R = _map2D.Get(_mapPosX + 1, _mapPosY);
            //TILE_AT_U = _map2D.Get(_mapPosX, _mapPosY - 1);
            //TILE_AT_D = _map2D.Get(_mapPosX, _mapPosY + 1);

            return base.Update(gameTime);
        }

        public override Node Render(SpriteBatch batch)
        {

            return base.Render(batch);
        }

    }
}
