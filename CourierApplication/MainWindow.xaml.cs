using CourierApplication.DAL;
using CourierApplication.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourierApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CourierDbContext db = new CourierDbContext();
        double currentDistance = 0.0;
        double temperature = 10000;
        double cooling_temperature = 0.1;
        double lambda = 0.99995;
        double delta_distance = 0.0;
        double shortestDistance = 9999.0;
        double bufor = 0.0;
        double currentCmax = 999.0;
        double p = 0.0;
        int iter = 0;
        List<double> neighbourDistance = new List<double>();
        List<double> bestDistance = new List<double>();


        public MainWindow()
        {
            InitializeComponent();
        }


        public void Swap(ref List<int> listTmp, ref int a, ref int b)
        {
            int temp = listTmp[a];
            listTmp[a] = listTmp[b];
            listTmp[b] = temp;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            await Start();
        }

        private async Task Start()
        {
            await Task.Run(() =>
            {
                /*Load available couriers*/
                int countOfCouriers = db.Couriers.Where(c => c.isFree == true).Count();

                /*Load initial route*/
                List<Order> orders = db.Orders.Where(o => o.isCompleted == false).Take(14).ToList();
                int countOfRoute = orders.Count() + countOfCouriers + 1;

                List<int> initialRoute = new List<int>();
                List<Adress> listOfAdresses = new List<Adress>();


                for (int i = 0; i < countOfRoute; i++)
                {
                    if (i == 0)
                    {
                        initialRoute.Add(1);
                        listOfAdresses.Add(db.Adresses.Where(a => a.AdressId == 1).Single());
                    }
                    else if (orders.Count() != 0)
                    {
                        initialRoute.Add(orders[0].AdressId);
                        listOfAdresses.Add(db.Adresses.Where(a => a.AdressId == orders[0].AdressId).Single());
                        orders.RemoveAt(0);
                    }
                    else
                    {
                        initialRoute.Add(1);
                    }

                }

                var listOfAdressesUnique = listOfAdresses.Distinct().ToList();

                /*Algorithm*/

                List<int> currentSolution = new List<int>();
                List<int> neighbourSolution = new List<int>();


                for (int i = 0; i < initialRoute.Count(); i++)
                {
                    currentSolution.Add(initialRoute[i]);
                }


                int size = currentSolution.Count();

                for (int i = 0; i < currentSolution.Count - 1; i++)
                {
                    var latitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == currentSolution[i] select z.Latitude).First());
                    var longitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == currentSolution[i] select z.Longitude).First());
                    GeoCoordinate g1 = new GeoCoordinate(latitude1, longitude1);

                    var latitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == currentSolution[i + 1] select z.Latitude).First());
                    var longitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == currentSolution[i + 1] select z.Longitude).First());
                    GeoCoordinate g2 = new GeoCoordinate(latitude2, longitude2);

                    currentDistance += g1.GetDistanceTo(g2);
                }

                List<int> bestSolution = new List<int>();
                for (int i = 0; i < currentSolution.Count; i++)
                    bestSolution.Add(currentSolution[i]);

                shortestDistance = currentDistance;

                for (int i = 0; i < countOfCouriers; i++)
                    neighbourDistance.Add(0.0);

                Random random = new Random();
                Random random2 = new Random();

                while (temperature > cooling_temperature)
                {

                    int randomNumber1 = random.Next(1, size - 1);
                    int randomNumber2 = random2.Next(1, size - 1);

                    while (randomNumber1 == randomNumber2)
                        randomNumber2 = random.Next(1, size - 1);

                    for (int i = 0; i < currentSolution.Count(); i++)
                        neighbourSolution.Add(currentSolution[i]);

                    Swap(ref neighbourSolution, ref randomNumber1, ref randomNumber2);

                    int u = 0;

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
                        bestSolution.Clear();
                        for (int i = 0; i < size; i++)
                            bestSolution.Add(neighbourSolution[i]);
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
                        delta_distance = currentCmax - currentDistance;
                        p = Math.Exp((-delta_distance) / temperature);
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

                double suma = 0.0;
                for (int i = 0; i < size - 1; i++)
                {
                    var latitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i] select z.Latitude).First());
                    var longitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i] select z.Longitude).First());
                    GeoCoordinate g1 = new GeoCoordinate(latitude1, longitude1);

                    var latitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i + 1] select z.Latitude).First());
                    var longitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestSolution[i + 1] select z.Longitude).First());
                    GeoCoordinate g2 = new GeoCoordinate(latitude2, longitude2);

                    suma += g1.GetDistanceTo(g2);
                }

                int y = 0;

                for (int i = 0; i < countOfCouriers; i++)
                    bestDistance.Add(0);

                for (int i = 0; i < size - 1; i++)
                {
                    var latitude1 = Decimal.ToDouble(db.Adresses.Where(a => a.AdressId == bestSolution[i]).Select(x => x.Latitude).Single());
                    var longitude1 = Decimal.ToDouble(db.Adresses.Where(a => a.AdressId == bestSolution[i]).Select(x => x.Longitude).Single());
                    GeoCoordinate g1 = new GeoCoordinate(latitude1, longitude1);

                    var latitude2 = Decimal.ToDouble(db.Adresses.Where(a => a.AdressId == bestSolution[i + 1]).Select(x => x.Latitude).Single());
                    var longitude2 = Decimal.ToDouble(db.Adresses.Where(a => a.AdressId == bestSolution[i + 1]).Select(x => x.Longitude).Single());
                    GeoCoordinate g2 = new GeoCoordinate(latitude2, longitude2);

                    if (bestSolution[i] != 1 || i == 0 || i == size - 1)
                    {
                        bufor += g1.GetDistanceTo(g2);
                    }
                    else
                    {
                        bestDistance[y] = bufor;
                        bufor = 0.0;
                        bufor += g1.GetDistanceTo(g2);
                        y++;
                    }

                }
                bestDistance[y] = bufor;
                y = 0;


                for (int i = 0; i < bestDistance.Count; i++)
                {
                    this.Dispatcher.Invoke((Action)delegate
                    {
                        ResultTextBox.Text += "Cmax : " + i + " " + Math.Round(bestDistance[i], 2);
                        ResultTextBox.Text += "\r\n";
                    });
                }
                this.Dispatcher.Invoke((Action)delegate
                {
                    ResultTextBox.Text += "Suma : " + Math.Round(suma, 2);
                    ResultTextBox.Text += "\r\n";
                    ResultTextBox.Text += "\r\n";
                });

                
                for (int i = 0; i < bestSolution.Count; i++)
                {
                    this.Dispatcher.Invoke((Action)delegate
                    {
                        ResultTextBox.Text += bestSolution[i] + "   ";
                    });
                }

                this.Dispatcher.Invoke((Action)delegate
                {
                    ResultTextBox.Text += "\r\n";
                    ResultTextBox.Text += iter;
                    ResultTextBox.Text += "\r\n";
                });
                
                delta_distance = 0.0;
                currentDistance = 0.0;
                shortestDistance = 999.0;
                currentCmax = 999.0;
                p = 0.0;
                iter = 0;
                temperature = 1000;
                cooling_temperature = 0.1;
                lambda = 0.95;
                currentSolution.Clear();
                bestDistance.Clear();
                suma = 0.0;
            });
        }


    }
}

