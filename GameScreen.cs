using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Threading;


namespace Miner
{
    public partial class GameScreen : Form
    {
        public GameScreen()
        {
            InitializeComponent();
        }

        public int HeightBlockSky;
        public int BlockHeight;
        public int BlockWidth;
        public string PlayerName;
        public double multiplication;
        public int PlayerTime;
        public string OutTimeString;
        public int TheGold;
        public bool Die;
      
        Image Grass = Miner.Properties.Resources.Grass;
        Image EarthBackground = Miner.Properties.Resources.EarthBackground;
        Image Earth = Miner.Properties.Resources.Earth;
        Image Gold = Miner.Properties.Resources.Gold;
        Image TimeLock = Miner.Properties.Resources.TimeLock;
        Image Sprint = Miner.Properties.Resources.Sprint;        
        Image Sky = Miner.Properties.Resources.Sky;
        Image Music = Miner.Properties.Resources.Music;
        Image Mine = Miner.Properties.Resources.Mine;
        Image Obsidian = Miner.Properties.Resources.Obsidian;
        Image TrueSight = Miner.Properties.Resources.TrueSight; 

        Image PlayerStand = Miner.Properties.Resources.Stand;
        Image PlayerStandJump = Miner.Properties.Resources.StandJump;
        Image[] PlayerStandSmokeAnimation = new Image[6];
        Image[] AnimationStar = new Image[6];
        Image PlayerLeft = Miner.Properties.Resources.Left;
        Image PlayerLeft1 = Miner.Properties.Resources.Left1;
        Image PlayerLeft2 = Miner.Properties.Resources.Left2;
        Image PlayerLeftJump = Miner.Properties.Resources.LeftJump;
        Image PlayerRight = Miner.Properties.Resources.Right;
        Image PlayerRight1 = Miner.Properties.Resources.Right1;
        Image PlayerRight2 = Miner.Properties.Resources.Right2;
        Image PlayerRightJump = Miner.Properties.Resources.RightJump;
        Image ClickUp = Miner.Properties.Resources.ClickUp;
        Image ClickDown = Miner.Properties.Resources.ClickDown;
        Image ClickLeft = Miner.Properties.Resources.ClickLeft;
        Image ClickRight = Miner.Properties.Resources.ClickRight;
        Image ClickUpLeft = Miner.Properties.Resources.ClickUpLeft;
        Image ClickUpRight = Miner.Properties.Resources.ClickUpRight;
        Image ClickDownLeft = Miner.Properties.Resources.ClickDownLeft;
        Image ClickDownRight = Miner.Properties.Resources.ClickDownRight;
      
        enum ManPosition {Stand, Left, Right, ClickUp, ClickDown, ClickLeft, ClickRight, ClickUpLeft, ClickUpRight, ClickDownLeft, ClickDownRight}
                
        struct GameBlock
        {
            public GraphicsPath path;
            public bool impact;
            public TypeBlock type;
            public Point[] points;
        }
        enum TypeBlock { Sky, Grass, EarthBackground, Earth, Gold, TimeLock, Sprint, Music, Mine, Obsidian,TrueSight }
                      
        GameBlock[,] block;
        SolidBrush BrushTextScreen;
        Font FontTextScreen;
        int TimeRune = 0;
        int TimeLockTime = 0;        
        int SprintTime = 0;
        int ManyGeneration = 3;
        int RuneGeneration = 20;
        int MineGeneration = 0;
        double BlockSize;
        int TimeSmoke = 0;
        int TimeTrueSight = 0;
        int SmokeStand = 0;
        public bool Style3DClick;
        double PlayerSpeed = 1;
        double PlayerSpeedRune = 0;
        int jump = 0;
        int timeJump = 0;
        int Block_X = 0;
        int Block_Y = 0;
        int AnimationRun = 0;
        int AnimationPickClick = 0;
        Point AnimationPick = new Point();
        GraphicsPath RangePick = new GraphicsPath();
        Point[] pointsLeftImpact;
        Point[] pointsRightImpact;
        Point[] pointsUpImpact;
        Point[] pointsDownImpact;
        bool RangePickVisible = false;
        bool GoLeft;
        bool GoRight;
        bool GoUp;
        bool GoDown;
        SoundPlayer music1;
        SoundPlayer music2;

        public void music()
        {
           
                Random rnd = new Random();
                if (rnd.Next(101) >= 30)
                {
                     music1.Play();
                }
                else
                {
                    music2.Play();
                }
           
 
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {           
            Random RandomSpawn = new Random();
            music1 = new SoundPlayer(Miner.Properties.Resources.music1);
            music2 = new SoundPlayer(Miner.Properties.Resources.music2);  
            
            PlayerStandSmokeAnimation[0] = Miner.Properties.Resources.Smoke1;
            PlayerStandSmokeAnimation[1] = Miner.Properties.Resources.Smoke2;
            PlayerStandSmokeAnimation[2] = Miner.Properties.Resources.Smoke3;
            PlayerStandSmokeAnimation[3] = Miner.Properties.Resources.Smoke4;
            PlayerStandSmokeAnimation[4] = Miner.Properties.Resources.Smoke5;
            PlayerStandSmokeAnimation[5] = Miner.Properties.Resources.Smoke6;

            AnimationStar[0] = Miner.Properties.Resources.Star1;
            AnimationStar[1] = Miner.Properties.Resources.Star2;
            AnimationStar[2] = Miner.Properties.Resources.Star3;
            AnimationStar[3] = Miner.Properties.Resources.Star4;
            AnimationStar[4] = Miner.Properties.Resources.Star5;
            AnimationStar[5] = Miner.Properties.Resources.Star6;
            if (Style3DClick)
            {
                Earth = Miner.Properties.Resources.Earth3D;
                Gold = Miner.Properties.Resources.Gold3D;
                TimeLock = Miner.Properties.Resources.TimeLock3D;
                Sprint = Miner.Properties.Resources.Sprint3D;
                Music = Miner.Properties.Resources.Music3D;
                Mine = Miner.Properties.Resources.Mine3D;
                Obsidian = Miner.Properties.Resources.Obsidian3D;
                TrueSight = Miner.Properties.Resources.TrueSight3D;
            }
            TheGold = 0;            
            string[] SettingString = Miner.Properties.Resources.Settings.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < SettingString.Length; i++)
            {
                string[] splitString = SettingString[i].Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                SettingString[i] = splitString[1];
            }
            ManyGeneration = Convert.ToInt32(SettingString[0]);
            RuneGeneration = Convert.ToInt32(SettingString[1]);
            PlayerTime = 0;
            BrushTextScreen = new SolidBrush(Color.White);
            FontTextScreen = new Font(SettingString[2], Convert.ToInt32(SettingString[3]));            
            BlockSize = (Height / Convert.ToInt32(SettingString[5]));
            TimeRune = Convert.ToInt32(SettingString[6]);
            PlayerSpeed = (BlockSize*0.05)*Convert.ToDouble(SettingString[4]);
            TimeSmoke = Convert.ToInt32(SettingString[7]);
            timeJump = Convert.ToInt32(SettingString[8]);
            PlayerSpeedRune = BlockSize * Convert.ToDouble(SettingString[9]);
            MineGeneration = Convert.ToInt32(multiplication*Convert.ToDouble(SettingString[10]));
            block = new GameBlock[BlockHeight,BlockWidth];
           
            for (int i = 0; i < BlockHeight; i++)
            {
                for (int j = 0; j < BlockWidth; j++)
                {
                    if (i <= HeightBlockSky)
                    {
                        if (i != HeightBlockSky)
                        {
                            block[i, j].impact = false;
                            block[i, j].type = TypeBlock.Sky;
                            block[i, j].path = new GraphicsPath();
                        }
                        else
                        {
                            block[i, j].impact = false;
                            block[i, j].type = TypeBlock.Grass;
                            block[i, j].path = new GraphicsPath();
                        }
                    }
                    else
                    {                       
                        block[i, j].impact = true;
                        if (RandomSpawn.Next(1001) > MineGeneration)
                        {
                            block[i, j].type = TypeBlock.Earth;
                        }
                        else
                        {
                            block[i, j].type = TypeBlock.Mine;
                        }
                        block[i, j].path = new GraphicsPath();
                        if (PlayerName == "test" && i == HeightBlockSky + 1)
                        {
                            block[i, j].type = TypeBlock.Sprint;
                        }
                    }
                    if (i == 0 || i == BlockHeight - 1 || j == 0 || j == BlockWidth - 1)
                    {
                        block[i, j].impact = true;                        
                    }
                    block[i, j].points = new Point[4];
                }
            }
            for (int i = HeightBlockSky+6; i < BlockHeight-2; i+=5)
            {
                for (int j = 0; j < ManyGeneration;j++)
                {
                    int SaveRandom = RandomSpawn.Next(3, BlockWidth - 3);
                    block[i, BlockWidth - SaveRandom].type = TypeBlock.Gold;
                    block[i, BlockWidth - SaveRandom - 1].type = TypeBlock.Gold;
                    block[i, BlockWidth - SaveRandom + 1].type = TypeBlock.Gold;
                    block[i, BlockWidth - SaveRandom + 2].type = TypeBlock.Gold;
                    block[i + RandomSpawn.Next(-1, 2), BlockWidth - SaveRandom].type = TypeBlock.Gold;
                    block[i + RandomSpawn.Next(-1, 2), BlockWidth - SaveRandom - 1].type = TypeBlock.Gold;
                    block[i + RandomSpawn.Next(-1, 2), BlockWidth - SaveRandom + 1].type = TypeBlock.Gold;
                    block[i + RandomSpawn.Next(-1, 2), BlockWidth - SaveRandom + 2].type = TypeBlock.Gold;
                }
            }            
            for (int i = HeightBlockSky+3; i < BlockHeight; i++)
            {
                for (int j = 0; j < BlockWidth; j++)
                {
                    if (i != 0 || i != BlockHeight - 1 || j != 0 || j != BlockWidth - 1)
                    {
                        if (RandomSpawn.Next(1001) < RuneGeneration)
                        {
                            int saveRandom = RandomSpawn.Next(30);
                            if (saveRandom >= 10)
                            {

                                block[i, j].type = TypeBlock.Music;
                            }
                            else
                            {
                                if (saveRandom >= 5)
                                {
                                    block[i, j].type = TypeBlock.TrueSight;
                                }
                                else
                                {
                                    if (saveRandom >= 3)
                                    {
                                        block[i, j].type = TypeBlock.TimeLock;
                                    }
                                    else
                                    {
                                        block[i, j].type = TypeBlock.Sprint;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < BlockHeight; i++)
            {
                block[i, 0].type = TypeBlock.Obsidian;
                block[i, BlockWidth-1].type = TypeBlock.Obsidian;
            }
            for (int i = 0; i < BlockWidth; i++)
            {
                block[0, i].type = TypeBlock.Obsidian;
                block[BlockHeight - 1, i].type = TypeBlock.Obsidian;
            }
            for (int i = 0; i < BlockHeight; i++)
            {
                for (int j = 0; j < BlockWidth; j++)
                {
                    if (block[i, j].type == TypeBlock.Gold)
                    {
                        TheGold++;
                    }
                }
            }
            pointsLeftImpact = new Point[4];
            pointsRightImpact = new Point[4];
            pointsUpImpact = new Point[2];
            pointsDownImpact = new Point[2];
           
            pointsUpImpact[0] = new Point(Width / 2 - (int)(BlockSize / 3), Height / 2 - (int)(BlockSize * 1.4));
            pointsUpImpact[1] = new Point(Width / 2 + (int)(BlockSize / 3), Height / 2 - (int)(BlockSize * 1.4));

            pointsDownImpact[0] = new Point(Width / 2 - (int)(BlockSize / 3), Height / 2 + (int)(BlockSize * 1.4));
            pointsDownImpact[1] = new Point(Width / 2 + (int)(BlockSize / 3), Height / 2 + (int)(BlockSize * 1.4));

            pointsLeftImpact[0] = new Point(Width / 2 - (int)(BlockSize / 2), Height / 2 - (int)(BlockSize * 1.25));
            pointsLeftImpact[1] = new Point(Width / 2 - (int)(BlockSize / 2), Height / 2 - (int)(BlockSize * 0.5));
            pointsLeftImpact[2] = new Point(Width / 2 - (int)(BlockSize / 2), Height / 2 + (int)(BlockSize * 0.5));
            pointsLeftImpact[3] = new Point(Width / 2 - (int)(BlockSize / 2), Height / 2 + (int)(BlockSize * 1.25));

            pointsRightImpact[0] = new Point(Width / 2 + (int)(BlockSize / 2), Height / 2 - (int)(BlockSize * 1.25));
            pointsRightImpact[1] = new Point(Width / 2 + (int)(BlockSize / 2), Height / 2 - (int)(BlockSize * 0.5));
            pointsRightImpact[2] = new Point(Width / 2 + (int)(BlockSize / 2), Height / 2 + (int)(BlockSize * 0.5));
            pointsRightImpact[3] = new Point(Width / 2 + (int)(BlockSize / 2), Height / 2 + (int)(BlockSize * 1.25));

            RangePick.Reset();
            RangePick.StartFigure();
            RangePick.AddRectangle(new Rectangle(Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4)));
            RangePick.CloseFigure();
            GameTime.Interval = 10;
            GameTime.Start();
            
        }


       

        private void GameTime_Tick(object sender, EventArgs e)
        {
            if (jump == 0)
            {               
                GoDown = true;
                GoUp = false;
            }
            for (int i = 0; i < BlockHeight; i++)
            {
                for (int j = 0; j < BlockWidth; j++)
                {
                    for (int n = 0; n < 4; n++)
                    {
                        if (block[i, j].points[n].Y > pointsUpImpact[0].Y - (int)(BlockSize) && block[i, j].points[n].Y < pointsUpImpact[0].Y + (int)(BlockSize))
                        {
                            if (block[i, j].points[n].X > pointsUpImpact[0].X - (int)(BlockSize) && block[i, j].points[n].X < pointsUpImpact[0].X + (int)(BlockSize))
                            {                           
                                if (block[i, j].impact && (block[i, j].path.IsVisible(pointsUpImpact[0])))
                                {
                                    GoUp = false;
                                }
                            }
                            if (block[i, j].points[n].X > pointsUpImpact[1].X - (int)(BlockSize) && block[i, j].points[n].X < pointsUpImpact[1].X + (int)(BlockSize))
                            {
                                if (block[i, j].impact && (block[i, j].path.IsVisible(pointsUpImpact[1])))
                                {
                                    GoUp = false;
                                }
                            }
                            
                        }
                        if (block[i, j].points[n].Y > pointsDownImpact[0].Y - (int)(BlockSize) && block[i, j].points[n].Y < pointsDownImpact[0].Y + (int)(BlockSize))
                        {
                            if (block[i, j].points[n].X > pointsDownImpact[0].X - (int)(BlockSize) && block[i, j].points[n].X < pointsDownImpact[0].X + (int)(BlockSize))
                            {
                                if (block[i, j].impact && (block[i, j].path.IsVisible(pointsDownImpact[0])))
                                {
                                    GoDown = false;
                                }
                            }
                            if (block[i, j].points[n].X > pointsDownImpact[1].X - (int)(BlockSize) && block[i, j].points[n].X < pointsDownImpact[1].X + (int)(BlockSize))
                            {
                                if (block[i, j].impact && (block[i, j].path.IsVisible(pointsDownImpact[1])))
                                {
                                    GoDown = false;
                                }
                            }

                        }
                        if (block[i, j].points[n].X > pointsLeftImpact[0].X - (int)(BlockSize) && block[i, j].points[n].X < pointsLeftImpact[0].X + (int)(BlockSize))
                        {
                            for (int m = 0; m < 4; m++)
                            {
                                if (block[i, j].points[n].Y > pointsLeftImpact[m].Y - (int)(BlockSize) && block[i, j].points[n].Y < pointsLeftImpact[m].Y + (int)(BlockSize))
                                {
                                    if (block[i, j].impact && (block[i, j].path.IsVisible(pointsLeftImpact[m])))
                                    {
                                        GoLeft = false;
                                    }
                                }
                            }
                        }
                        if (block[i, j].points[n].X > pointsRightImpact[0].X - (int)(BlockSize) && block[i, j].points[n].X < pointsRightImpact[0].X + (int)(BlockSize))
                        {
                            for (int m = 0; m < 4; m++)
                            {
                                if (block[i, j].points[n].Y > pointsRightImpact[m].Y - (int)(BlockSize) && block[i, j].points[n].Y < pointsRightImpact[m].Y + (int)(BlockSize))
                                {
                                    if (block[i, j].impact && (block[i, j].path.IsVisible(pointsRightImpact[m])))
                                    {
                                        GoRight = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (jump > 0)
            {
                if (GoUp)
                {
                    jump--;
                }
                else
                {
                    jump = 0;
                }
            }
            if (GoUp == true)
            {
                Block_Y+=(int)(PlayerSpeed);
            }
            if (GoLeft == true)
            {
                Block_X += (int)(PlayerSpeed);
            }
            if (GoRight == true)
            {
                Block_X -= (int)(PlayerSpeed);
            }
            if (GoDown == true)
            {
                Block_Y -= (int)(PlayerSpeed);
            }
            AnimationRun--;
            if (SprintTime > 0)
            {
                if (SprintTime == 1)
                {
                    PlayerSpeed -= PlayerSpeedRune;
                    timeJump /= 2;
                }
                SprintTime--;                
            }
            if (TimeTrueSight > 0)
            {
                TimeTrueSight--;
            }
            if (TimeLockTime > 0)
            {
                TimeLockTime--;
            }
            else
            {
                PlayerTime++;
            }
            if (AnimationRun < 0)
            {
                AnimationRun = 5;
            }
            if (AnimationPickClick > -1)
            {
                AnimationPickClick--;
            }            
            SmokeStand++;
            Invalidate();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < BlockHeight; i++)
            {
                for (int j = 0; j < BlockWidth; j++)
                {
                    block[i, j].path.Reset();
                    block[i, j].path.StartFigure();
                    block[i, j].path.AddRectangle(new Rectangle(Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize)));
                    block[i, j].path.CloseFigure();
                    block[i, j].points[0] = new Point(Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize));
                    block[i, j].points[1] = new Point(Block_X + (int)(j * BlockSize) + (int)(BlockSize), Block_Y + (int)(i * BlockSize));
                    block[i, j].points[2] = new Point(Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize) + (int)(BlockSize));
                    block[i, j].points[3] = new Point(Block_X + (int)(j * BlockSize) + (int)(BlockSize), Block_Y + (int)(i * BlockSize) + (int)(BlockSize));
                    if (i != 0 && i != BlockHeight - 1 && j != 0 && j != BlockWidth - 1 && block[i + 1, j].impact && block[i - 1, j].impact && block[i, j+1].impact && block[i, j-1].impact && TimeTrueSight == 0)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.Black), Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize)); // вот это специально чтоб разность в скорости игры небыло
                    }
                    else
                    switch(block[i, j].type)
                    {
                        case TypeBlock.Earth:
                            {
                                e.Graphics.DrawImage(Earth, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.EarthBackground:
                            {
                                e.Graphics.DrawImage(EarthBackground, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.Gold:
                            {
                                e.Graphics.DrawImage(Gold, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                e.Graphics.DrawImage(AnimationStar[AnimationRun], Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.Grass:
                            {
                                e.Graphics.DrawImage(Grass, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.Sprint:
                            {
                                e.Graphics.DrawImage(Sprint, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                e.Graphics.DrawImage(AnimationStar[AnimationRun], Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.Sky:
                            {
                                e.Graphics.DrawImage(Sky, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.TimeLock:
                            {
                                e.Graphics.DrawImage(TimeLock, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                e.Graphics.DrawImage(AnimationStar[AnimationRun], Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.Music:
                            {
                                e.Graphics.DrawImage(Music, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                e.Graphics.DrawImage(AnimationStar[AnimationRun], Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.Mine:
                            {
                                e.Graphics.DrawImage(Mine, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.Obsidian:
                            {
                                e.Graphics.DrawImage(Obsidian, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                        case TypeBlock.TrueSight:
                            {
                                e.Graphics.DrawImage(TrueSight, Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                e.Graphics.DrawImage(AnimationStar[AnimationRun], Block_X + (int)(j * BlockSize), Block_Y + (int)(i * BlockSize), (int)(BlockSize), (int)(BlockSize));
                                break;
                            }
                    }    
                }
            }
            if (AnimationPickClick > 0)
            {
                Point MidPoint = new Point(Width / 2, Height / 2);
                double y = MidPoint.Y - AnimationPick.Y;
                double x = MidPoint.X - AnimationPick.X;
                double Tan = Math.Abs(y) / Math.Abs(x);
                if (Tan <= Math.Tan(Math.PI / 6))
                {
                    if (x >= 0)
                    {
                        e.Graphics.DrawImage(ClickLeft, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                    }
                    else
                    {
                        e.Graphics.DrawImage(ClickRight, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                    }
                }
                else
                {
                    if (Tan >= Math.Tan(Math.PI / 3))
                    {
                        if (y >= 0)
                        {
                            e.Graphics.DrawImage(ClickUp, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                        }
                        else
                        {
                            e.Graphics.DrawImage(ClickDown, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                        }
                    }
                    else
                    {
                        if (y >= 0)
                        {
                            if (x >= 0)
                            {
                                e.Graphics.DrawImage(ClickUpLeft, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                            }
                            else
                            {
                                e.Graphics.DrawImage(ClickUpRight, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                            }
                        }
                        else
                        {
                            if (x >= 0)
                            {
                                e.Graphics.DrawImage(ClickDownLeft, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                            }
                            else
                            {
                                e.Graphics.DrawImage(ClickDownRight, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                            }
                        }
                    }
                }
            }
            else
            {
                if ((!GoLeft && !GoRight) || (GoLeft && GoRight))
                {
                    if (jump != 0)
                    {
                        e.Graphics.DrawImage(PlayerStandJump, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                    }
                    else
                    {
                        if (SmokeStand > TimeSmoke)
                        {
                            e.Graphics.DrawImage(PlayerStandSmokeAnimation[AnimationRun], Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                        }
                        else
                        {
                            e.Graphics.DrawImage(PlayerStand, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                        }
                    }
                }
                if (!GoLeft && GoRight)
                {
                    if (jump != 0)
                    {
                        e.Graphics.DrawImage(PlayerRightJump, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                    }
                    else
                    {
                        if (AnimationRun == 6 || AnimationRun == 5)
                        {
                            e.Graphics.DrawImage(PlayerRight2, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                        }
                        else
                        {
                            if (AnimationRun == 4 || AnimationRun == 3)
                            {
                                e.Graphics.DrawImage(PlayerRight1, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                            }
                            else
                            {
                                e.Graphics.DrawImage(PlayerRight, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                            }
                        }
                    }
                }
                if (GoLeft && !GoRight)
                {
                    if (jump != 0)
                    {
                        e.Graphics.DrawImage(PlayerLeftJump, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                    }
                    else
                    {
                        if (AnimationRun == 6 || AnimationRun == 5)
                        {
                            e.Graphics.DrawImage(PlayerLeft2, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                        }
                        else
                        {
                            if (AnimationRun == 4 || AnimationRun == 3)
                            {
                                e.Graphics.DrawImage(PlayerLeft1, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                            }
                            else
                            {
                                e.Graphics.DrawImage(PlayerLeft, Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                            }
                        }
                    }
                }
            }
            if (RangePickVisible)
            {
                e.Graphics.DrawEllipse(new Pen(Color.Red, 2), Width / 2 - (int)(BlockSize * 2), Height / 2 - (int)(BlockSize * 2), (int)(BlockSize * 4), (int)(BlockSize * 4));
                e.Graphics.DrawPolygon(new Pen(Color.Red, 4), pointsLeftImpact);
                e.Graphics.DrawPolygon(new Pen(Color.Red, 4), pointsDownImpact);
                e.Graphics.DrawPolygon(new Pen(Color.Red, 4), pointsRightImpact);
                e.Graphics.DrawPolygon(new Pen(Color.Red, 4), pointsUpImpact);
                e.Graphics.FillEllipse(new SolidBrush(Color.Blue), pointsUpImpact[0].X, pointsUpImpact[0].Y, (int)(BlockSize / 10), (int)(BlockSize / 10));
                e.Graphics.FillEllipse(new SolidBrush(Color.Blue), pointsUpImpact[1].X, pointsUpImpact[1].Y, (int)(BlockSize / 10), (int)(BlockSize / 10));
                e.Graphics.FillEllipse(new SolidBrush(Color.Blue), pointsDownImpact[0].X, pointsDownImpact[0].Y, (int)(BlockSize / 10), (int)(BlockSize / 10));
                e.Graphics.FillEllipse(new SolidBrush(Color.Blue), pointsDownImpact[1].X, pointsDownImpact[1].Y, (int)(BlockSize / 10), (int)(BlockSize / 10));
                for (int I = 0; I < 4; I++)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Blue), pointsLeftImpact[I].X, pointsLeftImpact[I].Y, (int)(BlockSize / 10), (int)(BlockSize / 10));
                    e.Graphics.FillEllipse(new SolidBrush(Color.Blue), pointsRightImpact[I].X, pointsRightImpact[I].Y, (int)(BlockSize / 10), (int)(BlockSize / 10));
                }
            }
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray),0,0,Width / 5,Height / 6);
            e.Graphics.DrawRectangle(new Pen(Color.Black,3), 0, 0, Width / 5, Height / 6);
            int OutTimeSec = PlayerTime/50;
            int OutTimeMin = OutTimeSec/60;
            int OutTimeHour = OutTimeMin/60;
            OutTimeSec = OutTimeSec % 60;
            OutTimeMin = OutTimeMin % 60;
            OutTimeString = OutTimeHour+":";
            if(OutTimeMin>9)
            {
                OutTimeString += OutTimeMin;
            }
            else
            {
                OutTimeString += "0"+OutTimeMin;
            }
            OutTimeString += ":";
            if (OutTimeSec > 9)
            {
                OutTimeString += OutTimeSec;
            }
            else
            {
                OutTimeString += "0" + OutTimeSec;
            }
            string OutSprint = SprintTime > 0 ? "on" : "off";
            string OutTime = TimeLockTime > 0 ? "on" : "off";
            e.Graphics.DrawString("Time: "+OutTimeString, FontTextScreen, BrushTextScreen, Width/100, Height/50);
            e.Graphics.DrawString("The gold : " + TheGold.ToString(), FontTextScreen, BrushTextScreen, Width / 100, Height / 20);
            e.Graphics.DrawString(OutBlock, FontTextScreen, BrushTextScreen, Width / 100, Height / 11);
            e.Graphics.DrawString("TimeLock " + OutTime, FontTextScreen, BrushTextScreen, Width / 100, Height / 8);
            if (TheGold == 0)
            {
                e.Graphics.DrawString("You Win", new Font("Courier New", Height / 4), BrushTextScreen, Width / 100, Height / 8);
                GameTime.Stop();
            }
            if (Die)
            {
                e.Graphics.DrawImage(Miner.Properties.Resources.Boom, Width / 2 - (int)(BlockSize * 3), Height / 2 - (int)(BlockSize * 3), (int)(BlockSize * 6), (int)(BlockSize * 6));
                GameTime.Stop();
            }
        }

        private void GameScreen_MouseClick(object sender, MouseEventArgs e)
        {
            if (!Die)
            {
                SmokeStand = 0;
                OutBlock = "Block ";
                AnimationPick = new Point(e.X, e.Y);
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (RangePick.IsVisible(AnimationPick))
                    {
                        AnimationPickClick = 3;
                        for (int i = 0; i < BlockHeight; i++)
                        {
                            for (int j = 0; j < BlockWidth; j++)
                            {
                                if (block[i, j].path.IsVisible(AnimationPick))
                                {
                                    OutBlock += i + " " + j;
                                    if (block[i, j].type == TypeBlock.Mine)
                                    {
                                        if (TimeLockTime > 0)
                                        {
                                            PlayerTime *= 2;
                                        }
                                        else
                                        {
                                            Die = true;                                            
                                        }
                                    }
                                    if (block[i, j].type == TypeBlock.Gold)
                                    {
                                        TheGold--;
                                    }
                                    if (block[i, j].type == TypeBlock.TrueSight)
                                    {
                                        TimeTrueSight = TimeRune;
                                    }
                                    if (block[i, j].type == TypeBlock.Music)
                                    {
                                        music();
                                    }
                                    if (block[i, j].type == TypeBlock.Sprint)
                                    {
                                        if (SprintTime == 0)
                                        {
                                            PlayerSpeed += PlayerSpeedRune;
                                            timeJump *= 2;
                                        }
                                        SprintTime = TimeRune;
                                    }
                                    if (block[i, j].type == TypeBlock.TimeLock)
                                    {
                                        TimeLockTime = TimeRune;
                                    }
                                    if (i == 0 || j == 0 || i == BlockHeight - 1 || j == BlockWidth - 1)
                                    {
                                        block[i, j].impact = true;
                                    }
                                    else
                                    {
                                        block[i, j].impact = false;
                                    }
                                    if (i <= HeightBlockSky)
                                    {
                                        if (block[i, j].type != TypeBlock.Obsidian)
                                        {
                                            if (i == HeightBlockSky)
                                            {
                                                block[i, j].type = TypeBlock.Grass;
                                            }
                                            else
                                            {
                                                block[i, j].type = TypeBlock.Sky;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (block[i, j].type != TypeBlock.Obsidian)
                                        {
                                            block[i, j].type = TypeBlock.EarthBackground;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (RangePick.IsVisible(AnimationPick))
                    {
                        for (int i = 0; i < BlockHeight; i++)
                        {
                            for (int j = 0; j < BlockWidth; j++)
                            {
                                if (block[i, j].path.IsVisible(AnimationPick) && !block[i, j].impact)
                                {
                                    block[i, j].impact = true;
                                    block[i, j].type = TypeBlock.Earth;
                                    if (PlayerName == "test")
                                    {
                                        Random rnd = new Random();
                                        int Kek = rnd.Next(3);
                                        if (Kek == 2)
                                             block[i, j].type = TypeBlock.TrueSight;
                                        if(Kek == 1)
                                             block[i, j].type = TypeBlock.Music;
                                        if (Kek == 0)
                                            block[i, j].type = TypeBlock.Sprint;
                                    }
                                }
                                if (block[i, j].path.IsVisible(AnimationPick) && block[i, j].type == TypeBlock.Mine)
                                {
                                    PlayerTime += 360;
                                    block[i, j].impact = true;
                                    block[i, j].type = TypeBlock.Earth;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Die)
            {
                SmokeStand = 0;
                if (e.KeyCode == Keys.W)
                {
                    if (jump == 0 && !GoDown && tumbler)
                    {
                        GoUp = true;
                        if (!GoDown)
                        {
                            jump = timeJump;
                        }
                        tumbler = false;
                    }
                }
                if (e.KeyCode == Keys.A)
                {
                    GoLeft = true;
                }
                if (e.KeyCode == Keys.D)
                {
                    GoRight = true;
                }
                if (e.KeyCode == Keys.S)
                {
                    GoDown = true;
                }
                if (e.KeyCode == Keys.V)
                {
                    RangePickVisible = !RangePickVisible;
                }
            }
        }

        bool tumbler = false;

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Die)
            {
                if (e.KeyCode == Keys.W)
                {
                    tumbler = true;
                    //jump = 0;
                    //GoUp = false;
                }
                if (e.KeyCode == Keys.A)
                {
                    GoLeft = false;
                }
                if (e.KeyCode == Keys.D)
                {
                    GoRight = false;
                }
                if (e.KeyCode == Keys.S)
                {
                    GoDown = false;
                }
            }
        }

        string OutBlock;

        private void GameScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            music1.Stop();
            music2.Stop();
        }
       
    }
}
