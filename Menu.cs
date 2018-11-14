using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Miner
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Would you like to see tutorial?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Tutorial tut = new Tutorial();
                tut.ShowDialog();
            }
            string name = (string)textBoxAddName.Text;
            string time = "";
            if (name != "" && comboBoxMapSize.Text != "")
            {                
                GameScreen game = new GameScreen();
                for (int i = 0; i < mapSize.Length; i++)
                {
                    if (comboBoxMapSize.Text == mapSize[i].name)
                    {
                        game.HeightBlockSky = mapSize[i].heightSky;
                        game.BlockHeight = mapSize[i].height;
                        game.BlockWidth = mapSize[i].width;
                        break;
                    }
                }
                for (int i = 0; i < dif.Length; i++)
                {
                    if (comboBoxDifficulty.Text == dif[i].name)
                    {
                        game.multiplication = dif[i].multiplication;                       
                        break;
                    }
                }
                game.PlayerName = name;          
                game.Style3DClick = Style3D.Checked;
               
                game.ShowDialog();
                time = game.OutTimeString;
                if(game.TheGold == 0 && !game.Die)
                {
                    dataGridView1.Rows.Add(name, time, comboBoxMapSize.Text, comboBoxDifficulty.Text);
                }
            }
            else
            {
                MessageBox.Show("Incorrect type the name or map");
            }
        }

        struct Difficulty
        {
            public string name;
            public double multiplication;           
        }

        struct MapSize
        {
            public string name;
            public int height;
            public int width;
            public int heightSky;
        }

        MapSize[] mapSize;
        Difficulty[] dif;

        private void Menu_Load(object sender, EventArgs e)
        {           
            string[] MapSizeString = Miner.Properties.Resources.MapSize.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] difficulty = Miner.Properties.Resources.Difficulty.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            mapSize = new MapSize[MapSizeString.Length];
            dif = new Difficulty[difficulty.Length];
            try
            {
                for (int i = 0; i < dif.Length; i++)
                {
                    string[] split = difficulty[i].Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                    dif[i].name = split[0];
                    dif[i].multiplication = Convert.ToDouble(split[1]);
                    comboBoxDifficulty.Items.Add(dif[i].name);
                    comboBoxDifficulty.Text = dif[i].name;
                }
            }
            catch
            {
                MessageBox.Show("Difficulty are set incorrectly");
                Close();
            }
            try
            {
                for (int i = 0; i < MapSizeString.Length; i++)
                {
                    string[] split = MapSizeString[i].Split(' ');
                    mapSize[i].name = split[0];
                    mapSize[i].height = Convert.ToInt32(split[1]);
                    mapSize[i].width = Convert.ToInt32(split[2]);
                    mapSize[i].heightSky = Convert.ToInt32(split[3]);
                    comboBoxMapSize.Items.Add(mapSize[i].name);
                    comboBoxMapSize.Text = mapSize[i].name;
                }
            }
            catch
            {
                MessageBox.Show("Map sizes are set incorrectly");
                Close();
            }
            using (BinaryReader Records = new BinaryReader(new FileStream("Record.dat", FileMode.Open)))
            {
                for (; ; )
                {
                    try
                    {
                        string read = Records.ReadString();
                        string[] split = read.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        dataGridView1.Rows.Add(split[0], split[1], split[2], split[3]);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (BinaryWriter RecordsWriter = new BinaryWriter(new FileStream("Record.dat", FileMode.Create)))
            {           
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    RecordsWriter.Write(dataGridView1.Rows[i].Cells[0].Value + " " + dataGridView1.Rows[i].Cells[1].Value + " " + dataGridView1.Rows[i].Cells[2].Value + " " + dataGridView1.Rows[i].Cells[3].Value);
                }           
            }            
        }
    }
}
