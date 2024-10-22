using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace konecdubnaseblizi
{
    public partial class background : Form
    {
        bool goleft, goright, jump, gameover;
        int JumpSpeed;
        int force;
        int score = 0;
        int playerspeed = 7;
        int enemyonespeed = 2;
        int enemytwospeed = 2;
        bool levelCompleted = false;

        private Button restartButton;
        private PictureBox character; 
        private bool isFacingRight = true;

        public background()
        {
            InitializeComponent();
            InitializeUI();
            character = player; 
        }

        private void InitializeUI()
        {
            restartButton = new Button();
            restartButton.Name = "restartButton";
            restartButton.Text = "Restart";
            restartButton.Visible = false;
            restartButton.Click += restartButton_Click;

            restartButton.Location = new Point((this.ClientSize.Width - restartButton.Width) / 2, (this.ClientSize.Height - restartButton.Height) / 2);
            restartButton.Size = new Size(100, 50);
            restartButton.BackColor = Color.Gray;

            this.Controls.Add(restartButton);
        }

        private void MainGameTimeEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Počet bitches: " + score;

            character.Top += JumpSpeed;

            if (goleft == true)
            {
                MoveCharacter(-playerspeed); 
            }
            if (goright == true)
            {
                MoveCharacter(playerspeed);
            }
            if (jump == true && force < 0)
            {
                jump = false;
            }

            if (jump == true)
            {
                JumpSpeed = -8;
                force -= 1;
            }
            else
            {
                JumpSpeed = 10;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "platform")
                    {
                        if (character.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8;
                            character.Top = x.Top - character.Height;
                        }
                        x.BringToFront();
                    }

                    if ((string)x.Tag == "bitch")
                    {
                        if (character.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score++;
                            Globals.Score = score;
                        }
                    }

                

                    if ((string)x.Tag == "enemyone" || (string)x.Tag == "enemytwo")
                    {
                        if (character.Bounds.IntersectsWith(x.Bounds))
                        {
                            timer.Stop();
                            gameover = true;
                            txtScore.Text = "Počet bitches: " + score + "\n" +"To nebyla žena :(";
                            restartButton.Enabled = true;
                            restartButton.Visible = true;
                        }
                    }

                    if (character.Top + character.Height > this.ClientSize.Height + 50)
                    {
                        timer.Stop();
                        gameover = true;
                        txtScore.Text = "Počet bitches: " + score + "\n" + "spadl jsi...";
                        restartButton.Enabled = true;
                        restartButton.Visible = true;
                    }

                    if (character.Bounds.IntersectsWith(door.Bounds) && !levelCompleted)
                    {
                        levelCompleted = true;
                        konec newlevel = new konec();
                        newlevel.FormClosed += (s, args) =>
                        {
                            this.Show();
                            timer.Start();
                            levelCompleted = false;
                        };

                        timer.Stop();
                        gameover = true;
                        newlevel.Show();
                        this.Hide();
                      
                    }
                }

                if ((string)x.Tag == "platform2")
                {
                    if (character.Bounds.IntersectsWith(x.Bounds))
                    {
                        force = 8;
                        character.Top = x.Top - character.Height;
                    }
                }
            }

            enemyone.Left += enemyonespeed;
            if (enemyone.Left + enemyone.Width >= platform2.Left + platform2.Width)
            {
                enemyonespeed = -Math.Abs(enemyonespeed);
            }
            else if (enemyone.Left <= platform2.Left)
            {
                enemyonespeed = Math.Abs(enemyonespeed);
            }

            enemytwo.Left += enemytwospeed;
            if (enemytwo.Left + enemytwo.Width >= platform5.Left + platform5.Width)
            {
                enemytwospeed = -Math.Abs(enemytwospeed);
            }
            else if (enemytwo.Left <= platform5.Left)
            {
                enemytwospeed = Math.Abs(enemytwospeed);
            }

            enemyone.BringToFront();
            enemytwo.BringToFront();
        }

        private void MoveCharacter(int distance)
        {
            
            character.Left += distance;

            
            if ((distance < 0 && isFacingRight) || (distance > 0 && !isFacingRight))
            {
                character.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                isFacingRight = !isFacingRight;
            }
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }
            if (e.KeyCode == Keys.Space && jump == false)
            {
                jump = true;
            }
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if (jump == true)
            {
                jump = false;
            }
            if (e.KeyCode == Keys.Enter && gameover == true)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            jump = false;
            goleft = false;
            goright = false;
            gameover = false;
            score = 0;
            Globals.Score = 0;

            txtScore.Text = "Počet bitches: " + score;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }

            character.Left = 66;
            character.Top = 584;

            timer.Start();
            restartButton.Enabled = false;
            restartButton.Visible = false;
            this.Focus();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            RestartGame();
        }
    }
}
