using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form2 : Form
    {
        List<TextBox> TB = new List<TextBox>();//Список textBox-ов на panel
        int n, time = 1;//n-число состояний в потоке

        public Form2(string mess)
        {
            InitializeComponent();
            try { n = Convert.ToInt32(mess); }
            catch (System.FormatException)
            { Form1 NEWForm = new Form1(this); NEWForm.Show(); this.Hide(); }
            VvodOnPanelL(n, 40);
        }

        public void VvodOnPanelP(int n, int h, int m)//Построение на panel textBox-ов и label-ов размерности n*n
        {
            for (int j = 0; j < n; j++)
                for (int i = 0; i < n; i++)
                {
                    Label lb = new Label();
                    lb.Location = new Point(128 * j + 5, h + 3 + 23 * i);
                    lb.Text = "P" + Convert.ToString(m) + "(λ" + Convert.ToString(i + 1) + "|λ" + Convert.ToString(j + 1) + "):";
                    lb.Width = 53;
                    lb.Height = 20;
                    panel1.Controls.Add(lb);
                    TextBox tb = new TextBox();
                    tb.Location = new Point(58 * (j + 1) + 70 * j, h + 3 + 23 * i);
                    tb.Width = 70;
                    tb.Height = 20;
                    tb.MaxLength = 4;
                    tb.TextAlign = HorizontalAlignment.Center;
                    panel1.Controls.Add(tb);
                    TB.Add(tb);
                }
        }

        public void VvodOnPanelL(int n, int h)//Построение на panel textBox-ов и label-ов размерности n*1
        {
            for (int i = 0; i < n; i++)
            {
                Label lb = new Label();
                lb.Location = new Point(128 * i + 5, h + 3);
                lb.Text = "λ" + Convert.ToString(i + 1) + ":";
                lb.Width = 53;
                lb.Height = 20;
                panel1.Controls.Add(lb);
                TextBox tb = new TextBox();
                tb.Location = new Point(58 * (i + 1) + 70 * i, h + 3);
                tb.Width = 70;
                tb.Height = 20;
                tb.MaxLength = 4;
                tb.TextAlign = HorizontalAlignment.Center;
                panel1.Controls.Add(tb);
                TB.Add(tb);
            }
        }

        private void button1_Click(object sender, EventArgs e)//Проверка формата
        {
            for (int i = 0; i < TB.Count; i++)
                TB[i].ReadOnly = true;
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            button3.Enabled = true;
            try
            {
                bool M = false;//Отвечает за верность введенных данных
                if (textBox1.Text == "" || textBox2.Text == "")//Проверка на заполнение всех полей
                { M = true; label4.Text = "Не все поля заполнены. Попробуйте снова!"; }
                for (int i = 0; i < TB.Count && M == false; i++)//Проверка на заполнение всех полей
                    if (TB[i].Text == "")
                    {
                        label4.Text = "Не все поля заполнены. Попробуйте снова!";
                        if (time == 2) time = 3; M = true;
                    }
                if (M == false)
                {
                    if (time == 2)
                    {
                        button1.Enabled = false; time = 3;
                        for (int j = 0; j < n && M == false; j++)//Проверка условия нормировки
                        {
                            int SUM = 0;
                            for (int i = n * (j + 1); i < n * (j + 2); i++)
                                SUM = SUM + Convert.ToInt32(Convert.ToDouble(TB[i].Text) * 100);
                            for (int i = n * (n + j + 1); i < n * (n + j + 2); i++)
                                SUM = SUM + Convert.ToInt32(Convert.ToDouble(TB[i].Text) * 100);
                            if (SUM != 100) { M = true; label4.Text = "Введены некорректные данные. Попробуйте снова!"; }

                        }
                        if (M == false) { label4.Text = "Параметры потока приняты!"; button2.Enabled = true; }
                    }
                    else
                    {
                        for (int i = 0; i < TB.Count && M == false; i++)//Проверка правильности введенных параметров
                            if (Convert.ToInt32(TB[i].Text) <= 0) M = true;
                        if (M == true) label4.Text = "Введены некорректные данные. Попробуйте снова!";
                        else
                        {
                            time = 2;
                            double[] L = new double[n];//Матрица λ
                            for (int i = 0; i < n; i++)
                                L[i] = Convert.ToDouble(TB[i].Text);
                            int R = 1, Rt = 0;//Упорядочивание λ в порядке убывания методом пузырька
                            while (R >= 0)
                            {
                                if (R == Rt) R = -1;
                                else
                                {
                                    R = Rt;
                                    for (int i = n - 1; i < n && i > R; i--)
                                        if (L[i - 1] < L[i])
                                        { double Tmp1; Rt = i; Tmp1 = L[i]; L[i] = L[i - 1]; L[i - 1] = Tmp1; }
                                }
                            }
                            for (int i = 0; i < n - 1 && M == false; i++)
                                if (L[i] == L[i + 1])
                                {
                                    M = true; label4.Text = "Есть повторяющиеся элементы λ i-ые. Попробуйте снова!";
                                    time = 1; button3.Enabled = false;
                                }
                            if (time == 2)
                            {
                                for (int i = 0; i < n; i++) TB[i].Text = Convert.ToString(L[i]);
                                VvodOnPanelP(n, 40 + 23, 0);
                                VvodOnPanelP(n, 40 + 23 * (n + 1), 1);
                                label4.Text = "Параметры потока приняты. Введите переходные вероятности!";
                            }
                        }
                    }
                }
            }
            catch (System.FormatException) { label4.Text = "Введены некорректные данные. Попробуйте снова!"; }
            catch (System.OverflowException) { label4.Text = "Введены некорректные данные. Попробуйте снова!"; }
        }

        private void button2_Click(object sender, EventArgs e)//Построение модели
        {
            button2.Enabled = false;
            label5.Text = "Обобщенный МАР-поток событий с " + Convert.ToString(n) + " состояниями:";
            Pen myPen = new Pen(Color.Black, 1);
            Pen myPen1 = new Pen(Color.DarkBlue, 2);
            Pen myPen2 = new Pen(Color.Red, 1);
            myPen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Graphics graph = pictureBox1.CreateGraphics();
            graph.DrawLine(myPen, 20, 230, 960, 230);
            graph.DrawLine(myPen, 952, 227, 960, 230);
            graph.DrawLine(myPen, 952, 233, 960, 230);
            graph.DrawLine(myPen, 20, 230, 20, 20);
            graph.DrawLine(myPen, 17, 28, 20, 20);
            graph.DrawLine(myPen, 23, 28, 20, 20);
            graph.DrawEllipse(myPen, 18, 228, 4, 4);
            graph.FillEllipse(Brushes.White, 18, 228, 4, 4);
            graph.DrawString("t0", new Font("Times New Roman", 10), new SolidBrush(Color.Black), 13, 233);
            for (int i = 0; i < 3; i++)
            {
                graph.DrawEllipse(myPen, 935 + 5 * i, 238, 3, 3);
                graph.FillEllipse(Brushes.Black, 935 + 5 * i, 238, 3, 3);
            }
            graph.DrawString("t", new Font("Times New Roman", 12), new SolidBrush(Color.Black), 950, 230);
            graph.DrawString("λ(t)", new Font("Times New Roman", 12), new SolidBrush(Color.Black), 10, 0);
            for (int i = 0; i < n; i++)
                graph.DrawString(Convert.ToString(n - i), new Font("Times New Roman", 10), new SolidBrush(Color.Black), 0, 225 - 200 / n * (i + 1));
            int d = 18;
            double[] tau = new double[n];//Длина времени, проведенного в i-ом состоянии
            double[] L = new double[n];//Матрица λ
            for (int i = 0; i < n; i++)
                L[i] = Convert.ToDouble(TB[i].Text);
            double[,] P = new double[2 * n, n];//Матрица переходных вероятностей
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    P[j, i] = Convert.ToDouble(TB[(i + 1) * n + j].Text);
                    P[j + n, i] = Convert.ToDouble(TB[(i + 1 + n) * n + j].Text);
                }
            double T = Convert.ToDouble(textBox1.Text);//Конечное время
            int N = Convert.ToInt32(textBox2.Text);//Число опытов
            int col = 0;//Количество событий в потоке
            long idum = 1235;
            for (int u = 0; u < N; u++)
            {
                double t = 0;//Текущее время
                double x = WindowsFormsApplication3.Random.ran1(ref idum);//Счетчик псевдослучайных чисел от 0 до 1
                int k;
                if (x <= Convert.ToDouble(1) / Convert.ToDouble(n)) k = 1;
                else
                    for (k = 2; k <= n; k++)
                        if ((x > Convert.ToDouble(k - 1) / Convert.ToDouble(n)) && (x <= Convert.ToDouble(k) / Convert.ToDouble(n)))
                            break;
                while (t < T)
                {
                    if (u == 0) myPen1.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                    double TAU = -1 / L[k - 1] * Math.Log10(1 - x);
                    t = t + TAU;
                    tau[k - 1] = tau[k - 1] + TAU;
                    if (t >= T)
                    {
                        tau[k - 1] = tau[k - 1] - (t - T);
                        t = T;
                        if (u == 0)
                            graph.DrawLine(myPen1, Convert.ToInt32(20 + (t - TAU) * 200), 232 - 200 / n * (n - k + 1), Convert.ToInt32(20 + t * 200), 232 - 200 / n * (n - k + 1));
                        break;
                    }
                    if (u == 0)
                        graph.DrawLine(myPen1, Convert.ToInt32(20 + (t - TAU) * 200), 232 - 200 / n * (n - k + 1), Convert.ToInt32(20 + t * 200), 232 - 200 / n * (n - k + 1));
                    x = WindowsFormsApplication3.Random.ran1(ref idum);
                    double SUM0 = 0;
                    for (int i = 0; i < n; i++)
                        SUM0 = SUM0 + P[i, k - 1];
                    int l = 1;
                    if (x <= SUM0)
                    {
                        double SUM2 = P[l - 1, k - 1];
                        if (x <= SUM2) l = 1;
                        else
                        {
                            for (l = 2; l <= n; l++)
                            {
                                SUM2 = SUM2 + P[l - 1, k - 1];
                                if ((x > SUM2 - P[l - 1, k - 1]) && (x <= SUM2)) break;
                            }
                        }
                    }
                    else
                    {
                        col++;
                        if (u == 0)
                        {
                            dataGridView1.Rows.Add(col, t);
                            graph.DrawLine(myPen2, Convert.ToInt32(20 + t * 200), 232 - 200 / n * (n - k + 1), Convert.ToInt32(20 + t * 200), 230);
                            if ((20 + t * 200) <= 930)
                            {
                                graph.DrawEllipse(myPen, Convert.ToInt32(18 + t * 200), 228, 4, 4);
                                graph.FillEllipse(Brushes.White, Convert.ToInt32(18 + t * 200), 228, 4, 4);
                                if (d <= (t * 200))
                                {
                                    graph.DrawString("t" + Convert.ToString(col), new Font("Times New Roman", 10), new SolidBrush(Color.Black), Convert.ToInt32(14 + t * 200), 233);
                                    d = Convert.ToInt32(16 + t * 200);
                                }
                            }
                        }
                        double SUM2 = 0;
                        for (l = 1; l <= n; l++)
                        {
                            SUM2 = SUM2 + P[l - 1 + n, k - 1];
                            if ((x > SUM0 + SUM2 - P[l - 1 + n, k - 1]) && (x <= SUM0 + SUM2))
                                break;
                        }
                    }
                    if (u == 0)
                    {
                        myPen1.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        graph.DrawLine(myPen1, Convert.ToInt32(20 + t * 200), 232 - 200 / n * (n - k + 1), Convert.ToInt32(20 + t * 200), 232 - 200 / n * (n - l + 1));
                    }
                    k = l;
                }
                if (u == 0)
                {
                    for (int i = 0; i < n; i++)
                        dataGridView2.Rows.Add(i + 1, tau[i]);
                    label6.Text = "Количество событий в потоке для одной реализации равно " + Convert.ToString(col);
                }
            }
            for (int i = 0; i < n; i++)
                dataGridView3.Rows.Add(i + 1, tau[i] / N);
            label7.Text = "Оценка количества событий в потоке равна " + Convert.ToString(col / N) + " (на основании N = " + textBox2.Text + " опытов)";
            if (n == 3) { button5.Enabled = true; button6.Enabled = true; }
        }

        private void button3_Click(object sender, EventArgs e)//Изменение внесенных данных
        {
            button5.Enabled = false;
            if (time == 3)
            {
                for (int i = n; i < TB.Count; i++)
                    TB[i].ReadOnly = false;
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                time = 2;
            }
            else
            {
                if (time == 2)
                {
                    TB.RemoveRange(n, 2 * n * n);
                    for (int i = 0; i < 4 * n * n; i++)
                        panel1.Controls.RemoveAt(panel1.Controls.Count - 1);
                }
                time = 1;
                for (int i = 0; i < TB.Count; i++)
                    TB[i].ReadOnly = false;
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                button3.Enabled = false;
            }
            pictureBox1.Image = null;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            button2.Enabled = false;
            button1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)//Очистка внесенных данных, возвращение на Form1
        {
            button5.Enabled = false;
            button6.Enabled = false;
            TB = null;
            n = 0; time = 1;
            Form1 NEWForm = new Form1(this);
            NEWForm.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try { if (Convert.ToDouble(textBox1.Text) <= 0) textBox1.Clear(); }
            catch (System.FormatException) { textBox1.Clear(); }
        }

        private void button5_Click(object sender, EventArgs e) //Оценка процесса
        {
            button7.Enabled = true;
            button7.Visible = true;
            button5.Enabled = false;
            button6.Enabled = true;
            panel1.Visible = false;
            panel1.Visible = false;
            panel1.Visible = false;
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            label4.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            label5.Location = new Point(12, 0);
            pictureBox1.Location = new Point(8, 15);

            Label LB = new Label();
            LB.Location = new Point(12, 290);
            LB.Width = 35;
            LB.Height = 13;
            LB.Text = "Поведение оценки процесса λ(t):";
            this.Controls.Add(LB);
            PictureBox pictureBox2 = new PictureBox();
            pictureBox2.Location = new Point(8, 310);
            pictureBox2.Width = 970;
            pictureBox2.Height = 250;
            this.Controls.Add(pictureBox2);

            Pen myPen = new Pen(Color.Black, 1);
            Pen myPen1 = new Pen(Color.DarkBlue, 2);
            Pen myPen2 = new Pen(Color.Red, 1);
            myPen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Graphics graph = pictureBox2.CreateGraphics();
            graph.DrawLine(myPen, 20, 230, 960, 230);
            graph.DrawLine(myPen, 952, 227, 960, 230);
            graph.DrawLine(myPen, 952, 233, 960, 230);
            graph.DrawLine(myPen, 20, 230, 20, 20);
            graph.DrawLine(myPen, 17, 28, 20, 20);
            graph.DrawLine(myPen, 23, 28, 20, 20);
            graph.DrawEllipse(myPen, 18, 228, 4, 4);
            graph.FillEllipse(Brushes.White, 18, 228, 4, 4);
            graph.DrawString("t0", new Font("Times New Roman", 10), new SolidBrush(Color.Black), 13, 233);
            for (int i = 0; i < 3; i++)
            {
                graph.DrawEllipse(myPen, 935 + 5 * i, 238, 3, 3);
                graph.FillEllipse(Brushes.Black, 935 + 5 * i, 238, 3, 3);
            }
            graph.DrawString("t", new Font("Times New Roman", 12), new SolidBrush(Color.Black), 950, 230);
            graph.DrawString("оценка λ(t)", new Font("Times New Roman", 12), new SolidBrush(Color.Black), 10, 0);
            for (int i = 0; i < n; i++)
                graph.DrawString(Convert.ToString(n - i), new Font("Times New Roman", 10), new SolidBrush(Color.Black), 0, 225 - 200 / n * (i + 1));
            
        }

        private void button6_Click(object sender, EventArgs e) //Апостериорная вероятность
        {
            button7.Enabled = true;
            button7.Visible = true;
            button6.Enabled = false;
            button5.Enabled = true;
            panel1.Visible = false;
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            label4.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            label5.Location = new Point(12, 0);
            pictureBox1.Location = new Point(8, 15);
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            button7.Visible = false;
            panel1.Visible = true;
            dataGridView1.Visible = true;
            dataGridView2.Visible = true;
            dataGridView3.Visible = true;
            label4.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            label5.Location=new Point(12,277);
            pictureBox1.Location = new Point(8, 299);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try { if (Convert.ToInt32(textBox2.Text) <= 0) textBox2.Clear(); }
            catch (System.FormatException) { textBox2.Clear(); }
        }
    }
}
