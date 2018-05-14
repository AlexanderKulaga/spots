using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Spots
{
    abstract class TheBasis
    {
        //Базовый абстрактный класс для всех остальных классов.
        //имеет координаты (верхний левый угол), размеры (ширина, высота) и картинка для отображения на экране
        //два абстрактных метода 
        //public abstract void Paint(Graphics G) - вывод на экран картинки класса. Метод абстрактрый - то есть без реализации

        //public abstract bool MouseHit(int mx, int my) - абстрактный метод проверяющий попал ли курсор мыши в поле класса. 

        protected float x;
        protected float y;
        protected float width;
        protected float height;
        protected Bitmap bitmap;

        public TheBasis( float X,  float Y, float Width, float Height, Bitmap bitmap)
        {
            x = X;
            y = Y;
            width = Width;
            height = Height;
            this.bitmap = bitmap;
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        public float Height
        {
            get { return height; }
            set { height = value; }
        }
        
        public Bitmap BITMAP
        {
            get { return bitmap; }
            set { bitmap = value; }
        }

        public abstract void Paint(Graphics G);

        public abstract bool MouseHit(int mx, int my);
    }
}
