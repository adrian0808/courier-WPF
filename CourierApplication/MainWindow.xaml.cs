using CourierApplication.DAL;
using CourierApplication.Infrastructure;
using CourierApplication.Model;
using System.Text.RegularExpressions;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.DataVisualization.Charting;
using System.Threading;

namespace CourierApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CourierDbContext db = new CourierDbContext();
        double currentDistance = 0.0;
        double delta_distance = 0.0;
        double shortestDistance = 9999.0;
        double bufor = 0.0;
        double currentCmax = 999.0;
        double p = 0.0;
        int iter = 0;
        List<double> neighbourDistance = new List<double>();
        List<double> bestDistance = new List<double>();
        List<KeyValuePair<int, double>[]> kilometersGraphList = new List<KeyValuePair<int, double>[]>();
        KeyValuePair<int, double>[] kilometersGraph;
        List<KeyValuePair<int, int>[]> routeGraphList = new List<KeyValuePair<int, int>[]>();
        KeyValuePair<int, int>[] routeGraph;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Swap(ref List<int> listTmp, ref int a, ref int b)
        {
            int temp = listTmp[a];
            listTmp[a] = listTmp[b];
            listTmp[b] = temp;
        }

        private void LoadCourierButton_Click(object sender, RoutedEventArgs e)
        {
            if ((LoadCourierButton.Content as string) == "Load Couriers")
            {
                LoadCourierData();
            }
            else
            {
                UnloadCourierData();
            }

        }

        private void LoadClientButton_Click(object sender, RoutedEventArgs e)
        {
            if ((LoadClientButton.Content as string) == "Load Clients")
            {
                LoadClientData();
            }
            else
            {
                UnloadClientData();
            }
        }

        private void LoadOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if ((LoadOrderButton.Content as string) == "Load Orders")
            {
                LoadOrderData();
            }
            else
            {
                UnloadOrderData();
            }
        }

        private void LoadAdressButton_Click(object sender, RoutedEventArgs e)
        {
            if ((LoadAdressButton.Content as string) == "Load Adresses")
            {
                LoadAdressData();
            }
            else
            {
                UnloadAdressData();
            }
        }

        private void AddCourierButton_Click(object sender, RoutedEventArgs e)
        {
            AddCourierData();
        }
        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            AddClientData();
        }
        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrderData();
        }

        private void AddAdressButton_Click(object sender, RoutedEventArgs e)
        {
            AddAdressData();
        }

        private void LoadCourierData()
        {
            var couriers = db.Couriers.ToList();
            courierGrid.ItemsSource = couriers;
            LoadCourierButton.Content = "Unload Couriers";
        }

        private void UnloadCourierData()
        {
            courierGrid.ItemsSource = null;
            LoadCourierButton.Content = "Load Couriers";
        }

        private void LoadClientData()
        {
            var clients = db.Clients.ToList();
            clientGrid.ItemsSource = clients; ;
            LoadClientButton.Content = "Unload Clients";
        }

        private void UnloadClientData()
        {
            clientGrid.ItemsSource = null;
            LoadClientButton.Content = "Load Clients";
        }

        private void LoadOrderData()
        {
            var orders = db.Orders.ToList();
            orderGrid.ItemsSource = orders;
            LoadOrderButton.Content = "Unload Orders";
        }

        private void UnloadOrderData()
        {
            orderGrid.ItemsSource = null;
            LoadOrderButton.Content = "Load Orders";
        }

        private void LoadAdressData()
        {
            var adresses = db.Adresses.ToList();
            adressGrid.ItemsSource = adresses;
            LoadAdressButton.Content = "Unload Adresses";
        }

        private void UnloadAdressData()
        {
            adressGrid.ItemsSource = null;
            LoadAdressButton.Content = "Load Adresses";
        }

        private void UpdateCourierButton(object sender, RoutedEventArgs e)
        {
            Courier courierRow = courierGrid.SelectedItem as Courier;
            Courier courier = db.Couriers.Where(c => c.CourierId == courierRow.CourierId).Single();
            courier.FirstName = courierRow.FirstName;
            courier.LastName = courierRow.LastName;
            courier.isFree = courierRow.isFree;
            db.SaveChanges();
            MessageBox.Show("Row was updated successfully");
            LoadCourierData();
        }

        private void UpdateClientButton(object sender, RoutedEventArgs e)
        {
            Client clientRow = clientGrid.SelectedItem as Client;
            Client client = db.Clients.Where(c => c.ClientId == clientRow.ClientId).Single();
            client.AdressId = clientRow.AdressId;
            db.SaveChanges();
            MessageBox.Show("Row was updated successfully");
            LoadClientData();
        }

        private void UpdateOrderButton(object sender, RoutedEventArgs e)
        {
            Order orderRow = orderGrid.SelectedItem as Order;
            Order order = db.Orders.Where(o => o.OrderId == orderRow.OrderId).Single();
            order.AdressId = orderRow.AdressId;
            order.isCompleted = orderRow.isCompleted;
            db.SaveChanges();
            MessageBox.Show("Row was updated successfully");
            LoadOrderData();
        }

        private void UpdateAdressButton(object sender, RoutedEventArgs e)
        {
            Adress adressRow = adressGrid.SelectedItem as Adress;
            Adress adress = db.Adresses.Where(a => a.AdressId == adressRow.AdressId).Single();
            adress.Name = adressRow.Name;
            adress.Latitude = adressRow.Latitude;
            adress.Longitude = adressRow.Longitude;
            db.SaveChanges();
            MessageBox.Show("Row was updated successfully");
            LoadAdressData();
        }

        private void DeleteCourierButton(object sender, RoutedEventArgs e)
        {
            Courier courierRow = courierGrid.SelectedItem as Courier;
            Courier courier = db.Couriers.Where(c => c.CourierId == courierRow.CourierId).Single();
            db.Remove(courier);
            db.SaveChanges();
            MessageBox.Show("Row was deleted successfully");
            LoadCourierData();
        }

        private void DeleteClientButton(object sender, RoutedEventArgs e)
        {
            Client clientRow = clientGrid.SelectedItem as Client;
            Client client = db.Clients.Where(c => c.ClientId == clientRow.ClientId).Single();
            db.Remove(client);
            db.SaveChanges();
            MessageBox.Show("Row was deleted successfully");
            LoadClientData();
        }

        private void DeleteOrderButton(object sender, RoutedEventArgs e)
        {
            Order orderRow = orderGrid.SelectedItem as Order;
            Order order = db.Orders.Where(o => o.OrderId == orderRow.OrderId).Single();
            db.Remove(order);
            db.SaveChanges();
            MessageBox.Show("Row was deleted successfully");
            LoadOrderData();
        }

        private void DeleteAdressButton(object sender, RoutedEventArgs e)
        {
            Adress adressRow = adressGrid.SelectedItem as Adress;
            Adress adress = db.Adresses.Where(a => a.AdressId == adressRow.AdressId).Single();
            db.Remove(adress);
            db.SaveChanges();
            MessageBox.Show("Row was deleted successfully");
            LoadAdressData();
        }

        private void AddCourierData()
        {
            string firstname = FirstNameCourier.Text;
            string lastname = LastNameCourier.Text;

            bool isFirstnameValid = firstname.All(Char.IsLetter);
            bool isLastnameValid = lastname.All(Char.IsLetter);

            if (isFirstnameValid == true && isLastnameValid == true)
            {
                Courier courier = new Courier() { FirstName = firstname, LastName = lastname, isFree = false };
                db.Add(courier);
                db.SaveChanges();
                MessageBox.Show("Row was inserted successfully");
                LoadCourierData();
            }
            else
            {
                MessageBox.Show("Invalid data!");
            }
        }

        private void AddClientData()
        {
            string adressId = AdressIdClient.Text;
            bool isAdressValid = adressId.All(Char.IsDigit);
            if (isAdressValid == true)
            {
                var isAdressExist = db.Adresses.Where(a => a.AdressId == int.Parse(adressId)).SingleOrDefault();
                if (isAdressExist != null)
                {
                    Client client = new Client() { AdressId = int.Parse(adressId) };
                    db.Add(client);
                    db.SaveChanges();
                    MessageBox.Show("Row was inserted successfully");
                    LoadClientData();
                }
                else
                {
                    MessageBox.Show("Adress doesn't exist!");
                }
            }
            else
            {
                MessageBox.Show("Invalid data!");
            }

        }

        private void AddOrderData()
        {
            string adressId = AdressIdOrder.Text;
            bool isAdressValid = adressId.All(Char.IsDigit);
            if (isAdressValid == true)
            {
                var isAdressExist = db.Orders.Where(a => a.AdressId == int.Parse(adressId)).SingleOrDefault();
                if (isAdressExist != null)
                {
                    Order order = new Order() { AdressId = int.Parse(adressId) };
                    db.Add(order);
                    db.SaveChanges();
                    MessageBox.Show("Row was inserted successfully");
                    LoadOrderData();
                }
                else
                {
                    MessageBox.Show("Adress doesn't exist!");
                }
            }
            else
            {
                MessageBox.Show("Invalid data!");
            }
        }

        private void AddAdressData()
        {
            string name = NameAdress.Text;
            string latitude = LatitudeAdress.Text;
            string longitude = LongitudeAdress.Text;
            decimal latitudeDec;
            decimal longitudeDec;

            if (decimal.TryParse(latitude, out latitudeDec) && decimal.TryParse(longitude, out longitudeDec))
            {

                Adress adress = new Adress() { Name = name, Latitude = latitudeDec, Longitude = longitudeDec };
                db.Adresses.Add(adress);
                db.SaveChanges();
                MessageBox.Show("Row was inserted successfully");
                LoadAdressData();
            }
            else
            {
                MessageBox.Show("Invalid data!");
            }
        }



        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            await Start();
        }

        private async Task Start()
        {
            await Task.Run(() =>
            {
                int countOfCouriers = 0;
                List<Order> orders = new List<Order>();

                this.Dispatcher.Invoke((Action)delegate
                {
                    countOfCouriers = db.Couriers.Where(c => c.isFree == true).Count();
                    if (int.Parse(numOrders.Text) > db.Orders.Where(o => o.isCompleted == false).Count())
                    {
                        MessageBox.Show("Nie ma tylu zamowien w bazie!\nPrzetworzono wszystkie zamowienia");
                        orders = db.Orders.Where(o => o.isCompleted == false).Take(db.Orders.Where(o => o.isCompleted == false).Count() - 1).ToList();
                    }

                    else
                        orders = db.Orders.Where(o => o.isCompleted == false).Take(int.Parse(numOrders.Text)).ToList();
                });



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

                double temperature = 0.0;
                double cooling_temperature = 0.0;
                double lambda = 0.0;

                this.Dispatcher.Invoke((Action)delegate
                {
                    temperature = Convert.ToDouble(InitTemp.Text);
                    cooling_temperature = Convert.ToDouble(CoolingTemp.Text);
                    lambda = Convert.ToDouble(Lambda.Text);
                });




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

                double sumGraphs = 0.0;
                double sum = 0.0;
                int countOfRoutes = 1;
                kilometersGraph = new KeyValuePair<int, double>[bestSolution.Count];
                routeGraph = new KeyValuePair<int, int>[bestSolution.Count];

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
                        if(i == size -2)
                        {
                            sumGraphs += g1.GetDistanceTo(g2);
                            sum += g1.GetDistanceTo(g2);
                            kilometersGraph[i] = new KeyValuePair<int, double>(countOfRoutes, sumGraphs);
                            routeGraph[i] = new KeyValuePair<int, int>(countOfRoutes, db.Adresses.Where(a => a.AdressId == bestSolution[i]).Select(b => b.AdressId).SingleOrDefault());
                            countOfRoutes++;
                        }
                        
                        kilometersGraphList.Add(kilometersGraph);
                        KeyValuePair<int, double>[] tmpKilometersGraph = new KeyValuePair<int, double>[bestSolution.Count];
                        kilometersGraph = tmpKilometersGraph;

                        routeGraph[i] = new KeyValuePair<int, int>(countOfRoutes + 1, 1);
                        routeGraphList.Add(routeGraph);
                        KeyValuePair<int, int>[] tmpRouteGraph = new KeyValuePair<int, int>[bestSolution.Count];
                        routeGraph = tmpRouteGraph;

                        sumGraphs = 0;
                        countOfRoutes = 1;
                    }

                    sumGraphs += g1.GetDistanceTo(g2);
                    sum += g1.GetDistanceTo(g2);
                    kilometersGraph[i] = new KeyValuePair<int, double>(countOfRoutes, sumGraphs);
                    routeGraph[i] = new KeyValuePair<int, int>(countOfRoutes, db.Adresses.Where(a => a.AdressId == bestSolution[i]).Select(b => b.AdressId).SingleOrDefault());
                    countOfRoutes++;
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

                ResultTSP results = new ResultTSP(bestSolution, bestDistance, iter, sum);

                this.Dispatcher.Invoke((Action)delegate
                {
                    ResultTextBox.Text = results.ToString();
                });


                delta_distance = 0.0;
                currentDistance = 0.0;
                shortestDistance = 999.0;
                currentCmax = 999.0;
                p = 0.0;
                iter = 0;
                this.Dispatcher.Invoke((Action)delegate
                {
                    temperature = Convert.ToDouble(InitTemp.Text);
                    cooling_temperature = Convert.ToDouble(CoolingTemp.Text);
                    lambda = Convert.ToDouble(Lambda.Text);
                });
                currentSolution.Clear();
                bestDistance.Clear();
                sum = 0.0;
                sumGraphs = 0.0;
            });
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var page1 = new Page1();
            this.Dispatcher.Invoke((Action)delegate
            {
                for (int i = 0; i < kilometersGraphList.Count; i++)
                {
                    ((LineSeries)page1.GraphsOfKm.Series[i]).ItemsSource = kilometersGraphList[i];
                }
            });
            MainGraph.Content = page1;
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            var page2 = new Page2();
            this.Dispatcher.Invoke((Action)delegate
            {
                for (int i = 0; i < kilometersGraphList.Count; i++)
                {
                    ((LineSeries)page2.GraphsOfRoute.Series[i]).ItemsSource = routeGraphList[i];
                }
            });
            MainGraph.Content = page2;
        }
    }
}

