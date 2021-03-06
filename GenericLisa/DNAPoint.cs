using System;


namespace GenericLisa
{
    public class DNAPoint: ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void SetRandom()
        {
            X = Tools.GetRandomNumber(0, Tools.MaxWidth);
            Y = Tools.GetRandomNumber(0, Tools.MaxHeight);
        }// генерация рандомных координат точки

        public void Mutate(DNAWorkarea drawing)
        {
            if (Tools.WillMutate(Settings.ActiveMovePointMaxMutationRate))
            {
                X = Tools.GetRandomNumber(0, Tools.MaxWidth);
                Y = Tools.GetRandomNumber(0, Tools.MaxHeight);
                drawing.IsChange = true;
            }

            if (Tools.WillMutate(Settings.ActiveMovePointMidMutationRate))
            {
                X =
                    Math.Min(
                        Math.Max(0,
                                 X +
                                 Tools.GetRandomNumber(-Settings.ActiveMovePointRangeMid,
                                                       Settings.ActiveMovePointRangeMid)), Tools.MaxWidth);
                Y =
                    Math.Min(
                        Math.Max(0,
                                 Y +
                                 Tools.GetRandomNumber(-Settings.ActiveMovePointRangeMid,
                                                       Settings.ActiveMovePointRangeMid)), Tools.MaxHeight);
                drawing.IsChange = true;
            }

            if (Tools.WillMutate(Settings.ActiveMovePointMinMutationRate))
            {
                X =
                    Math.Min(
                        Math.Max(0,
                                 X +
                                 Tools.GetRandomNumber(-Settings.ActiveMovePointRangeMin,
                                                       Settings.ActiveMovePointRangeMin)), Tools.MaxWidth);
                Y =
                    Math.Min(
                        Math.Max(0,
                                 Y +
                                 Tools.GetRandomNumber(-Settings.ActiveMovePointRangeMin,
                                                       Settings.ActiveMovePointRangeMin)), Tools.MaxHeight);
                drawing.IsChange = true;
            }
        }// мутация

        public object Clone()// клонирование
        {
            return new DNAPoint { X = X, Y = Y };
        }
    }
}
