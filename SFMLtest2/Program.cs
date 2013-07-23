using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SFML;
using SFML.Graphics;
using SFML.Window;

using System.Diagnostics;
namespace SFMLtest2
{
    static class Program
    {

        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        static void Main()
        {
            float angle = (float)(90 * (Math.PI / 180));
            float speed_x = 0.00f;
            float speed_y = 0.00f;
            float gravity = 0.0001f;

            //Gametime.
            Stopwatch time = new Stopwatch();
            time.Start();


            long fpstime = 0;
            long fpstime2 = 0;

            long fps = 0;
            long lastfps = 0;



            long currentTime = 0;
            long updateTime;
            long kickKeyTime = 0;


            float scale_x;
            float scale_y;

            float velocity_x;
            float velocity_y;


            //Angle for the ball
            scale_x = (float)(Math.Sin(angle));
            scale_y = (float)(Math.Cos(angle));

            //Gravity for the ball
            float gvelocity = (float)(Math.Cos(0) * gravity);

            //Ball speed x and y
            velocity_x = (speed_x * scale_x);
            velocity_y = (speed_y * scale_y);

            //Kick equasion
            float kick = (float)((Math.Cos((0 * (Math.PI / 180)))) / 3);


            //float velocity_xF = (float)velocity_x;
            //float velocity_yF = (float)velocity_y;

            // Create the main window
            RenderWindow app = new RenderWindow(new VideoMode(900, 600), "SFML Works!");
            app.Closed += new EventHandler(OnClose);
            Color windowColor = new Color(0, 192, 255);

            #region Texture definision
            //Zombie
            Texture TextureZombie = new Texture("textures\\zombie.png");
            Sprite zombie = new Sprite(new Texture("textures\\zombie.png"));
            zombie.Position = new Vector2f(50f, 250f);
            zombie.Scale = new Vector2f(0.4f, 0.4f);
            zombie.Texture.Smooth = true;

            //Lawn
            Texture TextureLawn = new Texture("textures\\lawn.png");
            Sprite lawn = new Sprite(new Texture("textures\\lawn.png"));
            //lawn.Position = new Vector2f(lawn.Texture.Size.X , lawn.Texture.Size.Y);
            lawn.Position = new Vector2f(0f, (600f - lawn.Texture.Size.Y));

            //Ball
            Texture TextureBall = new Texture("textures\\ball.png");
            Sprite ball = new Sprite(new Texture("textures\\ball.png"));
            ball.Position = new Vector2f(450f, 300f);
            ball.Scale = new Vector2f(0.3f, 0.3f);
            ball.Texture.Smooth = true;

            Font arial = new Font(@"C:\Windows\Fonts\arial.ttf");     //What if Font is not present on host pc?
            Text fpstext = new Text();
            fpstext.Position = new Vector2f(5f,5f);
            
            
            

            #endregion

            // Start the game loop
            while (app.IsOpen())
            {
                // Process events
                app.DispatchEvents();

                // Clear screen
                app.Clear(windowColor);

                //Gametime - something is wrong.
                updateTime = time.ElapsedMilliseconds - currentTime;
                currentTime = time.ElapsedMilliseconds;


                #region FPS
                fpstime = time.ElapsedMilliseconds - fpstime2;

                if (fpstime >= 1000)
                {
                    lastfps = fps;
                    fps = 0;
                    fpstime2 += 1000;
                    fpstext = new Text(lastfps.ToString() + " FPS", arial, 15);
                }
                fps++;

                #endregion
                 

                velocity_y += updateTime * 0.00009f;
                //velocity_x -= updateTime * 0.00009f;


                if (velocity_y < 0.38f)
                {
                    velocity_y += gvelocity;
                }



                if (ball.Position.X <= 800f)
                {
                    ball.Position += new Vector2f(velocity_x, velocity_y);
                }
                if (ball.Position.Y >= 500f)
                {
                    velocity_y = (velocity_y * -1f) + velocity_y * 0.05f;
                    velocity_x = 0f;
                    ball.Position = new Vector2f(ball.Position.X, 500f);
                }


                if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && time.ElapsedMilliseconds - kickKeyTime >= 1000)
                {
                    kickKeyTime = time.ElapsedMilliseconds;
                    velocity_y -= kick;
                    Console.WriteLine("KICK");
                }

                //Console.WriteLine(velocity_x);
                //Console.WriteLine(velocity_y);



                //Drawing onto window
                app.Draw(lawn);
                app.Draw(ball);
                app.Draw(zombie);
                app.Draw(fpstext);

                app.Display();


            } //End game loop
        } //End Main()
    } //End Program
}