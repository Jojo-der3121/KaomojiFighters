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
        private int HP = 25;
        private Scene scene;
        private int StunTimer;
        private Entity opponentEntity;
        private Punch EnemyAttack;
        private BoxCollider HitBox;
        private SpriteRenderer sprite;
        private Vector2 StunPosition;

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            opponentEntity = Entity.Scene.FindEntity("Kaomoji01");
            EnemyAttack = opponentEntity.GetComponent<Punch>();
            sprite = Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture("Kaomoji02")));
            Entity.AddComponent(new FollowPlayer() { LerpIndex = 0.02f });
            HitBox = Entity.AddComponent(new BoxCollider());
        }

        public void Update()
        {
            if (EnemyAttack.collider != null)
            {
                if (HitBox.CollidesWith(EnemyAttack.collider, out var hitResult) && EnemyAttack.collider.Enabled)
                {
                    HP--;
                    Entity.Position = new Vector2(Entity.Position.X + 200, Entity.Position.Y - 75);
                    StunPosition = Entity.Position;
                    sprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji02Hurt"));
                    StunTimer = 15;
                    Entity.Rotation +=1 ;
                }
            }

            if (StunTimer > 0)
            {
                StunTimer--;
                Entity.Position = StunPosition;
                if (StunTimer == 0)
                {
                    sprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji02"));
                    Entity.Rotation = 0;
                }
            }
            if (HP <= 0)
            {
                Entity.Destroy();
            }
        }
    }
}
