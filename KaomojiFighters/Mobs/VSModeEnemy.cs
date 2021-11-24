using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace KaomojiFighters.Mobs
{
    class VSModeEnemy : Component, IUpdatable
    {
        public int HP = 25;
        private Scene scene;
        private Entity opponentEntity;
        private Punch EnemyAttack;
        private BoxCollider HitBox;
        private SpriteRenderer sprite;

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            opponentEntity = Entity.Scene.FindEntity("Kaomoji01");
            EnemyAttack = opponentEntity.GetComponent<Punch>();
            sprite = Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture("Kaomoji02")));
            Entity.AddComponent(new FollowPlayer() { LerpIndex = 0.02f });
            HitBox = Entity.AddComponent(new BoxCollider());
            HitBox.IsTrigger = true;
        }

        public void Update()
        {
            if (HP <= 0)
            {
                Entity.Destroy();
            }
        }
    }
}
