#region using

using System;
using System.Diagnostics;
using System.Windows;
using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

#endregion

namespace Galaxy.Environments.Actors
{
    public class Ship2 : DethAnimationActor
    {
        
        private const long StartFlyMs = 2000;
        
        private bool m_flying;
        private Stopwatch m_flyTimer;
        public Ship2(ILevelInfo info)
            : base(info)
        {
            Width = 25;
            Height = 25;
            ActorType = ActorType.Enemy;
        }

     

       public bool kor;
       public bool Direction
       {
           get { return kor; }
           set { kor= value; }

       }
       private void h_changePosition()
       {
           
           if (Direction == false)
           {
               Position = new Point((Position.X + 2 ), (Position.Y + 2));
           }
           else
           {
               Position = new Point((Position.X ), (Position.Y + 2));
           }
           
       }
       
        public override void Load()
        {
            Inity();
            Load(@"Assets\ship2.png");
           
        }
        protected void Inity()
        {
            if (m_flyTimer == null)
            {
                m_flyTimer = new Stopwatch();
                m_flyTimer.Start();
            }
        }

        public override void Update()
        {
            base.Update();

            if (!IsAlive)
                return;

            if (!m_flying)
            {
                if (m_flyTimer.ElapsedMilliseconds <= StartFlyMs) return;

                m_flyTimer.Stop();
                m_flyTimer = null;
                h_changePosition();
                m_flying = true;
            }
            else
            {
                h_changePosition();
            }
        }

    }
}
