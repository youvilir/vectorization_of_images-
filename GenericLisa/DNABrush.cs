using System;

namespace GenericLisa
{
    public class DNABrush : ICloneable
    {
        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public void SetRandom()
        {
            A = Tools.GetRandomNumber(10, 60);
            R = Tools.GetRandomNumber(0, 255);
            G = Tools.GetRandomNumber(0, 255);
            B = Tools.GetRandomNumber(0, 255);
        }

        public void Mutate(DNAWorkarea drawing)
        {
            if (Tools.WillMutate(Settings.ActiveRedMutationRate))
            {
                R = Tools.GetRandomNumber(Settings.ActiveRedRangeMin, Settings.ActiveRedRangeMax);
                drawing.IsChange = true;
            }

            if (Tools.WillMutate(Settings.ActiveGreenMutationRate))
            {
                G = Tools.GetRandomNumber(Settings.ActiveGreenRangeMin, Settings.ActiveGreenRangeMax);
                drawing.IsChange = true;
            }

            if (Tools.WillMutate(Settings.ActiveBlueMutationRate))
            {
                B = Tools.GetRandomNumber(Settings.ActiveBlueRangeMin, Settings.ActiveBlueRangeMax);
                drawing.IsChange = true;
            }

            if (Tools.WillMutate(Settings.ActiveAlphaMutationRate))
            {
                A = Tools.GetRandomNumber(Settings.ActiveAlphaRangeMin, Settings.ActiveAlphaRangeMax);
                drawing.IsChange = true;
            }
        }

        public object Clone()
        {
            return new DNABrush
            {
                A = A,
                R = R,
                G = G,
                B = B
            };
        }
    }
}
