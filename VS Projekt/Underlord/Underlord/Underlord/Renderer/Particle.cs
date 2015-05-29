using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Renderer
{
    class Particle
    {
        ParticleGenerator generator;
        int id;
        Vector3 position, rotation, speed, rotationSpeed;
        float scale, lifeTime;

        public Particle(ParticleGenerator gen, int id, Vector3 pos, Vector3 rota, Vector3 speed, Vector3 rotaSpeed, float scale, float lifeTime)
        {
        }
        public void update()
        {
        }
        public void draw(Camera cam)
        {
        }

        private void delete()
        {
        }
        private void move()
        {
        }
    }
}
