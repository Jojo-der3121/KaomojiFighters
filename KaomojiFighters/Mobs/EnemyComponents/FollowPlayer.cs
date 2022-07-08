using Microsoft.Xna.Framework;
using Nez;

namespace KaomojiFighters.Mobs
{
    class FollowPlayer : Component, IUpdatable
    {

        public float LerpIndex;
        private int buffer ;

        public void Update()
        {
            Entity.Position = Vector2.Lerp(Entity.Position, Screen.Center, LerpIndex);
            if (buffer == 0)
            {
                buffer = 123;
            }


            if (buffer == 0) return;
            buffer--;
            if (buffer == 0)
            {
                Entity.Position = new Vector2(1400, 700);
            }
        }
    }
}
