using System;
using System.Collections.Generic;
using System.Text;

namespace CourierApplication.Infrastructure
{
    public class ResultTSP
    {
        public ResultTSP() { }
        public ResultTSP(List<int> bestS, List<double> cmax, int coi, double sum)
        {
            this.bestSolution = bestS;
            this.CMax = cmax;
            this.countOfIterations = coi;
            this.sumOfRouteKm = sum;
        }

        public List<int> bestSolution { get; set; }
        public List<double> CMax { get; set; }
        public int countOfIterations { get; set; }
        public double sumOfRouteKm { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Całkowita trasa:  ");
            sb.Append(Math.Round(sumOfRouteKm, 2).ToString());
            sb.Append(" km");
            sb.AppendLine();
            sb.AppendLine();

            for (int i = 0; i < CMax.Count; i++)
            {
                sb.Append("Liczba km kierowcy nr: ");
                sb.Append((i + 1).ToString());
                sb.Append(":  ");
                sb.Append(Math.Round(CMax[i], 2).ToString());
                sb.Append(" km");
                sb.AppendLine();
            }


            int licznik = 1;

            for (int i = 0; i < bestSolution.Count; i++)
            {

                if (bestSolution[i] == 1 && i != bestSolution.Count - 1)
                {
                    if (i != 0 && i != bestSolution.Count - 1)
                    {
                        sb.Append(" -> ");
                        sb.Append("1".ToString());
                    }
                    sb.AppendLine();
                    sb.Append("Trasa kierowcy nr: ");
                    sb.Append(licznik.ToString());
                    sb.Append(":  ");
                    sb.Append(bestSolution[i].ToString());
                    licznik++;
                }
                else
                {
                    sb.Append(" -> ");
                    sb.Append(bestSolution[i].ToString());
                }


            }

            sb.AppendLine();
            sb.AppendLine();
            sb.Append("Liczba iteracji algorytmu: ");
            sb.Append(countOfIterations.ToString());
            sb.AppendLine();

            return sb.ToString();
        }

    }
}
