#region using

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Galaxy.Core.Actors;
using Galaxy.Core.Collision;
using Galaxy.Core.Environment;
using Galaxy.Environments.Actors;
using System.Diagnostics;
using System;
using System.Threading;

#endregion

namespace Galaxy.Environments
{
    /// <summary>
    ///   The level class for Open Mario.  This will be the first level that the player interacts with.
    /// </summary>
    public class LevelOne : BaseLevel
    {
        private int m_frameCount;

        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="LevelOne" /> class.
        /// </summary>
        public LevelOne()
        {
            // Backgrounds
            FileName = @"Assets\LevelOne.png";

            // Enemies
            for (int i = 0; i < 5; i++)
            {
                var ship = new Ship(this);
                int positionY = ship.Height + 10;
                int positionX = 150 + i*(ship.Width + 50);

                ship.Position = new Point(positionX, positionY);

                Actors.Add(ship);
            }

            var molnia = new Molnia(this);
            int posY = 30;
            int posX = 100;

            molnia.Position = new Point(posX, posY);

            Actors.Add(molnia);

            int h = 7;
            for (int i = 0; i < h; i++)
            {
                var ship2 = new Ship2(this);
                int positionY = ship2.Height + 50;
                int positionX = 100 + i*(ship2.Width + 50);
                int g = h/2;
                if (i < g)
                {
                    ship2.Direction = false;
                }
                else
                {
                    ship2.Direction = true;
                }


                ship2.Position = new Point(positionX, positionY);

                Actors.Add(ship2);
            }

            // Player
            Player = new PlayerShip(this);
            int playerPositionX = Size.Width/2 - Player.Width/2;
            int playerPositionY = Size.Height - Player.Height - 50;
            Player.Position = new Point(playerPositionX, playerPositionY);
            Actors.Add(Player);
        }

        #endregion

        #region Overrides

        private void h_dispatchKey()
        {
            if (!IsPressed(VirtualKeyStates.Space)) return;

            if (m_frameCount%10 != 0) return;

            Bullet bullet = new Bullet(this)
            {
                Position = Player.Position
            };

            bullet.Load();
            Actors.Add(bullet);
        }

        public override BaseLevel NextLevel()
        {
            return new StartScreen();
        }


        public int i = 0;
        private Stopwatch strelba = new Stopwatch();

        public void Strelba()
        {
            TimeSpan ts = strelba.Elapsed;
            if (ts.TotalMilliseconds < 500)
                return;
            var enemybullets = new EnemyBullet(this);
            var enemyList = Actors.Where((actor) => actor is Ship2).ToList();
            if (enemyList.Count > 0)
            {
                i = i + 1;
                if (i >= enemyList.Count) i = 0;
                var position = enemyList[i].Position;
                enemybullets.Position = new Point(position.X + 5, position.Y + 12);
                enemybullets.Load();
                Actors.Add(enemybullets);
                strelba.Restart();
            }
        }


        public override void Update()
        {
            m_frameCount++;
            h_dispatchKey();
            Strelba();
            base.Update();

            IEnumerable<BaseActor> killedActors = CollisionChecher.GetAllCollisions(Actors);

            foreach (BaseActor killedActor in killedActors)
            {
                if (killedActor.IsAlive)
                    killedActor.IsAlive = false;
            }

            List<BaseActor> toRemove = Actors.Where(actor => actor.CanDrop).ToList();
            BaseActor[] actors = new BaseActor[toRemove.Count()];
            toRemove.CopyTo(actors);

            foreach (BaseActor actor in actors.Where(actor => actor.CanDrop))
            {
                Actors.Remove(actor);
            }

            if (Player.CanDrop)
                Failed = true;

            //has no enemy
            if (Actors.All(actor => actor.ActorType != ActorType.Enemy))
                Success = true;
        }

        public override void Load()
        {
            base.Load();
            strelba.Start();
        }

        #endregion
    }
}
