using Starcatcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starcatcher
{
    public partial class Form1 : Form
    {

        int BallXVelocity;
        int BallYVelocity;
        int points;

        int DeathCounter = 0;

        int StarsUpgradeLevel;
        int RarityUpgradeLevel;
        int UniverseUnlocked;

        public List<int> DataToSend;

        Random rand = new Random();

        private Rectangle paddleBounds;
        private Rectangle ballBounds;

        private List<STARS> stars = new List<STARS>();
        RARE_STAR rare_star = new RARE_STAR();
        private List<EXPLOSION> explosions = new List<EXPLOSION>();
        private List<CLOUDS> clouds = new List<CLOUDS>();
        private List<UFOS> ufos = new List<UFOS>();
        private List<ALIENS> aliens = new List<ALIENS>();

        Timer Gravity = new Timer();

        PictureBox pause_bg;

        bool ball_recovery = false;

        bool pause = false;
        int death_timer = 0;

        bool spawn_rare_star = false;
        bool allow_ufo = true;
        bool allow_alien = true;

        int click_timer = 0;
        bool allow_click = false;


        int ufo_timer;
        int alien_timer;


        float ball_angle = 0f;
        float angular_velocity = 5f;

        int rare_star_chance = 1; // %

        int star_gif_timer = 120;
        int current_star_texture_index = 0;
        private List<Image> star_textures = new List<Image>() {
            Resources.STAR1, Resources.STAR2, Resources.STAR3, Resources.STAR4,
            Resources.STAR5, Resources.STAR6, Resources.STAR7, Resources.STAR8,
            Resources.STAR1
        };

        int explosion_gif_timer = 15;
        int alien_explosion_gif_timer = 15;
        int alien_gif_timer = 900;
        int alien_hit_gif_timer = 15;
        int ufo_hit_gif_timer = 15;
        private List<Image> explosion2 = new List<Image>()
        {
            Resources.exp1, Resources.exp2, Resources.exp3, Resources.exp4, Resources.exp5,
            Resources.exp6, Resources.exp7, Resources.exp8, Resources.exp9, Resources.exp10,
            Resources.exp11, Resources.exp12, Resources.exp13, Resources.exp14
        };

        private List<ALIEN_HIT_PARTICLES> alien_hit_particles = new List<ALIEN_HIT_PARTICLES>();
        private List<UFO_HIT_PARTICLES> ufo_hit_particles = new List<UFO_HIT_PARTICLES>();
        private List<ALIEN_EXPLOSION> alien_explosion_particles = new List<ALIEN_EXPLOSION>();

        Image Ball_Texture = Resources.BALL_MINIMIZED;

        // __INIT__
        public Form1(List<int> SentData = null)
        {

            if (SentData == null)
            {
                SentData = new List<int>() {0, 1, 1, 0, 0 };

                BallXVelocity = 14 + rand.Next(1, 4);
                BallYVelocity = -14 - rand.Next(1, 4);
            }

            paddleBounds = new Rectangle(this.Size.Width / 2 + 200, 550, Resources.PADDLE_MINIMIZED.Width / 2 + 30, Resources.PADDLE_MINIMIZED.Height / 2);
            ballBounds = new Rectangle(this.Size.Width / 2 + 200, this.Size.Height * 2 - 20, Resources.BALL_MINIMIZED.Width - 7, Resources.BALL_MINIMIZED.Height - 7);

            InitializeComponent();

            //ufo_timer = rand.Next(20000, 75010 - (StarsUpgradeLevel * RarityUpgradeLevel));
            //alien_timer = rand.Next(20000, 75010 - (StarsUpgradeLevel * RarityUpgradeLevel));

            this.KeyPreview = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            Gravity.Interval = 14;
            Gravity.Tick += GravityEvents;
            Gravity.Start();

            points = SentData[0];
            StarsUpgradeLevel = SentData[1];
            RarityUpgradeLevel = SentData[2];
            UniverseUnlocked = SentData[3];
            DeathCounter = SentData[4];

            for (int i = 0; i < StarsUpgradeLevel; i++)
            {
                Spawn_Star();
            }

            // Set Font To all Components
            PausedLabel.Font = CustomFont.GetFont(30, FontStyle.Regular);
            ContinueButton.Font = CustomFont.GetFont(20, FontStyle.Regular);
            ShopButton.Font = CustomFont.GetFont(20, FontStyle.Regular);
            MenuButton.Font = CustomFont.GetFont(20, FontStyle.Regular);
        }
        // Gravity
        private void GravityEvents(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Spawn_Cloud();


                if (aliens.Count > 0 && click_timer > 0)
                {
                    click_timer -= Gravity.Interval;
                }

                if (click_timer <= 0)
                {
                    allow_click = true;
                }

                // Star Texture Changing
                star_gif_timer -= Gravity.Interval;
                alien_gif_timer -= Gravity.Interval;

                // Ball Rotation

                if (!ball_recovery)
                {
                    ball_angle += angular_velocity;
                    if (ball_angle >= 360f)
                        ball_angle -= 360f;

                    if (ball_angle < 0f)
                        ball_angle += 360f;

                    angular_velocity *= 0.95f;
                }

                if (!allow_alien)
                {
                    alien_timer -= Gravity.Interval;
                }

                if (alien_timer <= 0)
                {
                    allow_alien = true;
                    Spawn_alien();
                }

                if (!allow_ufo)
                {
                    ufo_timer -= Gravity.Interval;
                }

                if (ufo_timer <= 0)
                {
                    allow_ufo = true;
                    Spawn_ufo();
                }

                if (!pause)
                {
                    if (ball_recovery)
                    {
                        death_timer -= Gravity.Interval;
                        //recover_display_label.Text = (death_timer / 1000).ToString();

                        if (death_timer >= 3000)
                        {
                            TimerBrush.Color = Color.Yellow;
                        }

                        if (death_timer < 3000 && death_timer >= 2000)
                        {
                            TimerBrush.Color = Color.Orange;
                        }
                        if (death_timer < 2000)
                        {
                            TimerBrush.Color = Color.Red;
                        }

                        if (death_timer <= 1000)
                        {
                            BallYVelocity = -14 - rand.Next(1, 4);
                            ball_recovery = false;
                        }
                    }
                    else
                    {
                        MoveBall();
                    }

                    Starfall();
                    MoveAlien();
                    MoveUfo();

                    //Explosion Texture Changing
                    explosion_gif_timer -= Gravity.Interval;

                    if (explosion_gif_timer <= 0)
                    {
                        for (int i = 0; i < explosions.Count; i++)
                        {
                            if (!explosions[i].end_of_animation)
                            {
                                if (explosions[i].current_explosion_texture_index >= explosion2.Count - 1)
                                {
                                    explosions[i].end_of_animation = true;
                                    explosions.RemoveAt(i);
                                }
                                else
                                {
                                    explosions[i].Texture = explosion2[explosions[i].current_explosion_texture_index];
                                    explosions[i].current_explosion_texture_index++;
                                }
                            }
                        }

                        explosion_gif_timer = 15;
                    }

                    //Alien Explosion Particle
                    alien_explosion_gif_timer -= Gravity.Interval;

                    if (alien_explosion_gif_timer <= 0)
                    {
                        for (int i = 0; i < alien_explosion_particles.Count; i++)
                        {
                            if (!alien_explosion_particles[i].end_of_animation)
                            {
                                if (alien_explosion_particles[i].current_particle_frame >= alien_explosion_particles[i].Textures.Count - 1)
                                {
                                    alien_explosion_particles[i].end_of_animation = true;
                                    alien_explosion_particles.RemoveAt(i);
                                }
                                else
                                {
                                    alien_explosion_particles[i].Texture = alien_explosion_particles[i].Textures[alien_explosion_particles[i].current_particle_frame];
                                    alien_explosion_particles[i].current_particle_frame++;
                                }
                            }
                        }

                        alien_explosion_gif_timer = 15;
                    }

                    // Alien Hit Particle
                    alien_hit_gif_timer -= Gravity.Interval;

                    if (alien_hit_gif_timer <= 0)
                    {
                        for (int i = 0; i < alien_hit_particles.Count; i++)
                        {
                            if (!alien_hit_particles[i].end_of_animation)
                            {
                                if (alien_hit_particles[i].current_particle_frame >= alien_hit_particles[i].Textures.Count - 1)
                                {
                                    alien_hit_particles[i].end_of_animation = true;
                                    alien_hit_particles.RemoveAt(i);
                                }
                                else
                                {
                                    alien_hit_particles[i].Texture = alien_hit_particles[i].Textures[alien_hit_particles[i].current_particle_frame];
                                    alien_hit_particles[i].current_particle_frame++;
                                }
                            }
                        }

                        alien_hit_gif_timer = 15;
                    }

                    // UFO hit particle
                    ufo_hit_gif_timer -= Gravity.Interval;

                    if (ufo_hit_gif_timer <= 0)
                    {
                        for (int i = 0; i < ufo_hit_particles.Count; i++)
                        {
                            if (!ufo_hit_particles[i].end_of_animation)
                            {
                                if (ufo_hit_particles[i].current_particle_frame >= ufo_hit_particles[i].Textures.Count - 1)
                                {
                                    ufo_hit_particles[i].end_of_animation = true;
                                    ufo_hit_particles.RemoveAt(i);
                                }
                                else
                                {
                                    ufo_hit_particles[i].Texture = ufo_hit_particles[i].Textures[ufo_hit_particles[i].current_particle_frame];
                                    ufo_hit_particles[i].current_particle_frame++;
                                }
                            }
                        }

                        ufo_hit_gif_timer = 15;
                    }
                }

                MoveClouds();
            }

            this.Invalidate();
        }


        // ---Fundamental Mechanics---

        private void RecoverBall()
        {
            allow_click = false;
            click_timer = 400;
            ballBounds.Location = new Point(350, this.Size.Height - 250);
            ball_angle = 270f;
            BallXVelocity = rand.Next(0, 21);
            BallYVelocity = -14 - rand.Next(1, 4);

            Ball_Texture = Resources.BALL_MINIMIZED;

            ball_recovery = true;
            death_timer = 4000;
        }

        private void MoveBall()
        {
            float linearSpeed = (float)Math.Sqrt(BallXVelocity * BallXVelocity + BallYVelocity * BallYVelocity);

            if (ballBounds.Y < 0)
            {
                ballBounds.Y += 10;
            }

            if (ballBounds.Right >= this.Size.Width)
            {
                BallXVelocity = -BallXVelocity;
                angular_velocity = (BallXVelocity / linearSpeed) * linearSpeed * 0.7f;
                ballBounds.Location = new Point(ballBounds.Location.X - ballBounds.Size.Width / 2, ballBounds.Location.Y);
            }

            if (ballBounds.Top <= 0)
            {
                BallYVelocity = -BallYVelocity;
                angular_velocity = (BallXVelocity / linearSpeed) * linearSpeed * 0.7f;
            }
            if (ballBounds.Bottom >= this.Size.Height)
            {
                // Game Over
                DeathCounter++;

                if (points > DeathCounter * 2)
                {
                    points -= DeathCounter * 2;
                }
                else
                {
                    points = 0;
                }

                ball_recovery = true;
                //
                RecoverBall();
            }

            if (ballBounds.Left <= 0)
            {
                BallXVelocity = Math.Abs(BallXVelocity);
                angular_velocity = (BallXVelocity / linearSpeed) * linearSpeed * 1.5f;
            }

            Rectangle PaddleTop = paddleBounds;
            PaddleTop.Y += 10;

            if (ballBounds.IntersectsWith(PaddleTop))
            {
                int HitPoint = (ballBounds.Location.X + ballBounds.Size.Width / 2) - (paddleBounds.Location.X + paddleBounds.Size.Width / 2);
                BallXVelocity = HitPoint / 4;
                BallYVelocity = -Math.Abs(BallYVelocity);
                angular_velocity = (BallXVelocity / linearSpeed) * linearSpeed * 0.7f;
            }


            ballBounds.X += BallXVelocity;
            ballBounds.Y += BallYVelocity;
        }

        private void Starfall()
        {
            if (spawn_rare_star)
            {
                rare_star.Bounds.Y += 20;
            }

            if (rare_star.Bounds.Y > this.ClientSize.Height)
            {
                spawn_rare_star = false;
            }

            if (spawn_rare_star && !ball_recovery && ballBounds.IntersectsWith(rare_star.Bounds))
            {
                points += rare_star.score;
                spawn_rare_star = false;
            }

            for (int i = stars.Count - 1; i >= 0; i--)
            {
                stars[i].Bounds.Y += stars[i].speeds[i];

                if (!ball_recovery && ballBounds.IntersectsWith(stars[i].Bounds))
                {
                    points += stars[i].scores[i];
                    //points_label.Text = points.ToString();

                    if (stars[i].temporary == false)
                    {
                        stars.RemoveAt(i);
                        Spawn_Star();
                    }
                    else
                    {
                        stars.RemoveAt(i);
                    }

                    continue;
                }

                if (stars[i].Bounds.Y > this.ClientSize.Height)
                {

                    if (stars[i].temporary == false)
                    {
                        stars.RemoveAt(i);
                        Spawn_Star();
                    }
                    else
                    {
                        stars.RemoveAt(i);
                    }
                }

                if (star_gif_timer <= 0)
                {
                    if (current_star_texture_index >= star_textures.Count - 1)
                    {
                        current_star_texture_index = 0;
                    }
                    current_star_texture_index++;

                    foreach (var star in stars)
                    {
                        star.Texture = star_textures[current_star_texture_index];
                    }

                    star_gif_timer = 120;
                }
            }

        }

        private void MoveClouds()
        {
            if (clouds.Count != 0)
            {
                for (int i = clouds.Count - 1; i >= 0; i--)
                {
                    clouds[i].Bounds.X -= clouds[i].speeds[i];

                    if (!ball_recovery && ballBounds.IntersectsWith(clouds[i].Bounds))
                    {
                        if (clouds[i].hold_star)
                        {
                            Spawn_Star(true, clouds[i].Bounds.X + rand.Next(-10, 11), clouds[i].Bounds.Y + 10);
                            clouds[i].hold_star = false;
                        }


                    }

                    if (clouds[i].Bounds.X + clouds[i].Texture.Width < -10)
                    {
                        clouds.RemoveAt(i);
                        Spawn_Cloud();
                    }
                }
            }
        }

        private void MoveUfo()
        {
            if (ufos.Count != 0)
            {
                for (int i = ufos.Count - 1; i >= 0; i--)
                {
                    // Movement
                    if (ufos[i].Bounds.Y < 10)
                    {
                        ufos[i].Bounds.Y += 1;
                    }
                    if (ufos[i].Bounds.Y + ufos[i].Texture.Height > this.ClientSize.Height)
                    {
                        ufos[i].Bounds.Y -= 2;
                    }

                    if (ufos[i].loc == 1)
                    {
                        ufos[i].Bounds.X -= ufos[i].speeds[i];
                        ufos[i].HealthLabelBounds.X -= ufos[i].speeds[i];

                        if (ufos[i].Bounds.X + ufos[i].Texture.Width * 2 < -50)
                        {
                            ufos[i].loc = 2;
                        }
                    }
                    else
                    {
                        ufos[i].Bounds.X += ufos[i].speeds[i];
                        ufos[i].HealthLabelBounds.X += ufos[i].speeds[i];

                        if (ufos[i].Bounds.X - ufos[i].Texture.Width > this.Width + 10)
                        {
                            ufos[i].loc = 1;
                        }
                    }

                    // Intersection
                    if (spawn_rare_star && ufos[i].Bounds.IntersectsWith(rare_star.Bounds))
                    {
                        spawn_rare_star = false;
                        if (ufos[i].Bounds.Height < 250)
                        {
                            ufos[i].Bounds.Width += 10;
                            ufos[i].Bounds.Height += 10;
                        }
                    }

                    for (int j = stars.Count - 1; j >= 0; j--)
                    {
                        if (ufos[i].Bounds.IntersectsWith(stars[j].Bounds))
                        {
                            if (ufos[i].Bounds.Height < 250 && StarsUpgradeLevel < 4)
                            {
                                ufos[i].Bounds.Width += 2;
                                ufos[i].Bounds.Height += 2;

                                if (ufos[i].loc == 1)
                                {
                                    ufos[i].HealthLabelBounds.X = ufos[i].Bounds.X + ufos[i].Bounds.Width / 2;
                                    ufos[i].HealthLabelBounds.Y = ufos[i].Bounds.Y - ufos[i].Texture.Height + 10;
                                }
                                else
                                {
                                    ufos[i].HealthLabelBounds.X = ufos[i].Bounds.X + ufos[i].Bounds.Width / 2 - 10;
                                    ufos[i].HealthLabelBounds.Y = ufos[i].Bounds.Y - ufos[i].Texture.Height + 10;
                                }
                            }

                            if (stars[j].temporary == false)
                            {
                                stars.RemoveAt(j);
                                Spawn_Star();
                            }
                            else
                            {
                                stars.RemoveAt(j);
                            }
                        }
                    }

                    if (ufos[i].health >= 0 && ufos[i].Bounds.IntersectsWith(ballBounds)) {

                        ufos[i].Bounds.X += Convert.ToInt32(BallXVelocity / 4);
                        ufos[i].Bounds.Y += Convert.ToInt32(BallYVelocity / 4);

                        if (ball_recovery) { 
                            if (ufos[i].loc == 1)
                            {
                                ufos[i].loc = 2;
                            }
                            else
                            {
                                ufos[i].loc = 1;
                            }
                        
                        }

                        if (Math.Abs(BallXVelocity) < 10)
                            ufos[i].health -= 1;

                        else if (Math.Abs(BallXVelocity) >= 10 && Math.Abs(BallXVelocity) < 20)
                            ufos[i].health -= 2;

                        else if (Math.Abs(BallXVelocity) >= 20)
                            ufos[i].health -= 3;

                        int HitPoint = (ballBounds.Location.X + ballBounds.Size.Width / 2) - (ufos[i].Bounds.X + ufos[i].Bounds.Width / 2);
                        BallXVelocity = HitPoint / 5;
                        BallYVelocity = -BallYVelocity;

                        if (ufos[i].health > 0)
                        {
                            Rectangle intersect = Rectangle.Intersect(ufos[i].Bounds, ballBounds);
                            SpawnUfoHitParticle(intersect.X + BallXVelocity + intersect.Width, intersect.Y - BallYVelocity);
                        }
                    }

                    // Behavior
                    if (ufos[i].health <= 0)
                    {
                        points += ufos[i].max_health;

                        int HitPoint = (ballBounds.Location.X + ballBounds.Size.Width / 2) - (ufos[i].Bounds.X + ufos[i].Bounds.Width / 2);
                        BallXVelocity = HitPoint / 5;
                        BallYVelocity = -BallYVelocity;

                        Spawn_Explosion(ufos[i].Bounds.X - ufos[i].Bounds.Width / 2, ufos[i].Bounds.Y - ufos[i].Bounds.Height / 2);

                        ufos.RemoveAt(i);


                        allow_ufo = false;
                        if (ufos.Count == 0)
                        {
                            ufo_timer = rand.Next(20000, 120000);
                        }
                        if (ufos.Count == 1)
                        {
                            ufo_timer = rand.Next(20000 / 2, 120000 / 2);
                        }
                    }

                }
            }
        }

        private void MoveAlien()
        {
            if (aliens.Count != 0)
            {
                for (int i = aliens.Count - 1; i >= 0; i--)
                {
                    // Movement

                    if (aliens[i].Bounds.Y < 10)
                    {
                        aliens[i].Bounds.Y += 1;
                    }
                    if (aliens[i].Bounds.Y + aliens[i].Texture.Height > this.ClientSize.Height)
                    {
                        aliens[i].Bounds.Y -= 2;
                    }

                    if (aliens[i].loc == 1)
                    {
                        aliens[i].Bounds.X -= aliens[i].speeds[i];
                        aliens[i].HealthLabelBounds.X -= aliens[i].speeds[i];

                        if (aliens[i].Bounds.X + aliens[i].Texture.Width * 2 < -50)
                        {
                            aliens[i].loc = 2;
                        }
                    }
                    else
                    {
                        aliens[i].Bounds.X += aliens[i].speeds[i];
                        aliens[i].HealthLabelBounds.X += aliens[i].speeds[i];

                        if (aliens[i].Bounds.X - aliens[i].Texture.Width > this.Width + 10)
                        {
                            aliens[i].loc = 1;
                        }
                    }

                    // Intersection
                    if (spawn_rare_star && aliens[i].Bounds.IntersectsWith(rare_star.Bounds))
                    {
                        spawn_rare_star = false;
                        if (aliens[i].Bounds.Height < 250)
                        {
                            aliens[i].Bounds.Width += 10;
                            aliens[i].Bounds.Height += 10;
                        }
                    }

                    for (int j = stars.Count - 1; j >= 0; j--)
                    {
                        if (aliens[i].Bounds.IntersectsWith(stars[j].Bounds))
                        {
                            if (aliens[i].Bounds.Height < 250 && StarsUpgradeLevel < 4)
                            {
                                aliens[i].Bounds.Width += 2;
                                aliens[i].Bounds.Height += 2;
                                aliens[i].health++;

                                if (aliens[i].loc == 1)
                                {
                                    aliens[i].HealthLabelBounds.X = aliens[i].Bounds.X + aliens[i].Bounds.Width / 2 - 10;
                                    aliens[i].HealthLabelBounds.Y = aliens[i].Bounds.Y - aliens[i].Texture.Height + 10;
                                }
                                else
                                {
                                    aliens[i].HealthLabelBounds.X = aliens[i].Bounds.X + aliens[i].Bounds.Width / 2 - 10;
                                    aliens[i].HealthLabelBounds.Y = aliens[i].Bounds.Y - aliens[i].Texture.Height + 10;
                                }
                            }


                            if (stars[j].temporary == false)
                            {
                                stars.RemoveAt(j);
                                Spawn_Star();
                            }
                            else
                            {
                                stars.RemoveAt(j);
                            }

                            if (StarsUpgradeLevel < 4)
                            {
                                if (aliens[i].loc == 1)
                                {
                                    aliens[i].loc = 2;
                                }
                                else
                                {
                                    aliens[i].loc = 1;
                                }
                            }
                        }
                    }

                    for (int j = ufos.Count - 1; j >= 0; j--)
                    {
                        if (aliens[i].Bounds.IntersectsWith(ufos[j].Bounds))
                        {
                            if (aliens[i].loc == 1)
                            {
                                aliens[i].loc = 2;
                            }
                            else
                            {
                                aliens[i].loc = 1;
                            }

                            if (ufos[j].loc == 1)
                            {
                                ufos[j].loc = 2;
                            }
                            else
                            {
                                ufos[j].loc = 1;
                            }
                            if (aliens[i].Bounds.Y > 205)
                            {
                                aliens[i].Bounds.Y += 1;
                            }
                            else
                            {
                                aliens[i].Bounds.Y -= 1;
                            }
                        }
                    }

                    if (aliens[i].health >= 0 && aliens[i].Bounds.IntersectsWith(ballBounds))
                    {

                        aliens[i].Bounds.X += Convert.ToInt32(BallXVelocity / 4);
                        aliens[i].Bounds.Y += Convert.ToInt32(BallYVelocity / 4);

                        int HitPoint = (ballBounds.Location.X + ballBounds.Size.Width / 2) - (aliens[i].Bounds.X + aliens[i].Bounds.Width / 2);
                        BallXVelocity = HitPoint / 5;
                        BallYVelocity = -BallYVelocity;
                    }

                    // Behavior

                    if (aliens[i].health <= 0)
                    {
                        points += aliens[i].max_health;

                        SpawnAlienExplosion(aliens[i].Bounds.X - aliens[i].Texture.Width / 2, aliens[i].Bounds.Y - aliens[i].Texture.Height / 2);

                        aliens.RemoveAt(i);
                    }

                    if (alien_gif_timer <= 0)
                    {
                        aliens[i].Texture = aliens[i].Textures[aliens[i].current_animation_frame];

                        if (aliens[i].current_animation_frame >= aliens[i].Textures.Count - 1)
                        {
                            aliens[i].current_animation_frame = 0;
                        }
                        else
                        {
                            aliens[i].current_animation_frame++;
                        }

                        alien_gif_timer = 1000;

                    }
                }
            }
        }

        private void Spawn_Star(bool temp = false, int x = -1, int y = -1)
        {
            STARS star = new STARS();
            star.temporary = temp;

            if (x == -1 && y == -1)
                star.Bounds = new Rectangle(rand.Next(star.Texture.Width + 1, this.ClientSize.Width - star.Texture.Width - 30), -30, star.Texture.Width + 15, star.Texture.Height + 15);
            else
                star.Bounds = new Rectangle(x, y, star.Texture.Width + 15, star.Texture.Height + 15);
            stars.Add(star);

            if (rand.Next(1, 111) <= rare_star_chance)
            {
                rare_star.Bounds = new Rectangle(rand.Next(rare_star.Texture.Width + 1, this.ClientSize.Width - rare_star.Texture.Width), -20, Resources.STAR.Width + 10, Resources.STAR.Height + 10);
                spawn_rare_star = true;
            }
        }

        private void Spawn_Cloud()
        {
            if (clouds.Count <= 2 && rand.Next(1, 201) <= 5)
            {
                CLOUDS cloud = new CLOUDS();
                cloud.SetTexture();
                cloud.SetHoldStar();

                cloud.Bounds = new Rectangle(this.Width + 100, rand.Next(20, this.Height - 250), cloud.Texture.Width, cloud.Texture.Height);
                clouds.Add(cloud);
            }
        }

        private void Spawn_ufo()
        {
            if (ufos.Count < 2 && rand.Next(1, 15001) <= 10)
            {
                UFOS ufo = new UFOS();
                ufo.SetTexture();
                ufo.SetBrush();
                ufo.SetHealth(StarsUpgradeLevel, RarityUpgradeLevel);

                int loc = rand.Next(1, 3);
                if (loc == 1)
                {
                    ufo.Bounds = new Rectangle(this.Width + 100, rand.Next(20, this.Height - 300), ufo.Texture.Width + ufo.health * 6 - 45, ufo.Texture.Height + ufo.health * 6 - 40);
                    ufo.HealthLabelBounds = new Rectangle(ufo.Bounds.X + ufo.Bounds.Width / 2 - 10, ufo.Bounds.Y - ufo.Texture.Height, 50, 100);
                }
                else
                {
                    ufo.Bounds = new Rectangle(-100, rand.Next(20, this.Height - 300), ufo.Texture.Width + ufo.health * 6 - 45, ufo.Texture.Height + ufo.health * 6 - 40);
                    ufo.HealthLabelBounds = new Rectangle(ufo.Bounds.X + ufo.Bounds.Width / 2 - 10, ufo.Bounds.Y - ufo.Texture.Height, 50, 100);
                }

                ufo.loc = loc;
                ufos.Add(ufo);

                allow_ufo = false;
                if (StarsUpgradeLevel <= 3)
                {
                    if (ufos.Count == 0)
                    {
                        ufo_timer = rand.Next(10000, 100010 - (StarsUpgradeLevel * RarityUpgradeLevel));
                    }
                    else if (ufos.Count == 1)
                    {
                        ufo_timer = rand.Next(10000, 55000);
                    }
                    else
                    {
                        ufo_timer = rand.Next(10000, 110000);
                    }
                }
                else
                {
                    if (ufos.Count == 0)
                    {
                        ufo_timer = rand.Next(5000, 45000);
                    }
                    else if (ufos.Count == 1)
                    {
                        ufo_timer = rand.Next(10000, 35000);
                    }
                    else
                    {
                        ufo_timer = rand.Next(20000, 100000);
                    }

                }
            }
        }

        private void Spawn_Explosion(int x, int y)
        {
            EXPLOSION expl = new EXPLOSION();
            expl.Bounds = new Rectangle(x, y, expl.Texture.Width + 100, expl.Texture.Height + 100);

            explosions.Add(expl);
        }

        private void Spawn_alien()
        {
            if (aliens.Count <= 4 && rand.Next(1, 15001) <= 10)
            {
                ALIENS alien = new ALIENS();
                alien.SetTexture();
                alien.SetHealth(StarsUpgradeLevel, RarityUpgradeLevel);
                alien.SetBrush();
                alien.SetSpeeds(StarsUpgradeLevel, RarityUpgradeLevel);

                int loc = rand.Next(1, 3);
                if (loc == 1)
                {
                    if (alien.health < 10)
                        alien.Bounds = new Rectangle(this.Width + 100, rand.Next(20, this.Height - 300), alien.Texture.Width - 30 + alien.health * 6, alien.Texture.Height + alien.health * 6 - 30);
                    else
                        alien.Bounds = new Rectangle(this.Width + 100, rand.Next(20, this.Height - 300), alien.Texture.Width - 30 + alien.health * 4, alien.Texture.Height + alien.health * 4 - 30);
                    alien.HealthLabelBounds = new Rectangle(alien.Bounds.X + alien.Bounds.Width / 2 - 10, alien.Bounds.Y - alien.Texture.Height + 10, 50, 100);
                }
                else
                {
                    if (alien.health < 10)
                        alien.Bounds = new Rectangle(-100, rand.Next(20, this.Height - 350), alien.Texture.Width + alien.health * 4 - 40, alien.Texture.Height + alien.health * 4 - 40);
                    else
                        alien.Bounds = new Rectangle(-100, rand.Next(20, this.Height - 350), alien.Texture.Width + alien.health * 4 - 40, alien.Texture.Height + alien.health * 4 - 40);
                    alien.HealthLabelBounds = new Rectangle(alien.Bounds.X + alien.Bounds.Width / 2 - 10, alien.Bounds.Y - alien.Texture.Height + 10, 50, 100);
                }

                aliens.Add(alien);

                allow_alien = false;
                if (StarsUpgradeLevel <= 3)
                {
                    if (aliens.Count == 0)
                    {
                        alien_timer = rand.Next(15000, 75010 - (StarsUpgradeLevel * RarityUpgradeLevel));
                    }
                    else if (aliens.Count > 1)
                    {
                        alien_timer = rand.Next(10000, 37500);
                    }
                    else
                    {
                        alien_timer = rand.Next(10000, 55000);
                    }
                }
                else
                {
                    if (aliens.Count == 0)
                    {
                        alien_timer = rand.Next(10000, 40000);
                    }
                    else if (aliens.Count > 1)
                    {
                        alien_timer = rand.Next(5000, 25000);
                    }
                    else
                    {
                        alien_timer = rand.Next(10000, 55000);
                    }
                }

            }
        }

        private void SpawnAlienHitParticle(int x, int y)
        {
            ALIEN_HIT_PARTICLES ahp = new ALIEN_HIT_PARTICLES();
            ahp.Bounds = new Rectangle(x, y, ahp.Texture.Width + 20, ahp.Texture.Height + 20);
            alien_hit_particles.Add(ahp);
        }

        private void SpawnUfoHitParticle(int x, int y)
        {
            UFO_HIT_PARTICLES uhp = new UFO_HIT_PARTICLES();
            uhp.Bounds = new Rectangle(x, y, uhp.Texture.Width + 20, uhp.Texture.Height + 20);
            ufo_hit_particles.Add(uhp);
        }

        private void SpawnAlienExplosion(int x, int y)
        {
            ALIEN_EXPLOSION aex = new ALIEN_EXPLOSION();
            aex.Bounds = new Rectangle(x, y, aex.Texture.Width + 100, aex.Texture.Height + 100);
            alien_explosion_particles.Add(aex);
        }

        // ---Render & Misc---


        Font VPCH2_Font40 = CustomFont.GetFont(40, FontStyle.Regular);
        Font VPCH2_Font50 = CustomFont.GetFont(50, FontStyle.Regular);
        Font VPCH2_Font20 = CustomFont.GetFont(20, FontStyle.Regular);

        SolidBrush FontBrush = new SolidBrush(Color.MistyRose);
        SolidBrush BlackBrush = new SolidBrush(Color.Black);
        SolidBrush RedBrush = new SolidBrush(Color.Red);
        SolidBrush TimerBrush = new SolidBrush(Color.Black);

        private Rectangle BackgroundBounds = new Rectangle(-10, -420, 765, 1550);
        private Rectangle PointsLabel = new Rectangle(359, 600, 500, 59);
        private Rectangle PointsLabelShadow = new Rectangle(362, 605, 500, 59);
        private Rectangle DeathsLabel = new Rectangle(718, 9, 23 * 5, 25);
        private Rectangle RecoverDisplayLabel = new Rectangle(356, 318, 128, 75);

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            g.DrawImage(Resources.BG, BackgroundBounds);

            g.DrawString($"{DeathCounter}", VPCH2_Font20, RedBrush, DeathsLabel);

            if (ball_recovery)
            {
                g.DrawString($"{death_timer / 1000}", VPCH2_Font50, TimerBrush, RecoverDisplayLabel);
            }

            if (spawn_rare_star)
            {
                g.DrawImage(Resources.RARE_STAR_MINIMIZED, rare_star.Bounds);
            }

            foreach (var star in stars)
            {
                g.DrawImage(star.Texture, star.Bounds);
            }

            foreach (var alien in aliens)
            {
                g.DrawImage(alien.Texture, alien.Bounds);
                g.DrawString(alien.health.ToString(), VPCH2_Font20, alien.forecolor, alien.HealthLabelBounds);

            }

            foreach (var ufo in ufos)
            {
                g.DrawImage(ufo.Texture, ufo.Bounds);
                g.DrawString(ufo.health.ToString(), VPCH2_Font20, ufo.forecolor, ufo.HealthLabelBounds);
            }

            foreach (var expl in explosions)
            {
                if (!expl.end_of_animation)
                {
                    g.DrawImage(expl.Texture, expl.Bounds);
                }
            }

            foreach (var particle in alien_hit_particles)
            {
                g.DrawImage(particle.Texture, particle.Bounds);
            }

            foreach (var particle in ufo_hit_particles)
            {
                g.DrawImage(particle.Texture, particle.Bounds);
            }

            foreach (var expl in alien_explosion_particles)
            {
                if (!expl.end_of_animation)
                {
                    g.DrawImage(expl.Texture, expl.Bounds);
                }
            }

            float BallCenterX = ballBounds.X + (ballBounds.Width / 2f);
            float BallCenterY = ballBounds.Y + (ballBounds.Height / 2f);

            var state = g.Save();

            g.DrawImage(Resources.PADDLE_MINIMIZED, paddleBounds);

            g.DrawString($"{points}", VPCH2_Font40, BlackBrush, PointsLabelShadow);
            g.DrawString($"{points}", VPCH2_Font40, FontBrush, PointsLabel);

            g.TranslateTransform(BallCenterX, BallCenterY);
            g.RotateTransform(ball_angle);

            if (Math.Abs(BallXVelocity) < 10)
                Ball_Texture = Resources.BALL_MINIMIZED;

            else if (Math.Abs(BallXVelocity) >= 10 && Math.Abs(BallXVelocity) < 20)
                Ball_Texture = Resources.BALL_MINIMIZED2;

            else if (Math.Abs(BallXVelocity) >= 20)
                Ball_Texture = Resources.BALL_MINIMIZED3;

            g.DrawImage(Ball_Texture, -ballBounds.Width / 2f, -ballBounds.Height / 2f, ballBounds.Width, ballBounds.Height);

            g.Restore(state);

            foreach (var cloud in clouds)
            {
                g.DrawImage(cloud.Texture, cloud.Bounds);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int newX = e.X - (paddleBounds.Width / 2);

            if (newX < 0)
            {
                newX = 0;
            }


            if (newX > this.ClientSize.Width - paddleBounds.Width)
            {
                newX = this.ClientSize.Width - paddleBounds.Width;
            }


            paddleBounds.X = newX;


        }

        private void SetPause()
        {
            if (!pause)
            {

                pause = true;
                pause_bg = new PictureBox();

                pause_bg.Image = Resources.pause_bg_blurred;
                pause_bg.SizeMode = PictureBoxSizeMode.Zoom;
                pause_bg.Location = new Point(-10, -420);
                pause_bg.Size = new Size(765, 1550);

                this.Controls.Add(pause_bg);

                PausedLabel.BackColor = Color.Black;
                PausedLabel.BringToFront();
                PausedLabel.Visible = true;

                MenuButton.BringToFront();
                MenuButton.Visible = true;

                ShopButton.BringToFront();
                ShopButton.Visible = true;

                ContinueButton.BringToFront();
                ContinueButton.Visible = true;


            }
            else
            {
                pause = false;
                this.Controls.Remove(pause_bg);

                PausedLabel.Visible = false;
                MenuButton.Visible = false;
                ShopButton.Visible = false;
                ContinueButton.Visible = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Space)
            {
                SetPause();
            }
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            pause = false;
            this.Controls.Remove(pause_bg);

            PausedLabel.Visible = false;
            MenuButton.Visible = false;
            ShopButton.Visible = false;
            ContinueButton.Visible = false;
        }

        private void ShopButton_Click(object sender, EventArgs e)
        {

            DataToSend = new List<int>() { points, StarsUpgradeLevel, RarityUpgradeLevel, UniverseUnlocked, DeathCounter };

            Form2 ShopForm = new Form2(DataToSend);

            if (ShopForm.ShowDialog() == DialogResult.OK)
            {
                List<int> DataBack = ShopForm.DataToReturn;
                points = DataBack[0];
                StarsUpgradeLevel = DataBack[1];
                RarityUpgradeLevel = DataBack[2];
                UniverseUnlocked = DataBack[3];
                rare_star_chance = RarityUpgradeLevel * 2;

                for (int i = stars.Count - 1; i >= 0; i--)
                {
                    stars.RemoveAt(i);
                }

                for (int i = 0; i < StarsUpgradeLevel; i++)
                {
                    Spawn_Star();
                }
            }

        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            DataToSend = new List<int>() { points, StarsUpgradeLevel, RarityUpgradeLevel, UniverseUnlocked, DeathCounter };

            this.DialogResult = DialogResult.OK;
            this.Hide();

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var alien in aliens)
            {
                if (!ball_recovery && allow_click && alien.Bounds.Contains(e.Location) && e.Button == MouseButtons.Left)
                {
                    allow_click = false;
                    alien.health -= StarsUpgradeLevel;

                    if (alien.health > 0)
                    {
                        int offset_x = rand.Next(-alien.Texture.Width * 2, alien.Texture.Width * 2);
                        int offset_y = rand.Next(-alien.Texture.Height, alien.Texture.Height);
                        alien.Bounds.X += offset_x;
                        alien.Bounds.Y += offset_y;

                        alien.HealthLabelBounds.X += offset_x;
                        alien.HealthLabelBounds.Y += offset_y;

                        SpawnAlienHitParticle(e.Location.X + offset_x, e.Location.Y + offset_y);
                    }
                    else
                    {
                        SpawnAlienHitParticle(e.Location.X, e.Location.Y);
                    }

                    alien_hit_gif_timer = 15;

                    click_timer = 300;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ApplyCustomFont(this);
        }
        private void ApplyCustomFont(Control container)
        {
            container.Font = CustomFont.GetFont(12, FontStyle.Regular);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataToSend = new List<int>() { points, StarsUpgradeLevel, RarityUpgradeLevel, UniverseUnlocked, DeathCounter };
        }
    }
}

// ---Additional Classes---
public class STARS
{
    public List<int> speeds = new List<int>() { 6, 8, 12, 14, 16, 14, 12, 8, 6, 8, 12, 14, 16 };
    public List<int> scores = new List<int>() { 1, 2, 3, 4, 5, 4, 3, 2, 1 };
    public Rectangle Bounds;
    public Image Texture = Resources.STAR1;

    public bool temporary = false;
}

public class EXPLOSION
{
    public Image Texture = Resources.exp1;
    public Rectangle Bounds;

    public bool end_of_animation = false;
    public int current_explosion_texture_index = 0;
}

public class RARE_STAR
{
    public int speed = 20;
    public int score = 25;
    public Rectangle Bounds;
    public Image Texture = Resources.RARE_STAR_MINIMIZED;
}

public class CLOUDS
{
    public List<int> speeds = new List<int>() { 1, 2, 3 };
    public Rectangle Bounds;
    private List<Image> Textures = new List<Image>() { Resources.cloud1, Resources.cloud2, Resources.cloud3 };
    public Image Texture;

    public bool hold_star;

    public void SetTexture()
    {
        Random rand = new Random();
        Texture = Textures[rand.Next(0, Textures.Count)];
    }

    public void SetHoldStar()
    {
        Random rand = new Random();
        if (rand.Next(0, 51) <= 10)
        {
            hold_star = true;
        }
        else
        {
            hold_star = false;
        }
    }
}

public class UFOS
{
    public List<int> speeds = new List<int>() { 1, 2 };
    public Rectangle Bounds;
    private List<Image> Textures = new List<Image>() { Resources.ufo1, Resources.ufo2, Resources.ufo3, Resources.ufo4 };
    public Image Texture;
    public int health;
    public int max_health;
    public bool spawn_explosion = false;

    public Brush forecolor;

    public Rectangle HealthLabelBounds;

    public int loc;
    public void SetTexture()
    {
        Random rand = new Random();
        Texture = Textures[rand.Next(0, Textures.Count)];
    }

    public void SetHealth(int StarLvl, int RarityLvl)
    {
        Random rand = new Random();
        health = rand.Next(5 + RarityLvl, 16 + StarLvl);

        max_health = health;
    }

    public void SetBrush()
    {
        Random rand = new Random();
        forecolor = new SolidBrush(Color.FromArgb(rand.Next(100, 256), rand.Next(100, 256), rand.Next(100, 256)));

    }



}

public class ALIENS
{
    public Image Texture;
    public Rectangle Bounds;
    public int health;
    public int max_health;
    public int current_animation_frame = 0;
    public List<Image> Textures = new List<Image>() { };
    public List<int> speeds = new List<int>() { 1, 2, 3 };
    public int loc;

    public Brush forecolor;
    public Rectangle HealthLabelBounds;

    public void SetTexture()
    {
        Random rand = new Random();

        int color = rand.Next(1, 4);
        int type = rand.Next(1, 4);

        switch (type)
        {
            case 1:
                switch (color)
                {
                    case 1:
                        Textures.Add(Resources.a1m1);
                        Textures.Add(Resources.a1m2);
                        break;
                    case 2:
                        Textures.Add(Resources.a1r1);
                        Textures.Add(Resources.a1r2);
                        break;
                    case 3:
                        Textures.Add(Resources.a1y1);
                        Textures.Add(Resources.a1y2);
                        break;
                }
                break;

            case 2:
                switch (color)
                {
                    case 1:
                        Textures.Add(Resources.a2b1);
                        Textures.Add(Resources.a2b2);
                        break;
                    case 2:
                        Textures.Add(Resources.a2g1);
                        Textures.Add(Resources.a2g2);
                        break;
                    case 3:
                        Textures.Add(Resources.a2o1);
                        Textures.Add(Resources.a2o2);
                        break;
                }
                break;
            case 3:
                switch (color)
                {
                    case 1:
                        Textures.Add(Resources.a3b1);
                        Textures.Add(Resources.a3b2);
                        break;
                    case 2:
                        Textures.Add(Resources.a3g1);
                        Textures.Add(Resources.a3g2);
                        break;
                    case 3:
                        Textures.Add(Resources.a3r1);
                        Textures.Add(Resources.a3r2);
                        break;
                }
                break;
        }

        Texture = Textures[0];

    }

    public void SetHealth(int StarLvl, int RarityLvl)
    {
        Random rand = new Random();
        health = rand.Next(3 + RarityLvl + StarLvl, 8 + StarLvl * RarityLvl);

        max_health = health;
    }

    public void SetBrush()
    {
        Random rand = new Random();
        forecolor = new SolidBrush(Color.FromArgb(rand.Next(100, 256), rand.Next(100, 256), rand.Next(100, 256)));

    }

    public void SetSpeeds(int StarsLevel, int RarityLevel)
    {
        if (StarsLevel < 4)
        {
            for (int i = 0; i < speeds.Count; i++)
            {
                speeds[i] += StarsLevel + RarityLevel - 2;
            }
        }
        else
        {
            for (int i = 0; i < speeds.Count; i++)
            {
                speeds[i] += 4;
            }
        }
    }
}

public class ALIEN_HIT_PARTICLES
{
    public List<Image> Textures = new List<Image> {
            Resources.p1, Resources.p2, Resources.p3, Resources.p4, Resources.p5, Resources.p6,
            Resources.p7, Resources.p8, Resources.p9
        };
    public bool end_of_animation = false;
    public Rectangle Bounds;
    public int current_particle_frame = 0;
    public Image Texture = Resources.p1;
}

public class UFO_HIT_PARTICLES
{
    public List<Image> Textures = new List<Image> {
            Resources.up1, Resources.up2, Resources.up3, Resources.up4, Resources.up5, Resources.up6,
            Resources.up7, Resources.up8, Resources.up9
        };
    public bool end_of_animation = false;
    public Rectangle Bounds;
    public int current_particle_frame = 0;
    public Image Texture = Resources.up1;
}

public class ALIEN_EXPLOSION
{
    public List<Image> Textures = new List<Image>()
        {
            Resources.ae1, Resources.ae2, Resources.ae3, Resources.ae4, Resources.ae5,
            Resources.ae6, Resources.ae7, Resources.ae8, Resources.ae9, Resources.ae10
        };

    public bool end_of_animation = false;
    public Rectangle Bounds;
    public int current_particle_frame = 0;
    public Image Texture = Resources.ae1;
}

