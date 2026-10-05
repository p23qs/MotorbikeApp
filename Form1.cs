using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MotorbikeApp.folferformodel;

namespace MotorbikeApp
{
    public partial class Form1 : Form
    {
        MODELbd db = new MODELbd();
        List<Table_Motorbike> allList;   // все записи из БД
        List<Table_Motorbike> list;      // записи после фильтрации
        int index = 0;

        MotorbikeTile[] tiles = new MotorbikeTile[3];
        Button btnLeft, btnRight, btnApply, btnReset;
        ComboBox cbBrand;
        TextBox tbModel, tbPriceMax, tbPowerMin, tbMileageMax;
        Label lblCount;

        public Form1()
        {
            InitializeComponent();

            Text = "Мотоциклы";
            ClientSize = new Size(900, 420);

            BuildFilterPanel();

            btnLeft = new Button { Text = "<", Location = new Point(10, 230), Size = new Size(40, 60) };
            btnRight = new Button { Text = ">", Location = new Point(850, 230), Size = new Size(40, 60) };
            btnLeft.Click += btnLeft_Click;
            btnRight.Click += btnRight_Click;
            Controls.Add(btnLeft);
            Controls.Add(btnRight);

            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new MotorbikeTile { Location = new Point(60 + i * 260, 110) };
                Controls.Add(tiles[i]);
            }

            Load += Form1_Load;
        }

        // Панель фильтров сверху
        void BuildFilterPanel()
        {
            AddLabel("Бренд", 10, 10);
            cbBrand = new ComboBox { Location = new Point(10, 30), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cbBrand);

            AddLabel("Модель", 150, 10);
            tbModel = new TextBox { Location = new Point(150, 30), Width = 130 };
            Controls.Add(tbModel);

            AddLabel("Цена до, руб.", 290, 10);
            tbPriceMax = new TextBox { Location = new Point(290, 30), Width = 100 };
            Controls.Add(tbPriceMax);

            AddLabel("Мощность от, л.с.", 400, 10);
            tbPowerMin = new TextBox { Location = new Point(400, 30), Width = 100 };
            Controls.Add(tbPowerMin);

            AddLabel("Пробег до, км", 510, 10);
            tbMileageMax = new TextBox { Location = new Point(510, 30), Width = 100 };
            Controls.Add(tbMileageMax);

            btnApply = new Button { Text = "Применить", Location = new Point(630, 28), Size = new Size(100, 27) };
            btnReset = new Button { Text = "Сбросить", Location = new Point(740, 28), Size = new Size(100, 27) };
            btnApply.Click += btnApply_Click;
            btnReset.Click += btnReset_Click;
            Controls.Add(btnApply);
            Controls.Add(btnReset);

            lblCount = new Label { Location = new Point(10, 70), AutoSize = true };
            Controls.Add(lblCount);
        }

        void AddLabel(string text, int x, int y)
        {
            Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            allList = db.Table_Motorbike.ToList();

            cbBrand.Items.Add("Все");
            foreach (string b in allList.Select(m => m.Brand.Trim()).Distinct().OrderBy(b => b))
                cbBrand.Items.Add(b);
            cbBrand.SelectedIndex = 0;

            ApplyFilter();
        }

        // Фильтрация списка по введённым параметрам
        void ApplyFilter()
        {
            IEnumerable<Table_Motorbike> q = allList;

            if (cbBrand.SelectedIndex > 0)
            {
                string brand = cbBrand.SelectedItem.ToString();
                q = q.Where(m => m.Brand.Trim() == brand);
            }

            string model = tbModel.Text.Trim();
            if (model != "")
                q = q.Where(m => m.Model.IndexOf(model, StringComparison.OrdinalIgnoreCase) >= 0);

            int priceMax, powerMin, mileageMax;
            if (int.TryParse(tbPriceMax.Text, out priceMax))
                q = q.Where(m => m.Price <= priceMax);
            if (int.TryParse(tbPowerMin.Text, out powerMin))
                q = q.Where(m => m.Horsepower >= powerMin);
            if (int.TryParse(tbMileageMax.Text, out mileageMax))
                q = q.Where(m => m.Mileage <= mileageMax);

            list = q.ToList();
            index = 0;
            lblCount.Text = "Найдено: " + list.Count;
            ShowData();
        }

        // Заполнение плиток
        void ShowData()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                int n = index + i;
                tiles[i].SetData(n < list.Count ? list[n] : null);
            }
        }

        // Перегрузка: true - вперёд, false - назад
        void ShowData(bool next)
        {
            if (next && index + tiles.Length < list.Count) index++;
            else if (!next && index > 0) index--;
            ShowData();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            ShowData(false);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            ShowData(true);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cbBrand.SelectedIndex = 0;
            tbModel.Text = "";
            tbPriceMax.Text = "";
            tbPowerMin.Text = "";
            tbMileageMax.Text = "";
            ApplyFilter();
        }
    }
}