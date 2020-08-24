using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Endless_Runner_Game
{
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();
        Random rand = new Random();
        ImageBrush playerSprite = new ImageBrush();
        ImageBrush background1Sprite = new ImageBrush();
        ImageBrush groundSprite = new ImageBrush();
        ImageBrush sunSprite = new ImageBrush();
        ImageBrush background2Sprite = new ImageBrush();
        ImageBrush przeszkodaSprite = new ImageBrush();
        ImageBrush jeziorkoSprite = new ImageBrush();

        Rect playerHitBox;
        Rect groundHitBox;
        Rect przeszkodaHitBox;
        Rect jeziorkoHitBox;
        
        bool jumping;
        int force = 20;
        int fallingSpeed = 5;
        int speed = 10;
        bool gameover = false;       
        double playerAnimationSpriteCounter = 0;         
        int highscore = 0;
        int score = 0;
        int[] wysokoscPrzeszkody = { 650, 620, 600, 590, 570 };

        public MainWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            myCanvas.Focus();                                                  //set the focus on my canvas from the WPF
            gameTimer.Tick += gameEngine;                                      // assign the game engine event to the game timer tick
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);                // set the game timer interval to 20 milliseconds
            background1Sprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/tlo.png"));
            background2Sprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/tlo.png"));
            sunSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/slonko.png"));
            groundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/trawa.png"));
            background1.Fill = background1Sprite;                              // add the background sprite to both rectangles
            background2.Fill = background2Sprite;
            sun.Fill = sunSprite;
            ground.Fill = groundSprite;

            StartGame();

        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && gameover)
            {
                StartGame();
            }
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && !jumping && Canvas.GetTop(player) > 260)
            {
                jumping = true;
                force = 15;
                fallingSpeed = -12;
               
                playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));  // change the player sprite so it looks like he's jumping
            }

        }

        private void StartGame()
        {
            Canvas.SetLeft(background1, 0);       // set the first background to 0
            Canvas.SetLeft(background2, 3000);    // set the second background to 3000
            Canvas.SetLeft(player, 110);
            Canvas.SetTop(player, 300);
            Canvas.SetLeft(przeszkoda, 950);
            Canvas.SetTop(przeszkoda, 610);
            Canvas.SetLeft(jeziorko, 1750);
            Canvas.SetTop(jeziorko, 700);
            runSprite(1);
            jeziorkoSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/jeziorko.png"));
            jeziorko.Fill = jeziorkoSprite;                   // assign the obstacle sprite to the obstacle object 
            przeszkodaSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/przeszkoda.png"));
            przeszkoda.Fill = przeszkodaSprite;               // assign the obstacle sprite to the obstacle object 
            jumping = false;
            gameover = false;
            score = 0;
            scoreText.Content = "Punkty: " + score;
            highScoreText.Content = "Rekord: " + highscore;
            infoText.Content = " ";
            gameTimer.Start();
        }

        private void runSprite(double i)
        {
            switch (i)
            {
                case 1:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_01.gif"));
                    break;
                case 2:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
                    break;
                case 3:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_03.gif"));
                    break;
                case 4:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_04.gif"));
                    break;
                case 5:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_05.gif"));
                    break;
                case 6:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_05.gif"));
                    break;
                case 7:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_07.gif"));
                    break;
                case 8:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_08.gif"));
                    break;
            }
            player.Fill = playerSprite;
        }



        private void gameEngine(object sender, EventArgs e)
        {
            Canvas.SetTop(player, Canvas.GetTop(player) + fallingSpeed);        // move the player character down using the speed integer
            Canvas.SetLeft(background1, Canvas.GetLeft(background1) - 3);       // move the background 3 pixels to the left each tick
            Canvas.SetLeft(background2, Canvas.GetLeft(background2) - 3);
            Canvas.SetLeft(przeszkoda, Canvas.GetLeft(przeszkoda) - speed);     // move the obstacle rectangle to the left 12 pixels per tick
            Canvas.SetLeft(jeziorko, Canvas.GetLeft(jeziorko) - speed);
            scoreText.Content = "Punkty: " + score;  // link the score text label to the score integer
            highScoreText.Content = "Rekord: " + highscore;
            // assign the player hit box to the player, ground hit box to the ground rectangle and obstacle hit box to the obstacle rectangle
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            groundHitBox = new Rect(Canvas.GetLeft(ground), Canvas.GetTop(ground), ground.Width, ground.Height);
            przeszkodaHitBox = new Rect(Canvas.GetLeft(przeszkoda), Canvas.GetTop(przeszkoda), przeszkoda.Width, przeszkoda.Height);
            jeziorkoHitBox = new Rect(Canvas.GetLeft(jeziorko), Canvas.GetTop(jeziorko), jeziorko.Width, jeziorko.Height);

            // check player and ground collision
            // IF player hits the ground 
            if (playerHitBox.IntersectsWith(groundHitBox))
            {
                //if the player is on the ground set the speed to 0
                fallingSpeed = 0;
                // place the character on top of the ground rectangle
                Canvas.SetTop(player, Canvas.GetTop(ground) - player.Height);
                // set jumping to false
                jumping = false;
                // add .5 to the sprite int double
                playerAnimationSpriteCounter += .5;
                // if the sprite int goes above 8
                if (playerAnimationSpriteCounter > 8)
                {
                    // reset the sprite int to 1
                    playerAnimationSpriteCounter = 1;
                }
                // pass the sprite int values to the run sprite function
                runSprite(playerAnimationSpriteCounter);
            }

            //if the player hit the obstacle
            if (playerHitBox.IntersectsWith(przeszkodaHitBox))
            {
                // set game over boolean to true
                gameover = true;
                // stop the game timer
                gameTimer.Stop();

            }
            if (playerHitBox.IntersectsWith(jeziorkoHitBox))
            {
                // set game over boolean to true
                gameover = true;
                // stop the game timer
                gameTimer.Stop();

            }

            //if jumping boolean is true
            if (jumping)
            {
                // set speed integer to -9 so the player will go upwards
                fallingSpeed = -15;
                // reduce the force integer
                force--;
            }
            else
            {
                // if jumping is not true then set speed to 12
                fallingSpeed = 10;
            }

            // if force is less than 0 
            if (force < 0)
            {
                // set jumping boolean to false
                jumping = false;
            }

            // parallax scrolling code for c#
            // the code below will scroll the background simlutaniously and make it seem endless

            // check the first background
            // if the first background X position goes below -3000 pixels
            if (Canvas.GetLeft(background1) < -3000)
            {
                // position the first background behind the second background
                // below we are setting the backgrounds left, to background2 width position
                Canvas.SetLeft(background1, Canvas.GetLeft(background2) + background2.Width);
            }
            // we do the same for the background 2
            // if background 2 X position goes below -3000
            if (Canvas.GetLeft(background2) < -3000)
            {
                // position the second background behind the first background
                // below we are setting background 2s left position or X position to backgrounds width position
                Canvas.SetLeft(background2, Canvas.GetLeft(background1) + background1.Width);
            }

            // if the obstacle goes beyond -50 location
            if (Canvas.GetLeft(przeszkoda) < -50)
            {
                // set the left position of the obstacle to 950 pixels
                Canvas.SetLeft(przeszkoda, 1750);
                // randomly set the top positio of the obstacle from the array we created earlier
                // this will randomly pick a position from the array so it won't be the same each time it comes around the screen
                Canvas.SetTop(przeszkoda, wysokoscPrzeszkody[rand.Next(0, wysokoscPrzeszkody.Length)]);
                // add 1 to the score
                score += 1;
                if (score > highscore)
                    highscore = score;

            }
            if (Canvas.GetLeft(jeziorko) < -50)
            {
                // set the left position of the obstacle to 950 pixels
                Canvas.SetLeft(jeziorko, 1750);
                // randomly set the top positio of the obstacle from the array we created earlier
                // this will randomly pick a position from the array so it won't be the same each time it comes around the screen
                Canvas.SetTop(jeziorko, 700);
                // add 1 to the score
                score += 1;
                if (score > highscore)
                    highscore = score;
            }
            if (score>10)speed = score;
        
            if (gameover)
            {
                // draw a black border around the obstacle
                // and set the border size to 1 pixel
                przeszkoda.Stroke = Brushes.Black;
                przeszkoda.StrokeThickness = 3;
                speed = 10;
                // draw a red border around the player
                // and set the border size to 1 pixel
                player.Stroke = Brushes.Red;
                player.StrokeThickness = 3;
                // add the following to the existing score text label
                infoText.Content = "[Enter] Jeszcze raz,  [Esc] Wyjście";

            }
            else
            {
                // if the game is not order then reset the border thickness to 0 pixel
                player.StrokeThickness = 0;
                przeszkoda.StrokeThickness = 0;
            }
        }

    }
}