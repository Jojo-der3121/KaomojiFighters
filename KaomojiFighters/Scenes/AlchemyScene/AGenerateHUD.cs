using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace KaomojiFighters.Scenes.AlchemyScene
{
    class AGenerateHUD : RenderableComponent, IUpdatable
    {
        public void Update()
        {
            throw new NotImplementedException();
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            throw new NotImplementedException();
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;
    }
}
