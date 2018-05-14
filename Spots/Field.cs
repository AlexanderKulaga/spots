using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Spots
{
    class Field : TheBasis
    {
        //Класс поля игры, наследует TheBasis

        Spot[,] Spots; //массив блоков
        public Field(float X, float Y, float Width, float Height, int n, Bitmap bitmap, int Mistery)
            : base(X, Y, Width, Height, bitmap) 
        {
            
            Spots = new Spot[n, n];
            int count = 1;

            //создаём пятнашки
            for (int i = 0; i < Spots.GetLength(0); i++)
            for (int j = 0; j < Spots.GetLength(1); j++)
                {
                    //счётчик
                    if (count == n * n)
                        count = 0; //если дошли до конца, то обнуляем счётчик, чтобы создать нулевой (пустой) блок
                    
                    //создаём новый блок
                    Spots[i, j] = new Spot(i * (Width / n), j * (Height / n), Width / n, Height / n, count++, null);

                    //создаём новую картинку для блока. метод Clone копирует картинку либо её часть.
                    if (bitmap != null)
                        Spots[i, j].BITMAP = bitmap.Clone(
                            new RectangleF(                             //размеры копируемой части
                            j * bitmap.Width / n,                       //х
                            i * bitmap.Height / n,                      //у
                            bitmap.Width / n,                           //ширина
                            bitmap.Height / n),                         //высота
                            System.Drawing.Imaging.PixelFormat.DontCare //формат пикселей
                            ); 
                }

            
            //перемешиваем блоки
            Random R = new Random();
            
            for (int c = 0; c < n * n; c++ )
            {
                //случайные индексы блоков 1 и 2
                int ri1 = R.Next(n);
                int rj1 = R.Next(n);
                int ri2 = R.Next(n);
                int rj2 = R.Next(n);

                //меняем местами блоки координаты блоков
                Spots[ri1, rj1].Rearrange(Spots[ri2, rj2]);

                if (Mistery > 0 && Spots[ri1, rj1].Index > 0)
                {
                    Spots[ri1, rj1].ShowIndex = false;
                    Mistery--;
                }

                //меняем местами расположение блоков в массиве
                SwapSpots(ri1, rj1, ri2, rj2);
            }
        }

        public override void Paint(Graphics G)
        {
            //выводим прямоугольник поля на экран
            G.DrawRectangle(new Pen(Color.FromArgb(0, 0, 0)), x, y, width, height);

            //выводим все блоки на экран
            for (int i = 0; i < Spots.GetLength(0); i++)
                for (int j = 0; j < Spots.GetLength(1); j++)
                    Spots[i,j].Paint(G);
        }

        public override bool MouseHit(int mx, int my)
        {
            //метод обрабатывает координаты мыши
            //в отличает от блоков, реакция на мышь у поля другая - перемещение блока на пусток место
            //технически - это обмен блока с нулевым блоком местами.

            for (int i = 0; i < Spots.GetLength(0); i++)
                for (int j = 0; j < Spots.GetLength(1); j++)
                    //если координаты попали в блок и номер блока больше нуля
                    if (Spots[i,j].Index>0 && Spots[i,j].MouseHit(mx,my))
                    {
                        //проверяем есть ли нулевой блок левее
                        if (i > 0 && Spots[i - 1, j].Index == 0)
                        {
                            //меняем местами координаты текущего и нулевого блока
                            Spots[i, j].Rearrange(Spots[i - 1, j]);

                            //меняем местами эти блоки в массиве
                            SwapSpots(i, j, i - 1, j);

                            //возвращаем true - обмен блоков произошел
                            return true;
                        }

                        //проверяем есть ли нулевой блок правее
                        if (i < Spots.GetLength(0) - 1 && Spots[i + 1, j].Index == 0)
                        {
                            Spots[i, j].Rearrange(Spots[i + 1, j]);
                            SwapSpots(i, j, i + 1, j);
                            return true;
                        }

                        //проверяем есть ли нулевой блок выше
                        if (j > 0 && Spots[i, j - 1].Index == 0)
                        {
                            Spots[i, j].Rearrange(Spots[i , j - 1]);
                            SwapSpots(i, j, i, j-1);
                            return true;
                        }

                        //проверяем есть ли нулевой блок ниже
                        if (j < Spots.GetLength(1) - 1 && Spots[i , j+1].Index == 0)
                        {
                            Spots[i, j].Rearrange(Spots[i, j+1]);
                            SwapSpots(i, j, i, j+1);
                            return true;
                        }


                        return false;
                    }

            return false;
        }

        public bool Checking()
        {
            //метод проверяет упорядоченность блоков
            //метод проверят последовательно каждый блок и сравнивает его индекс с номером, который должен быть в клетке. Если они не равны
            //то поле неупорядочено ещё и нужно вернуть false
            //если удалось дойти до конца - значит, блоки упорядочены

            int count = 1; //счётчик блоков
            
            //два цикла идут по порядку расположения блоков
                for (int j = 0; j < Spots.GetLength(1); j++)
                    for (int i = 0; i < Spots.GetLength(0); i++)
                {
                    if (Spots[i, j].Index == count) //если счётчик  равен номеру блока, то
                    {
                        count++; //увеличиваем счётчик на 1
                        if (count == Spots.Length) //если счётчик равен размеру массива блоков
                            return true; // поле упорядочено
                    }
                    else //(если счётчик count не равен индексу блока, значит поле не упорядоченно)
                        return false;
                }

            return true;
        }

        private void SwapSpots(int i1,int j1, int i2,int j2)
        {
            //метод меняет блок i1,j1 в массиве Spots местами с i2,j2

            Spot S = Spots[i1,j1];
            Spots[i1, j1] = Spots[i2, j2];
            Spots[i2, j2] = S;
        }
    }
}
