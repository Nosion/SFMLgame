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
            float BounceEnergyLoss = 0.6f;
            float spin = 0;
            float airDrag = 0.000003f;

            //Gametime.
            Stopwatch time = new Stopwatch();
            time.Start();
            long currentTime = 0;
            long updateTime;
            long kickKeyTime = 0;

            long fpstime = 0;
            long fpsSec = 0;
            long fps = 0;


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
            //float kick = - (float)(90 * Math.PI / 180);
            float kick = (float)((Math.Cos((0 * (Math.PI / 180))))/ 3);


            //float velocity_xF = (float)velocity_x;
            //float velocity_yF = (float)velocity_y;
            #region Texture definision / Window
            // Create the main window
            uint windowX = 900; //define window length x
            uint windowY = 600; //define window length y
            RenderWindow app = new RenderWindow(new VideoMode(windowX, windowY), "SFML Works!");
            app.Closed += new EventHandler(OnClose);
            Color windowColor = new Color(0, 192, 255);
                        
            //Zombie
            Texture TextureZombie = new Texture("textures\\zombie.png");
            Sprite zombie = new Sprite(new Texture("textures\\zombie.png"));
            zombie.Position = new Vector2f(50f, 250f);
            zombie.Scale = new Vector2f(0.3f, 0.3f);
            zombie.Texture.Smooth = true;

            //Lawn
            Texture TextureLawn = new Texture("textures\\lawn.png");
            Sprite lawn = new Sprite(new Texture("textures\\lawn.png"));
            //lawn.Position = new Vector2f(lawn.Texture.Size.X , lawn.Texture.Size.Y);
            lawn.Position = new Vector2f(0f, (600f - lawn.Texture.Size.Y));


            //Ball
            Texture TextureBall = new Texture("textures\\ball.png");
            Sprite ball = new Sprite(new Texture("textures\\ball.png"));
            ball.Position = new Vector2f(100f, 300f);
            ball.Scale = new Vector2f(0.2f, 0.2f);
            ball.Texture.Smooth = true;
            ball.Origin = new Vector2f(171, 171);
            float ballSize = TextureBall.Size.X;
            float ballScale = ball.Scale.X;
            //Radius of the ball.
            float ballSizeScaled = ballSize * ballScale / 2; 
            


            //FPS
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

                //Trying to control gametime. NOT WORKING
                updateTime = time.ElapsedMilliseconds - currentTime;
                currentTime = time.ElapsedMilliseconds;
                velocity_y += updateTime * 0.00009f;

                #region FPS
                fpstime = time.ElapsedMilliseconds - fpsSec;

                if (fpstime >= 1000)
                {                    
                    fpsSec += 1000;
                    fpstext = new Text(fps.ToString() + " FPS", arial, 15);
                    fps = 0;
                }
                fps++;

                #endregion
                               
                //Adding gravity.
                if (velocity_y < 0.38f)
                {
                    velocity_y += gvelocity;
                }


                ball.Position += new Vector2f(velocity_x, velocity_y);


                //Ball moving
                if (ball.Position.X >= windowX - ballSizeScaled)  //windowX - ballSize * ball.Scale.X / 2
                {
                    velocity_x = +velocity_x * -BounceEnergyLoss;
                    ball.Position += new Vector2f(velocity_x, velocity_y);

                    //Experimental spin
                    //spin += -0.2f;
                    //ball.Rotation = spin;
                }

                //Left boundary
                if (ball.Position.X <= ballSizeScaled)
                {
                    velocity_x = -velocity_x * BounceEnergyLoss;
                    ball.Position += new Vector2f(velocity_x, velocity_y);
                }

                //Lower boundary
                if (ball.Position.Y >= windowY - ballSizeScaled)
                {
                    velocity_y = (velocity_y * -BounceEnergyLoss);//+ velocity_y * 0.05f

                    if (velocity_x != 0)
                    {
                        velocity_x = velocity_x * 0.0000000002f;
                    }
                    
                    //velocity_x = 0f;
                    ball.Position = new Vector2f(ball.Position.X, windowY-ballSizeScaled);
                }

                //Upper boundary
                if (ball.Position.Y <= ballSizeScaled)
                {
                    velocity_y = -velocity_y;
                    
                    ball.Position += new Vector2f(velocity_x, velocity_y);
                }



                //Kick
                if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && time.ElapsedMilliseconds - kickKeyTime >= 1000)
                {
                    kickKeyTime = time.ElapsedMilliseconds;
                    velocity_y -= kick;
                    velocity_x -= kick;

                    Console.WriteLine("KICK");
                }


                //Drawing onto window
                app.Draw(lawn);
                
                app.Draw(zombie);
                app.Draw(ball);
                app.Draw(fpstext);

                app.Display();


            } //End game loop
        } //End Main()
    } //End Program
}