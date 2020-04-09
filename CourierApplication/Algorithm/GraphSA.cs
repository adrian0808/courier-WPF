using CourierApplication.DAL;
using CourierApplication.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourierApplication.Algorithm
{
    public class GraphSA
    {
        public List<KeyValuePair<int, int>[]> RouteGraphList { get; set; }
        public List<KeyValuePair<int, double>[]> KilometersGraphList { get; set; }

        public GraphSA()
        {
            RouteGraphList = new List<KeyValuePair<int, int>[]>();
            KilometersGraphList = new List<KeyValuePair<int, double>[]>();
        }

        public void GenerateGraphs(List<int> bestSolution, List<Adress> listOfAdressesUnique)
        {
            KeyValuePair<int, double>[] kilometersGraph;
            KeyValuePair<int, int>[] routeGraph;
            int size = bestSolution.Count;
            RouteGraphList.Clear();
            KilometersGraphList.Clear();

            double sumGraphs = 0.0;
            int countOfRoutes = 1;
            kilometersGraph = new KeyValuePair<int, double>[size];
            routeGraph = new KeyValuePair<int, int>[size];

            for (int i = 0; i < size - 1; i++)
            {
                var latitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i] select z.Latitude).First());
                var longitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i] select z.Longitude).First());
                GeoCoordinate g1 = new GeoCoordinate(latitude1, longitude1);

                var latitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i + 1] select z.Latitude).First());
                var longitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i + 1] select z.Longitude).First());
                GeoCoordinate g2 = new GeoCoordinate(latitude2, longitude2);

                if ((bestSolution[i] == 1 && i != 0) || i == size - 2)
                {
                    if (i == size - 2)
                    {
                        sumGraphs += g1.GetDistanceTo(g2);
                        kilometersGraph[i] = new KeyValuePair<int, double>(countOfRoutes, sumGraphs);
                        KilometersGraphList.Add(kilometersGraph);
                        KeyValuePair<int, double>[] tmpKilometersGraph = new KeyValuePair<int, double>[size];
                        kilometersGraph = tmpKilometersGraph;

                        routeGraph[i] = new KeyValuePair<int, int>(countOfRoutes, (from z in listOfAdressesUnique where z.AdressId == bestSolution[i] select z.AdressId).SingleOrDefault());
                        routeGraph[i+1] = new KeyValuePair<int, int>(countOfRoutes + 1, 1);
                        RouteGraphList.Add(routeGraph);
                        KeyValuePair<int, int>[] tmpRouteGraph = new KeyValuePair<int, int>[size];
                        routeGraph = tmpRouteGraph;
                    }
                    else
                    {
                        KilometersGraphList.Add(kilometersGraph);
                        KeyValuePair<int, double>[] tmpKilometersGraph = new KeyValuePair<int, double>[size];
                        kilometersGraph = tmpKilometersGraph;

                        routeGraph[i] = new KeyValuePair<int, int>(countOfRoutes + 1, 1);
                        RouteGraphList.Add(routeGraph);
                        KeyValuePair<int, int>[] tmpRouteGraph = new KeyValuePair<int, int>[size];
                        routeGraph = tmpRouteGraph;

                        sumGraphs = 0;
                        countOfRoutes = 1;
                    }
                }

                sumGraphs += g1.GetDistanceTo(g2);
                kilometersGraph[i] = new KeyValuePair<int, double>(countOfRoutes, sumGraphs);
                routeGraph[i] = new KeyValuePair<int, int>(countOfRoutes, (from z in listOfAdressesUnique where z.AdressId == bestSolution[i] select z.AdressId).SingleOrDefault());
                countOfRoutes++;

            }

        }
    }

}
