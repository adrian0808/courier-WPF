using CourierApplication.DAL;
using CourierApplication.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourierApplication.Infrastructure
{
    public class ResultSA
    {
        public ResultSA(List<int> bestS, int iter)
        {
            CMax = new List<double>();
            bestSolution = bestS;
            countOfIterations = iter;
            sumOfRouteKm = 0.0;
        }

        public List<int> bestSolution { get; set; }
        public List<double> CMax { get; set; }
        public int countOfIterations { get; set; }
        public double sumOfRouteKm { get; set; }

        public void GenerateResult(List<Adress> listOfAdressesUnique, int countOfCouriers)
        {
            int size = bestSolution.Count;
            double bufor = 0.0;
            int y = 0;

            for (int i = 0; i < countOfCouriers; i++)
                CMax.Add(0);

            for (int i = 0; i < size - 1; i++)
            {
                var latitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i] select z.Latitude).SingleOrDefault());
                var longitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i] select z.Longitude).SingleOrDefault());
                GeoCoordinate g1 = new GeoCoordinate(latitude1, longitude1);

                var latitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i + 1] select z.Latitude).SingleOrDefault());
                var longitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i + 1] select z.Longitude).SingleOrDefault());
                GeoCoordinate g2 = new GeoCoordinate(latitude2, longitude2);

                if (bestSolution[i] != 1 || i == 0 || i == size - 1)
                {
                    bufor += g1.GetDistanceTo(g2);
                }
                else
                {
                    CMax[y] = bufor;
                    bufor = 0.0;
                    bufor += g1.GetDistanceTo(g2);
                    y++;
                }
                sumOfRouteKm += g1.GetDistanceTo(g2);

            }
            CMax[y] = bufor;
        }



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
