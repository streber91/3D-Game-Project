using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Grid
{
    class ParticleGenerator
    {
        #region Fields
        private int emission;
        private int newEmission;

        private GraphicsDevice device;
        private Model model;

        private Particle[] particels;

        private Matrix[] boneTransforms;
        private Random r = new Random();
        #endregion

        #region Properties
        public int Emission
        {
            set
            {
                emission = value;
            }
        }
        #endregion

        #region Constructor
        public ParticleGenerator(GraphicsDevice device, Model model, int emission)
        {

            this.device = device;
            this.model = model;
            this.emission = emission;
            this.particels = new Particle[this.emission];

            int generate = this.emission;
                        
            generatFirstParticles(generate);
            boneTransforms = new Matrix[model.Bones.Count];
        }
        #endregion
        
        #region Delete
        public void deleteParticle(int id){
            int emissionRate = 1;
            while (emissionRate > 0)
            {
                for (int y = -20; y < -10; y++)
                {
                    for (int x = -40; x < 40; x += 5)
                    {
                        for (float z = -800; z < 300; z += 10)
                        {
                            if (emissionRate > 0)
                            {
                                int direction = 0;
                                switch (r.Next(2))
                                {
                                    case 0: direction = -1; break;
                                    case 1: direction = 1; break;
                                }
                                Vector3 speed = new Vector3(r.Next(5) + 1, r.Next(1) + 1, r.Next(8) + 1) * direction;
                                float scale = MathHelper.Max(1, r.Next(30));
                                float lifeTime = MathHelper.Max(1, r.Next(10));

                                switch (r.Next(5))
                                {
                                    case 1:
                                        this.particels[id] = new Particle(this.device, this.model, new Vector3(x, -10 + y, 0 + 30), new Vector3(-(MathHelper.PiOver4 * 5 / 4), 2 * MathHelper.PiOver2, 0), speed * (1 / scale), Vector3.Zero, lifeTime * scale, scale, z, this, id);
                                        emissionRate--;
                                        break;
                                    case 2:
                                        this.particels[id] = new Particle(this.device, this.model, new Vector3(x, 40 + y, 0 + 30), new Vector3((MathHelper.PiOver4 * 5 / 4), 2 * MathHelper.PiOver2, 0), speed * (1 / scale), Vector3.Zero, lifeTime * scale, scale, z, this, id);
                                        emissionRate--;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
        

        #region Update Particles
        public void updateParticles(float time)
        {
            for (int index = 0; index <this.particels.Length; ++index)
            {
                this.particels[index].Position = new Vector3(this.particels[index].Position.X, this.particels[index].Position.Y, 0 + 50 + this.particels[index].OffSetZ);
                this.particels[index].update(time);
            }
        }
        #endregion
        
        #region Genterate Particles
        public void generatFirstParticles(int emissionRate)
        {
            int counter = 0;
            while (emissionRate > 0)
            {
                for (int y = -20; y < -10; y++)
                {
                    for (int x = -40; x < 40; x += 5)
                    {
                        for (float z = -800; z < 300; z += 10)
                        {
                            if (emissionRate > 0)
                            {
                                int direction = 0;
                                switch (r.Next(2))
                                {
                                    case 0: direction = -1; break;
                                    case 1: direction = 1; break;
                                }
                                Vector3 speed = new Vector3(r.Next(5) + 1, r.Next(1) + 1, r.Next(8) + 1) * direction;
                                float scale = MathHelper.Max(1, r.Next(30));
                                float lifeTime = MathHelper.Max(1, r.Next(10));

                                switch (r.Next(5))
                                {
                                    case 1:
                                        this.particels[counter] = new Particle(this.device, this.model, new Vector3(x, -10 + y,30), new Vector3(-(MathHelper.PiOver4 * 5 / 4), 2 * MathHelper.PiOver2, 0), speed * (1 / scale), Vector3.Zero, lifeTime * scale, scale, z, this, counter);
                                        counter++;
                                        emissionRate--;
                                        break;
                                    case 2:
                                        this.particels[counter] = new Particle(this.device, this.model, new Vector3(x, 40 + y, 30), new Vector3((MathHelper.PiOver4 * 5 / 4), 2 * MathHelper.PiOver2, 0), speed * (1 / scale), Vector3.Zero, lifeTime * scale, scale, z, this, counter);
                                        counter++;
                                        emissionRate--;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Draw
        public void drawParticles(Camera camera)
        {
            foreach (Particle p in this.particels)
                p.Draw(camera);
        }        
        #endregion

        #region Move
        public void moveParticles(float elapsed, float turpoSpeed)
        {
            foreach (Particle p in this.particels)
                p.Move(elapsed, turpoSpeed);
        }
        #endregion
    }
}
