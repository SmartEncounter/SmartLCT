using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nova.SmartLCT.Interface;

namespace SmartLCT
{
    public enum SkinType { Blue, Red }

    public class SkinKey
    {
        public SkinType BlueSkin
        {
            get { return _blueSkin; }
        }
        private SkinType _blueSkin = SkinType.Blue;
        public SkinType RedSkin
        {
            get { return _redSkin; }
        }
        private SkinType _redSkin = SkinType.Red;
    }


}
