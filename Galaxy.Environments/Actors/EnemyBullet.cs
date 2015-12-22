#region using

using System.Drawing;
using Galaxy.Core.Actors;
using Galaxy.Core.Environment;

#endregion

namespace Galaxy.Environments.Actors
{
    public class EnemyBullet : BaseActor
    {
        #region Constant

        private const int Speed = 10;

        #endregion

        #region Constructors

        public EnemyBullet(ILevelInfo info)
            : base(info)
        {
            Width = 3;
            Height = 3;
            ActorType = ActorType.Enemy;
        }

        #endregion

        #region Overrides

        public override void Load()
        {
            Load(@"Assets\bullet.png");
        }

        public override bool IsAlive
        {
            get { return m_isAlive; }
            set
            {
                m_isAlive = value;
                CanDrop = !value;
            }
        }

        public override void Update()
        {
            Size levelSize = Info.GetLevelSize();

            Position = new Point(Position.X, Position.Y + Speed);

            if (Position.Y > levelSize.Height)
            {
                IsAlive = false;
            }
            else
            {
                IsAlive = true;
            }
        }

        #endregion
    }
}
