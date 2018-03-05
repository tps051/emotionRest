using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEmotionsRESTAPI
{
    class FaceObject
    {
        public FaceRectangle faceRectangle { get; set; }
        public Scores scores { get; set; }
    }
}
