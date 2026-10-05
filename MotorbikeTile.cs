using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MotorbikeApp.folferformodel;

namespace MotorbikeApp
{
    public class MotorbikeTile : UserControl
    {
        private PictureBox pictureBox1;
        private Label label1;

        public MotorbikeTile()
        {
            Size = new Size(230, 280);
            BorderStyle = BorderStyle.FixedSingle;

            pictureBox1 = new PictureBox
            {
                Location = new Point(5, 5),
                Size = new Size(218, 150),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            label1 = new Label
            {
                Location = new Point(5, 160),
                Size = new Size(218, 115),
                Font = new Font("Microsoft Sans Serif", 10f)
            };
            Controls.Add(pictureBox1);
            Controls.Add(label1);
        }

        // Заполняет плитку данными одного мотоцикла
        public void SetData(Table_Motorbike m)
        {
            if (m == null)
            {
                label1.Text = "";
                pictureBox1.Image = null;
                return;
            }

            label1.Text = m.Brand + " " + m.Model + "\n" +
                          "Цена: " + m.Price + " руб.\n" +
                          "Мощность: " + m.Horsepower + " л.с.\n" +
                          "Пробег: " + m.Mileage + " км";

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pictures", m.Picture.Trim());
            pictureBox1.Image = File.Exists(path) ? Image.FromFile(path) : null;
        }
    }
}
