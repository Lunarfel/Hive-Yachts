using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public class CustomMath
    {
        public static float Rad2deg(float rad) => rad * 57.29578f;

        public static float Deg2rad(float deg) => deg * ((float)Math.PI / 180f);

    }
}
