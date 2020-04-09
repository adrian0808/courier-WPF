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
        public List<int> initialRoute { get; set; }
        public List<int> currentSolution { get; set; }
        public List<int> neighbourSolution { get; set; }
        public List<double> neighbourDistance { get; set; }
        public List<Adress> listOfAdressesUnique { get; set; }
        public double currentDistance { get; set; }
        public double shortestDistance { get; set; }
        public double currentCmax { get; set; }
        public double deltaDistance { get; set; }
        public double lambda { get; set; }
        public double temperature { get; set; }
        public double coolingTemperature { get; set; }
        public int countOfCouriers { get; set; }
        public int iter { get; set; }

        public SimulatingAnnealing(List<int> initialRoute, List<Adress> listOfAdressesUnique, double temperature, double coolingTemperature, double lambda, int countOfCouriers)
        {
            this.initialRoute = initialRoute;
            currentSolution = new List<int>();
            neighbourSolution = new List<int>();
            neighbourDistance = new List<double>();
            this.listOfAdressesUnique = listOfAdressesUnique;
            currentDistance = 0.0;
            shortestDistance = 9999.0;
            currentCmax = 9999;
            deltaDistance = 0.0;
            this.temperature = temperature;
            this.coolingTemperature = coolingTemperature;
            this.lambda = lambda;
            this.countOfCouriers = countOfCouriers;
        }

        

        public List<int> StartSA()
        {
            List<int> bestResults = new List<int>();

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
                bestResults.Add(currentSolution[i]);

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
                    bestResults.Clear();
                    for (int i = 0; i < size; i++)
                        bestResults.Add(neighbourSolution[i]);
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
                iter++;

            } 
            return bestResults;
        }
    }
}
