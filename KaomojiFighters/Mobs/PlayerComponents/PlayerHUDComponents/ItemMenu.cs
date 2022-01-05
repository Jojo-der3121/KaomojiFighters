using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    class ItemMenu : Component , IUpdatable
    {
        private Player player;
        public SpriteRenderer ItemMenuDisplay;
        public List<SpriteRenderer> Textures;
        
        public override void OnAddedToEntity()
        {
            var scene = new Scene();
            base.OnAddedToEntity();
            player = Entity.GetComponent<Player>();
            Textures = new List<SpriteRenderer>();
            ItemMenuDisplay = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("ItemMenu")));
            ItemMenuDisplay.LocalOffset = Screen.Center;
            ItemMenuDisplay.Enabled = false;
            for (var i= 0; i < player.ItemList.Count; i++)
            {
                Textures.Add(Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture(player.ItemList[i].ItemType))));
                Textures[i].Size = new Vector2(27,45);
                Textures[i].LocalOffset = new Vector2(Screen.Center.X-ItemMenuDisplay.Width/2+ 30*(i+1),Screen.Center.Y);
                Textures[i].RenderLayer = 0;
                Textures[i].LayerDepth = 0;
                Textures[i].Enabled = false;
            }


        }

        public void Update()
        {
            
        }
    }
}
