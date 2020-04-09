using CourierApplication.DAL;
using CourierApplication.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourierApplication.Algorithm
{
    public class SimulatingAnnealing
    {
        public List<int> BestSolution { get; set; }
        public int Iter { get; set; }

        public SimulatingAnnealing()
        {
            this.BestSolution = new List<int>();
            this.Iter = 0;
        }



        public void StartSA(List<int> initialRoute, List<Adress> listOfAdressesUnique, double temperature, double coolingTemperature, double lambda, int countOfCouriers)
        {
            List<int> currentSolution = new List<int>();
            List<int> neighbourSolution = new List<int>();
            List<double> neighbourDistance = new List<double>();
            double currentDistance = 0.0;
            double shortestDistance = 9999.0;
            double currentCmax = 9999;
            double deltaDistance = 0.0;

            for (int i = 0; i < initialRoute.Count(); i++)
            {
                currentSolution.Add(initialRoute[i]);
            }

            int size = currentSolution.Count();

            for (int i = 0; i < size - 1; i++)
            {
                var latitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == currentSolution[i] select z.Latitude).First());
                var longitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == currentSolution[i] select z.Longitude).First());
                GeoCoordinate g1 = new GeoCoordinate(latitude1, longitude1);

                var latitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == currentSolution[i + 1] select z.Latitude).First());
                var longitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == currentSolution[i + 1] select z.Longitude).First());
                GeoCoordinate g2 = new GeoCoordinate(latitude2, longitude2);

                currentDistance += g1.GetDistanceTo(g2);
            }


            for (int i = 0; i < currentSolution.Count; i++)
                BestSolution.Add(currentSolution[i]);

            shortestDistance = currentDistance;

            Random random = new Random();
            Random random2 = new Random();

            while (temperature > coolingTemperature)
            {

                int randomNumber1 = random.Next(1, size - 1);
                int randomNumber2 = random2.Next(1, size - 1);

                while (randomNumber1 == randomNumber2)
                    randomNumber2 = random.Next(1, size - 1);

                for (int i = 0; i < currentSolution.Count(); i++)
                    neighbourSolution.Add(currentSolution[i]);


                int temp = neighbourSolution[randomNumber1];
                neighbourSolution[randomNumber1] = neighbourSolution[randomNumber2];
                neighbourSolution[randomNumber2] = temp;

                int u = 0;
                double bufor = 0.0;

                for (int i = 0; i < countOfCouriers; i++)
                    neighbourDistance.Add(0);

                for (int i = 0; i < size - 1; i++)
                {
                    var latitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == neighbourSolution[i] select z.Latitude).First());
                    var longitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == neighbourSolution[i] select z.Longitude).First());
                    GeoCoordinate g1 = new GeoCoordinate(latitude1, longitude1);

                    var latitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == neighbourSolution[i + 1] select z.Latitude).First());
                    var longitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == neighbourSolution[i + 1] select z.Longitude).First());
                    GeoCoordinate g2 = new GeoCoordinate(latitude2, longitude2);

                    if (neighbourSolution[i] != 1 || i == 0 || i == size - 1)
                        bufor += g1.GetDistanceTo(g2);
                    else
                    {
                        neighbourDistance[u] = bufor;
                        bufor = 0.0;
                        bufor += g1.GetDistanceTo(g2);
                        u++;
                    }

                }

                neighbourDistance[u] = bufor;
                u = 0;
                bufor = 0;

                neighbourDistance.Sort();
                bufor = neighbourDistance.Last();

                if (bufor < currentCmax)
                    currentCmax = bufor;

                if (shortestDistance > currentCmax)
                {
                    shortestDistance = currentCmax;
                    BestSolution.Clear();
                    for (int i = 0; i < size; i++)
                        BestSolution.Add(neighbourSolution[i]);
                }

                if (currentCmax <= currentDistance)
                {
                    currentDistance = currentCmax;
                    currentSolution.Clear();
                    for (int i = 0; i < size; i++)
                        currentSolution.Add(neighbourSolution[i]);
                }
                else
                {
                    deltaDistance = currentCmax - currentDistance;
                    double p = Math.Exp((-deltaDistance) / temperature);
                    Random random3 = new Random();
                    double z = random3.Next(0, 101);
                    z = z / 100;

                    if (z <= p)
                    {
                        currentSolution.Clear();
                        for (int i = 0; i < size; i++)
                            currentSolution.Add(neighbourSolution[i]);
                        currentDistance = currentCmax;
                    }

                }


                neighbourSolution.Clear();
                neighbourDistance.Clear();
                temperature *= lambda;
                bufor = 0.0;
                Iter++;
            }
        }
    }
}
