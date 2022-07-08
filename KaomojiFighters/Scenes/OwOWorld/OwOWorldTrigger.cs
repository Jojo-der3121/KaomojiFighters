using Nez;

namespace KaomojiFighters.Scenes.OwOWorld
{
    class OwOWorldTrigger: BoxCollider
    {
        public OwOWorldTrigger(int width, int hight) : base(width, hight) => IsTrigger = true;

        public Enums.OwOWOrldTriggerTypes owoWorldTriggerType;
    }

    class LevelWallCollider : BoxCollider
    {
        public LevelWallCollider(int width, int hight) => IsTrigger = true;
    }
}
