using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Spots
{
    class Spot:TheBasis
    {
        //класс "пятнашки", или блока.
        //наследуется от TheBasis, так что сразу имеет все методы, переменные и свойства класса TheBasis
        //методы Paint и MouseHit обязательно длжны быть перезаписаны, и иметь ключевое слово override.

        int index; //номер блока
        bool showIndex;
        public Spot(float X, float Y, float Width, float Height, int Index, Bitmap bitmap)
            : base(X, Y, Width, Height, bitmap) 
        {
            index = Index;
            showIndex = true;
        }

        public override void Paint(Graphics G)
        {  
            if(index>0)
            {
                //G.DrawRectangle(new Pen(Color.FromArgb(0, 0, 0)), x, y, width, height);
                if (bitmap!=null)
                    G.DrawImage(bitmap, x, y, width, height); //вывод изображения bitmap

                String Message;
                if (showIndex)
                    Message = index.ToString();
                else
                    Message = "?";

                G.SmoothingMode = SmoothingMode.AntiAlias; //режим сглаживания

                //создаём формат отображения трок
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                //выводим номер блока в центре блока
                G.DrawString(Message, new Font("Impact", 30), new SolidBrush(Color.White),
                    x + width / 2,
                    y + height / 2,
                    sf);

                //создаём контур для номера блока
                // GraphicsPath - Представляет последовательность соединенных линий и кривых
                GraphicsPath Path = new GraphicsPath();
                Path.AddString(Message, new FontFamily("Impact"), 0, 40,
                    new Point(
                    Convert.ToInt32( x+width / 2),
                    Convert.ToInt32( y+height / 2)),
                    sf);

                //выводим контур на экран
                G.DrawPath(new  Pen(Color.Black,2),Path);

                
            }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public override bool MouseHit(int mx, int my)
        {
            //возвращаем true, если координаты мыши попали по блоку
            if (mx >= x && mx < x + width && my >= y && my < y + height)
            {
                return true;
            }

            return false;
        }

        public void Rearrange(Spot S)
        {
            //этот метод меняет местами координаты текущего блока и блока S.
            float tx = x;
            float ty = y;
            float tw = width;
            float th = height;

            x = S.X;
            y = S.Y;
            width = S.Width;
            height = S.Height;

            S.X = tx;
            S.Y = ty;
            S.Width = tw;
            S.Height = th;
        }

        public bool ShowIndex
        {
            get { return showIndex; }
            set { showIndex = value; }
        }
    }
}
