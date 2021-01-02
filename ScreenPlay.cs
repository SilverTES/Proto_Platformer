using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Retro2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto_00
{
    class ScreenPlay : Node
    {

        Node _level;

        Gui.Text _text;

        public ScreenPlay(ContentManager content)
        {
            SetSize(Game1._screenW, Game1._screenH);

            _level = new Level(content).Init();

            _text = (Gui.Text)new Gui.Text(Game1._mouseInput)
                .This<Gui.Text>().SetClickable(true)
                .This<Gui.Text>().SetStyle(Game1._style)
                .AppendTo(this);

            _text._style._horizontalAlign = Style.HorizontalAlign.Center;

            _text.SetLabel("ScreenPlay").SetPosition(Game1._screenW / 2, 8);
        }

        public override Node Update(GameTime gameTime)
        {
            _level.Update(gameTime);

            if (_text.IsMouseOver)
                _text._style._color = Style.ColorValue.MakeColor(Color.White);
            else
                _text._style._color = Style.ColorValue.MakeColor(Color.MonoGameOrange);

            UpdateChilds(gameTime);

            return base.Update(gameTime);
        }

        public override Node Render(SpriteBatch batch)
        {
            //batch.GraphicsDevice.Clear(new Color(0, 20, 40));

            //Draw.Grid(batch, 0, 0, Game1._screenW, Game1._screenH, 64, 64, Color.Gray * .4f);

            _level.Render(batch);

            RenderChilds(batch);

            //Draw.Rectangle(batch, _text.GetLabelRect(), Color.Red);
            //Draw.Rectangle(batch, _text.AbsRect, Color.Orange);

            return base.Render(batch);
        }

    }
}
