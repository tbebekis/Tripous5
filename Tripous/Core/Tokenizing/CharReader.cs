using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Tokenizing
{

    public interface ICharReader
    {

    }

    public class CharReader: ICharReader
    {
        StringBuilder SB;
        int fPosition;

        public CharReader(string Text)
        {
            SB = new StringBuilder(Text);
        }







        public int Length => SB.Length;
        public int Position => fPosition;
    }
}
