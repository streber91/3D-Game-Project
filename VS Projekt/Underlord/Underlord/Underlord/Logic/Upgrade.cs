using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Underlord.Entity
{
    class Upgrade : Thing
    {

            String name = "";
            int cost = 150;
            float size = 1;
            Ability effect;

        public Upgrade(String name, Ability eff)
        {
            this.name = name;
            this.effect = eff;
        }
        public Ability getEffect()
        {
            return this.effect;
        }
        public String getName()
        {
            return this.name;
        }

    }
}
