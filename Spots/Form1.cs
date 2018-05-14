using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spots
{
    public partial class Form1 : Form
    {
        Field F;    //поле
        int Steps;  //число шагов
        int level;  //уровень (размер поля)
        Timer T;    //таймер
        bool GameStart; //показывает, что игра начата
        long Sec;   //число прошедших секунд
        public Form1()
        {
            InitializeComponent();

            GameStart = false;
            level = 3;
            Steps = 0;
            Sec = 0;

            if (level == 5)
            {
                F = new Field(0, 0, pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height, 3, GetRandomBitmap(), 2);
                Sec = 120;
            }
            else
            {
                F = new Field(0, 0, pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height, level, GetRandomBitmap(), 0);
                Sec = 0;
            }

            T = new Timer();
            T.Interval = 1000;
            T.Enabled = false;
            T.Tick += TimerTick;
            //T.Start();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //обработка клика мыши по pictureBox1


            if(F.MouseHit(e.X, e.Y)) //если было действие на поле
            {
                if (!GameStart) //если игра не начата
                {
                    GameStart = true; //начинаем игру и обнуляем все переменные
                    if (level == 5)
                        Sec = 120;
                    else
                        Sec = 0;
                    Steps = 0;
                    T.Enabled = true; //запускаем таймер
                    T.Start();
                }
                Steps++; //увеличиваем счётчик шагов
                pictureBoxScore.Invalidate();//обновляем экран
            }
            if(F.Checking()) //если поле упорядочено
            {
                T.Stop(); //останавливаем таймер
                //выводим на экран сообщение о победе
                if (level<5)
                MessageBox.Show("Вы сложили пазл за " + Steps.ToString()+" ходов и "+FormatedSec(Sec), "Поздравляем!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);
                else
                    MessageBox.Show("Вы сложили пазл за " + Steps.ToString() + " ходов и " + FormatedSec(120-Sec), "Поздравляем!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);

                //переходим на следующий уровень, который определяет размер поля
                if (level < 5)
                    level++;
                else
                    level = 3;

                //обнуляем переменные и останавливаем таймер
                GameStart = false;
                if (level == 5)
                    Sec = 120;
                else
                    Sec = 0;
                Steps = 0;
                T.Enabled = false;
                
                //создаём новое поле
                if (level == 5)
                    F = new Field(0, 0, pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height, 3, GetRandomBitmap(), 2);
                else
                    F = new Field(0, 0, pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height, level, GetRandomBitmap(), 0);
                

                //обновляем экран
                this.Invalidate();
            }
            pictureBox1.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //событие обновления главной формы

            //запрашивает события обновления для pictureBoxScore и pictureBox1
            pictureBoxScore.Invalidate();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //выводим поле на экран
            F.Paint(e.Graphics);
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            //кнопка Заного.
            //обновляет все переменные, останавливает таймер, создаёт новое поле. Переменную level не трогает
            GameStart = false;
            
            Steps = 0;
            T.Enabled = false;
            T.Stop();

            if (level == 5)
            {
                F = new Field(0, 0, pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height, 3, GetRandomBitmap(),2);
                Sec = 120;
            }
            else
            {
                F = new Field(0, 0, pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height, level, GetRandomBitmap(),0);
                Sec = 0;
            }
            this.Invalidate();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (level == 5)
            {
                if (Sec<=0)
                {
                    T.Stop();
                    MessageBox.Show("Вы проиграли!Уровень начнётся заного.", ":(",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
                    buttonRestart_Click(null, null);
                }
                else
                    Sec--;
            }
            else
                Sec++;


            pictureBoxScore.Invalidate();
        }

        private void pictureBoxScore_Paint(object sender, PaintEventArgs e)
        {
            //обработка события обновления поля очков

            //создаём формат вывода текста
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            //выводим сообщение о число прошедших секунд
            //FormatedSec() преобразует число long Sec в формат 00:00:00
            e.Graphics.DrawString(FormatedSec(Sec), new Font("Impact", 20), new SolidBrush(Color.Black),
                pictureBoxScore.ClientRectangle.Width *3/4,
                pictureBoxScore.ClientRectangle.Height/2,
                sf);

            //выводим сообщение о сделнных ходах
            e.Graphics.DrawString("Ходы: " + Steps.ToString(), new Font("Impact", 20), new SolidBrush(Color.Black),
                pictureBoxScore.ClientRectangle.Width / 4,
                pictureBoxScore.ClientRectangle.Height / 2,
                sf);
        }

        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            //кнопка Новая игра.
            //обновляет все переменные, останавливает таймер, создаёт новое поле. Переменную level изменяет на 3
            GameStart = false;
            Sec = 0;
            Steps = 0;
            T.Enabled = false;
            T.Stop();
            level = 3;
            F = new Field(0, 0, pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height, level, GetRandomBitmap(),0);
            this.Invalidate();
        }

        private string FormatedSec(long Sec)
        {
            //FormatedSec() преобразует число long Sec в формат 00:00:00

            String F = "";

            long hours = Sec / 60 / 60;
            long min = (Sec - hours * 3600) / 60;
            long sec = Sec - hours * 3600 - min * 60;

            if (hours > 9)
                F += hours.ToString() + ":";
            else
                F += "0" + hours.ToString() + ":";

            if (min > 9)
                F += min.ToString() + ":";
            else
                F += "0" + min.ToString() + ":";

            if (sec > 9)
                F += sec.ToString();
            else
                F += "0" + sec.ToString();

            return F;
        }

        private Bitmap GetRandomBitmap()
        {
            //метод возвращает случайную картинку из ресурсов.
            //R.Next(3) возвращает случайное число от 0 до 2 включительно.

            Random R = new Random();
            switch(R.Next(3))
            {
                case 0:
                    if(level == 3)
                        return Properties.Resources.Cat;
                    if(level == 4)
                        return Properties.Resources.T34;
                    if (level == 5)
                        return Properties.Resources.ЛИСА;

                    break;
                case 1:
                    if(level == 3)
                        return Properties.Resources.NyanCat;
                    if(level == 4)
                        return Properties.Resources.ИС2;
                    if (level == 5)
                        return Properties.Resources.ЛИСА2;

                    break;
                case 2:
                    if(level == 3)
                        return Properties.Resources.ДЛИННОКОТ;
                    if(level == 4)
                        return Properties.Resources.ТИГР;
                    if (level == 5)
                        return Properties.Resources.ЛИСА3;

                    break;
                default:
                    return Properties.Resources.T34;
            }

            return Properties.Resources.T34;
        }

        private void buttonRules_Click(object sender, EventArgs e)
        {
            //кнопка правила

            //создаём строку сообщения
            string Message =
            "Пятнашки - популярная головоломка, придуманная в 1878 году Ноем Чепмэном.\n" +
            "Цель игры — перемещая костяшки по полю, добиться упорядочивания их по номерам, " +
            "желательно сделав как можно меньше перемещений.";

            //создаём MessageBox с сообщением.
            MessageBox.Show(Message, "Правила игры",
                MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);
        }
    }
}
