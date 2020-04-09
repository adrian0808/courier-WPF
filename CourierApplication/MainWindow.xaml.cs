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
using CourierApplication.Algorithm;

namespace CourierApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CourierDbContext db;
        private IService<Courier> courierServices;
        private IService<Client> clientServices;
        private IService<Order> orderServices;
        private IService<Adress> adressServices;
        double bufor = 0.0;
        double p = 0.0;
        double currentDistance = 0.0;
        double shortestDistance = 999.0;
        double currentCmax = 999.0;
        double deltaDistance = 0.0;
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
            db = new CourierDbContext();
            courierServices = new CourierService();
            clientServices = new ClientService();
            orderServices = new OrderService();
            adressServices = new AdressService();
        }


        private void Swap(ref List<int> listTmp, ref int a, ref int b)
        {
            int temp = listTmp[a];
            listTmp[a] = listTmp[b];
            listTmp[b] = temp;
        }

        private void RefreshCourierDataGridView()
        {
            courierGrid.ItemsSource = courierServices.LoadData();
        }
        private void RefreshClientDataGridView()
        {
            clientGrid.ItemsSource = clientServices.LoadData();
        }
        private void RefreshOrderDataGridView()
        {
            orderGrid.ItemsSource = orderServices.LoadData();
        }
        private void RefreshAdressDataGridView()
        {
            adressGrid.ItemsSource = adressServices.LoadData();
        }

        private void LoadCourierButtonClick(object sender, RoutedEventArgs e)
        {
            if ((LoadCourierButton.Content as string) == "Load Couriers")
            {
                courierGrid.ItemsSource = courierServices.LoadData();
                LoadCourierButton.Content = "Unload Couriers";
            }
            else
            {
                courierGrid.ItemsSource = null;
                LoadCourierButton.Content = "Load Couriers";
            }
        }

        private void LoadClientButtonClick(object sender, RoutedEventArgs e)
        {
            if ((LoadClientButton.Content as string) == "Load Clients")
            {
                clientGrid.ItemsSource = clientServices.LoadData();
                LoadClientButton.Content = "Unload Clients";
            }
            else
            {
                clientGrid.ItemsSource = null;
                LoadClientButton.Content = "Load Clients";
            }
        }

        private void LoadOrderButtonClick(object sender, RoutedEventArgs e)
        {
            if ((LoadOrderButton.Content as string) == "Load Orders")
            {
                orderGrid.ItemsSource = orderServices.LoadData();
                LoadOrderButton.Content = "Unload Orders";
            }
            else
            {
                orderGrid.ItemsSource = null;
                LoadOrderButton.Content = "Load Orders";
            }
        }

        private void LoadAdressButtonClick(object sender, RoutedEventArgs e)
        {
            if ((LoadAdressButton.Content as string) == "Load Adresses")
            {
                adressGrid.ItemsSource = adressServices.LoadData();
                LoadAdressButton.Content = "Unload Adresses";
            }
            else
            {
                adressGrid.ItemsSource = null;
                LoadAdressButton.Content = "Load Adresses";
            }
        }

        private void AddCourierButtonClick(object sender, RoutedEventArgs e)
        {
            string firstname = FirstNameCourier.Text;
            string lastname = LastNameCourier.Text;

            bool isFirstnameValid = firstname.All(Char.IsLetter);
            bool isLastnameValid = lastname.All(Char.IsLetter);

            if (isFirstnameValid == true && isLastnameValid == true)
            {
                Courier courier = new Courier() { FirstName = firstname, LastName = lastname, isFree = false };
                courierServices.AddData(courier);
                MessageBox.Show("Row was inserted successfully");
                RefreshCourierDataGridView();
            }
            else
            {
                MessageBox.Show("Invalid data!");
            }
        }
        private void AddClientButtonClick(object sender, RoutedEventArgs e)
        {
            string adressId = AdressIdClient.Text;
            bool isAdressValid = adressId.All(Char.IsDigit);
            if (isAdressValid == true)
            {
                var isAdressExist = db.Adresses.Where(a => a.AdressId == int.Parse(adressId)).SingleOrDefault();
                if (isAdressExist != null)
                {
                    Client client = new Client() { AdressId = int.Parse(adressId) };
                    clientServices.AddData(client);
                    MessageBox.Show("Row was inserted successfully");
                    RefreshClientDataGridView();
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
        private void AddOrderButtonClick(object sender, RoutedEventArgs e)
        {
            string adressId = AdressIdOrder.Text;
            bool isAdressValid = adressId.All(Char.IsDigit);
            if (isAdressValid == true)
            {
                var isAdressExist = db.Adresses.Where(a => a.AdressId == int.Parse(adressId)).SingleOrDefault();
                if (isAdressExist != null)
                {
                    Order order = new Order() { AdressId = int.Parse(adressId) };
                    orderServices.AddData(order);
                    MessageBox.Show("Row was inserted successfully");
                    RefreshOrderDataGridView();
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

        private void AddAdressButtonClick(object sender, RoutedEventArgs e)
        {
            string name = NameAdress.Text;
            string latitude = LatitudeAdress.Text;
            string longitude = LongitudeAdress.Text;
            decimal latitudeDec;
            decimal longitudeDec;

            if (decimal.TryParse(latitude, out latitudeDec) && decimal.TryParse(longitude, out longitudeDec))
            {
                Adress adress = new Adress() { Name = name, Latitude = latitudeDec, Longitude = longitudeDec };
                adressServices.AddData(adress);
                MessageBox.Show("Row was inserted successfully");
                RefreshAdressDataGridView();
            }
            else
            {
                MessageBox.Show("Invalid data!");
            }
        }

 
        private void UpdateCourierButtonClick(object sender, RoutedEventArgs e)
        {
            Courier courierRow = courierGrid.SelectedItem as Courier;
            courierServices.UpdateData(courierRow);
            MessageBox.Show("Row was updated successfully");
            RefreshOrderDataGridView();
        }

        private void UpdateClientButtonClick(object sender, RoutedEventArgs e)
        {
            Client clientRow = clientGrid.SelectedItem as Client;
            clientServices.UpdateData(clientRow);
            MessageBox.Show("Row was updated successfully");
            RefreshClientDataGridView();
        }

        private void UpdateOrderButtonClick(object sender, RoutedEventArgs e)
        {
            Order orderRow = orderGrid.SelectedItem as Order;
            orderServices.UpdateData(orderRow);
            MessageBox.Show("Row was updated successfully");
            RefreshOrderDataGridView();
        }

        private void UpdateAdressButtonClick(object sender, RoutedEventArgs e)
        {
            Adress adressRow = adressGrid.SelectedItem as Adress;
            adressServices.UpdateData(adressRow);
            MessageBox.Show("Row was updated successfully");
            RefreshAdressDataGridView();
        }

        private void DeleteCourierButtonClick(object sender, RoutedEventArgs e)
        {
            Courier courierRow = courierGrid.SelectedItem as Courier;
            courierServices.DeleteData(courierRow);
            MessageBox.Show("Row was deleted successfully");
            RefreshCourierDataGridView();
        }

        private void DeleteClientButtonClick(object sender, RoutedEventArgs e)
        {
            Client clientRow = clientGrid.SelectedItem as Client;
            clientServices.DeleteData(clientRow);
            MessageBox.Show("Row was deleted successfully");
            RefreshClientDataGridView();
        }

        private void DeleteOrderButtonClick(object sender, RoutedEventArgs e)
        {
            Order orderRow = orderGrid.SelectedItem as Order;
            orderServices.DeleteData(orderRow);
            MessageBox.Show("Row was deleted successfully");
            RefreshOrderDataGridView();
        }

        private void DeleteAdressButtonClick(object sender, RoutedEventArgs e)
        {
            Adress adressRow = adressGrid.SelectedItem as Adress;
            adressServices.DeleteData(adressRow);
            MessageBox.Show("Row was deleted successfully");
            RefreshAdressDataGridView();
        }


        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            await Start();
        }

        private async Task Start()
        {
            await Task.Run(() =>
            {
                /*Loading couriers and orders from text box*/
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

                /*Creating initial route based on orders*/
                int countOfRoute = orders.Count() + countOfCouriers + 1;
                List<int> initialRoute = new List<int>();
                List<Adress> listOfAdresses = new List<Adress>();
                List<Adress> listOfAdressesUnique = new List<Adress>();

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

                int size = initialRoute.Count;
                listOfAdressesUnique = listOfAdresses.Distinct().ToList();

                /*Algorithm*/

                double temperature = 0.0;
                double cooling_temperature = 0.0;
                double lambda = 0.0;

                this.Dispatcher.Invoke((Action)delegate
                {
                    temperature = Convert.ToDouble(InitTemp.Text);
                    cooling_temperature = Convert.ToDouble(CoolingTemp.Text);
                    lambda = Convert.ToDouble(Lambda.Text);
                });


                SimulatingAnnealing SA = new SimulatingAnnealing(initialRoute, listOfAdressesUnique, temperature, cooling_temperature, lambda, countOfCouriers);
                List<int> bestResult = SA.StartSA();

                
                double sumGraphs = 0.0;
                double sum = 0.0;
                int countOfRoutes = 1;
                kilometersGraph = new KeyValuePair<int, double>[bestResult.Count];
                routeGraph = new KeyValuePair<int, int>[bestResult.Count];

                for (int i = 0; i < size - 1; i++)
                {
                    var latitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestResult[i] select z.Latitude).First());
                    var longitude1 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestResult[i] select z.Longitude).First());
                    GeoCoordinate g1 = new GeoCoordinate(latitude1, longitude1);

                    var latitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestResult[i + 1] select z.Latitude).First());
                    var longitude2 = Decimal.ToDouble((from z in listOfAdressesUnique where z.AdressId == bestResult[i + 1] select z.Longitude).First());
                    GeoCoordinate g2 = new GeoCoordinate(latitude2, longitude2);

                    if ((bestResult[i] == 1 && i != 0) || i == size - 2)
                    {
                        if (i == size - 2)
                        {
                            sumGraphs += g1.GetDistanceTo(g2);
                            sum += g1.GetDistanceTo(g2);
                            kilometersGraph[i] = new KeyValuePair<int, double>(countOfRoutes, sumGraphs);
                            routeGraph[i] = new KeyValuePair<int, int>(countOfRoutes, db.Adresses.Where(a => a.AdressId == bestResult[i]).Select(b => b.AdressId).SingleOrDefault());
                            countOfRoutes++;
                        }

                        kilometersGraphList.Add(kilometersGraph);
                        KeyValuePair<int, double>[] tmpKilometersGraph = new KeyValuePair<int, double>[bestResult.Count];
                        kilometersGraph = tmpKilometersGraph;

                        routeGraph[i] = new KeyValuePair<int, int>(countOfRoutes + 1, 1);
                        routeGraphList.Add(routeGraph);
                        KeyValuePair<int, int>[] tmpRouteGraph = new KeyValuePair<int, int>[bestResult.Count];
                        routeGraph = tmpRouteGraph;

                        sumGraphs = 0;
                        countOfRoutes = 1;
                    }

                    if (i != size - 2)
                    {
                        sumGraphs += g1.GetDistanceTo(g2);
                        sum += g1.GetDistanceTo(g2);
                        kilometersGraph[i] = new KeyValuePair<int, double>(countOfRoutes, sumGraphs);
                        routeGraph[i] = new KeyValuePair<int, int>(countOfRoutes, db.Adresses.Where(a => a.AdressId == bestResult[i]).Select(b => b.AdressId).SingleOrDefault());
                        countOfRoutes++;
                    }
                }

               
                int y = 0;

                for (int i = 0; i < countOfCouriers; i++)
                    bestDistance.Add(0);

                for (int i = 0; i < size - 1; i++)
                {
                    var latitude1 = Decimal.ToDouble(db.Adresses.Where(a => a.AdressId == bestResult[i]).Select(x => x.Latitude).Single());
                    var longitude1 = Decimal.ToDouble(db.Adresses.Where(a => a.AdressId == bestResult[i]).Select(x => x.Longitude).Single());
                    GeoCoordinate g1 = new GeoCoordinate(latitude1, longitude1);

                    var latitude2 = Decimal.ToDouble(db.Adresses.Where(a => a.AdressId == bestResult[i + 1]).Select(x => x.Latitude).Single());
                    var longitude2 = Decimal.ToDouble(db.Adresses.Where(a => a.AdressId == bestResult[i + 1]).Select(x => x.Longitude).Single());
                    GeoCoordinate g2 = new GeoCoordinate(latitude2, longitude2);

                    if (bestResult[i] != 1 || i == 0 || i == size - 1)
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
               

                ResultTSP results = new ResultTSP(bestResult, bestDistance, SA.iter, sum);

                this.Dispatcher.Invoke((Action)delegate
                {
                    ResultTextBox.Text = results.ToString();
                });

                this.Dispatcher.Invoke((Action)delegate
                {
                    temperature = Convert.ToDouble(InitTemp.Text);
                    cooling_temperature = Convert.ToDouble(CoolingTemp.Text);
                    lambda = Convert.ToDouble(Lambda.Text);
                });
               
                bestDistance.Clear();
                sum = 0.0;
                sumGraphs = 0.0;
                p = 0.0;
                y = 0;
                bufor = 0.0;
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

