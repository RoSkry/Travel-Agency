using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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

namespace task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DBTravelAgency dBTravelAgency;
        DBForUsers dBforUsers;
        DBForAdmin dBForAdmin;
        public MainWindow()
        {
          InitializeComponent();
          dBTravelAgency   = new DBTravelAgency();
          dBforUsers = new DBForUsers();
          dBForAdmin = new DBForAdmin();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dBTravelAgency.CreateDataBase();     
                                                                         
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                Application.Current.Shutdown();
            } 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt1.Text) || string.IsNullOrWhiteSpace(psb1.Password))
                {
                    txt1.BorderBrush = Brushes.Red;
                    txt1.BorderThickness = new Thickness(2);
                    psb1.BorderBrush = Brushes.Red;
                    psb1.BorderThickness = new Thickness(2);
                    MessageBox.Show("You didn't fill login or password");
                    return;
                }
                User result = dBTravelAgency.SelectRole(txt1.Text, psb1.Password);
                if (result == null)
                {
                    MessageBox.Show("Incorrect login or password was written");
                    return;
                }

                if (result.Role == "User")
                {
                    this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "2.jpg")));
                    auth.Visibility = Visibility.Hidden;
                    Userview.Visibility = Visibility.Visible;
                    dBforUsers.SelectAll();
                    dtgtourist.ItemsSource = dBforUsers.tourists;
                    dtgtour.ItemsSource = dBforUsers.tours;
                    dtgdisc.ItemsSource = dBforUsers.discounts;
                    List<int> TourIds = new List<int>();
                    List<int> DiscountsIds = new List<int>();
                    TourIds.AddRange(dBforUsers.tours.Select(r => r.Id));
                    DiscountsIds.AddRange(dBforUsers.discounts.Select(r => r.Id));
                    comboBoxColum.ItemsSource = TourIds;
                    cmbdisc.ItemsSource = DiscountsIds;
                   
              

                    stuser.DataContext = result;
                }
                else if (result.Role == "Admin")
                {
                    this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "london.jpg")));
                    auth.Visibility = Visibility.Hidden;
                    Adminview.Visibility = Visibility.Visible;
                    dBForAdmin.SelectAdminAll();
                    dtgadmtour.ItemsSource = dBForAdmin.tours;
                    dtgusertour.ItemsSource = dBForAdmin.users;
                }
                else
                {
                    MessageBox.Show("Incorrect login or password was written");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (dtgtourist.SelectedIndex >= 0)
            {
                 TouristInfo tourist=new TouristInfo();
                int index = dtgtourist.SelectedIndex;

                try
                {
                    tourist.FirstName = (dtgtourist.Items[index] as TouristInfo).FirstName;
                    tourist.LastName = (dtgtourist.Items[index] as TouristInfo).LastName;

                    tourist.Serial = (dtgtourist.Items[index] as TouristInfo).Serial;
                    tourist.Number = (dtgtourist.Items[index] as TouristInfo).Number;
                    tourist.TourId = (dtgtourist.Items[index] as TouristInfo).TourId;
                    tourist.PhoneNumber = (dtgtourist.Items[index] as TouristInfo).PhoneNumber;
                    tourist.DiscountId = (dtgtourist.Items[index] as TouristInfo).DiscountId;
                    tourist.TotalSum = dBforUsers.TourPrice(tourist.TourId)*(100- dBforUsers.SelectDiscount(tourist.DiscountId))/100;
                    if (tourist.TotalSum == 0) tourist.TotalSum = dBforUsers.TourPrice(tourist.TourId);
                    dBforUsers.AddTourist(tourist);
                    dBforUsers.tourists.Add(tourist);
                    UpdateDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDatabase();
        }
        private void UpdateDatabase()
        {
            dBforUsers.tourists = null;
            dBforUsers.tourists = new List<TouristInfo>();
            dBforUsers.tours = null;
            dBforUsers.tours = new List<TourInfo>();
            dBforUsers.discounts = null;
            dBforUsers.discounts = new List<Discount>();
            dBforUsers.SelectAll();
            dtgtourist.ItemsSource = dBforUsers.tourists;
            dtgtour.ItemsSource = dBforUsers.tours;
            dtgdisc.ItemsSource = dBforUsers.discounts;
            List<int> TourIds = new List<int>();
            TourIds.AddRange(dBforUsers.tours.Select(r => r.Id));
            comboBoxColum.ItemsSource = TourIds;
            List<int> DiscountsIds = new List<int>();
            TourIds.AddRange(dBforUsers.tours.Select(r => r.Id));
            DiscountsIds.AddRange(dBforUsers.discounts.Select(r => r.Id));
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dtgtourist.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Are you sure you want to delete", "Check", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    try
                    {
                        TouristInfo tourist = new TouristInfo();
                        int index = dtgtourist.SelectedIndex;
                        tourist.Id = (dtgtourist.Items[index] as TouristInfo).Id;
                        dBforUsers.tourists.Remove(dtgtourist.Items[index] as TouristInfo);
                        //  dBStore.students.RemoveAt(index);
                        dBforUsers.DeleteTourist(tourist.Id);
                        UpdateDatabase();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            if (dtgtourist.SelectedIndex >= 0)
            {
                TouristInfo tourist = new TouristInfo();
                int index = dtgtourist.SelectedIndex;

                tourist.Id= (dtgtourist.Items[index] as TouristInfo).Id;
                tourist.FirstName = (dtgtourist.Items[index] as TouristInfo).FirstName;
                tourist.LastName = (dtgtourist.Items[index] as TouristInfo).LastName;
                tourist.Serial = (dtgtourist.Items[index] as TouristInfo).Serial;
                tourist.Number = (dtgtourist.Items[index] as TouristInfo).Number;
                tourist.TourId = (dtgtourist.Items[index] as TouristInfo).TourId;
                tourist.PhoneNumber = (dtgtourist.Items[index] as TouristInfo).PhoneNumber;
                tourist.DiscountId = (dtgtourist.Items[index] as TouristInfo).DiscountId;
                tourist.TotalSum = dBforUsers.TourPrice(tourist.TourId) * (100 - dBforUsers.SelectDiscount(tourist.DiscountId)) / 100;
                if (tourist.TotalSum == 0) tourist.TotalSum = dBforUsers.TourPrice(tourist.TourId);

                dBforUsers.UpdateTourist(tourist);
              
                UpdateDatabase();
            }
        }


        private void Addbtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (dtgadmtour.SelectedIndex >= 0)
            {
                TourInfo tour = new TourInfo();
                int index = dtgadmtour.SelectedIndex;

                try
                {
                    tour.TourCountry = (dtgadmtour.Items[index] as TourInfo).TourCountry;
                    tour.TourCity = (dtgadmtour.Items[index] as TourInfo).TourCity;

                    tour.TourBegin = (dtgadmtour.Items[index] as TourInfo).TourBegin;
                    tour.TourEnd = (dtgadmtour.Items[index] as TourInfo).TourEnd;
                    tour.MaxSeats = (dtgadmtour.Items[index] as TourInfo).MaxSeats;
                    tour.CurrentSeats = (dtgadmtour.Items[index] as TourInfo).CurrentSeats;
                    tour.DiscountId = (dtgadmtour.Items[index] as TourInfo).DiscountId;
                    tour.Price = (dtgadmtour.Items[index] as TourInfo).Price;
                    dBForAdmin.AddTour(tour);
                    dBForAdmin.tours.Add(tour);
                    AdminUpdateDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Dltbtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (dtgadmtour.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Are you sure you want to delete", "Check", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    try
                    {
                        TourInfo tour = new TourInfo();
                        int index = dtgadmtour.SelectedIndex;
                        tour.Id = (dtgadmtour.Items[index] as TourInfo).Id;
                        //dBTravelAgency.tours.Remove(dtgadmtour.Items[index] as TourInfo);
                        //  dBStore.students.RemoveAt(index);
                        dBForAdmin.DeleteTour(tour.Id);
                        AdminUpdateDatabase();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Chgbtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (dtgadmtour.SelectedIndex >= 0)
            {
                TourInfo tour = new TourInfo();
                int index = dtgadmtour.SelectedIndex; ;

                tour.TourCountry = (dtgadmtour.Items[index] as TourInfo).TourCountry;
                tour.TourCity = (dtgadmtour.Items[index] as TourInfo).TourCity;
                tour.Id = (dtgadmtour.Items[index] as TourInfo).Id;
                tour.TourBegin = (dtgadmtour.Items[index] as TourInfo).TourBegin;
                tour.TourEnd = (dtgadmtour.Items[index] as TourInfo).TourEnd;
                tour.MaxSeats = (dtgadmtour.Items[index] as TourInfo).MaxSeats;
                tour.CurrentSeats = (dtgadmtour.Items[index] as TourInfo).CurrentSeats;
                tour.DiscountId = (dtgadmtour.Items[index] as TourInfo).DiscountId;
                tour.Price = (dtgadmtour.Items[index] as TourInfo).Price;

                dBForAdmin.UpdateTour(tour);

                AdminUpdateDatabase();
            }
        }

        private void AdminUpdateDatabase()
        {
            dBForAdmin.tours = null;
            dBForAdmin.tours = new List<TourInfo>();
            dBForAdmin.users = null;
            dBForAdmin.users = new List<User>();
            dBForAdmin.SelectAdminAll();
            dtgadmtour.ItemsSource = dBForAdmin.tours;
            dtgusertour.ItemsSource = dBForAdmin.users;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (dtgusertour.SelectedIndex >= 0)
            {
                User user = new User();
                int index = dtgusertour.SelectedIndex;

                try
                {
                    user.LastName = (dtgusertour.Items[index] as User).LastName;
                    user.FirstName = (dtgusertour.Items[index] as User).FirstName;

                    user.BirthDate = (dtgusertour.Items[index] as User).BirthDate;
                    user.Userlogin = (dtgusertour.Items[index] as User).Userlogin;
                    user.UserPassword = (dtgusertour.Items[index] as User).UserPassword;
                    dBForAdmin.AddUser(user);
                    dBForAdmin.users.Add(user);
                    AdminUpdateDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DltUser_Click(object sender, RoutedEventArgs e)
        {
            if (dtgusertour.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Are you sure you want to delete", "Check", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    try
                    {
                        User user = new User();
                        int index = dtgusertour.SelectedIndex;

                        user.Id = (dtgusertour.Items[index] as User).Id;
                        //dBTravelAgency.tours.Remove(dtgadmtour.Items[index] as TourInfo);
                        //  dBStore.students.RemoveAt(index);
                        dBForAdmin.DeleteUser(user.Id);
                        AdminUpdateDatabase();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            if (dtgusertour.SelectedIndex >= 0)
            {
                User user = new User();
                int index = dtgusertour.SelectedIndex;

                try
                {
                    user.LastName = (dtgusertour.Items[index] as User).LastName;
                    user.FirstName = (dtgusertour.Items[index] as User).FirstName;
                    user.Id = (dtgusertour.Items[index] as User).Id;
                    user.BirthDate = (dtgusertour.Items[index] as User).BirthDate;
                    user.Userlogin = (dtgusertour.Items[index] as User).Userlogin;
                    user.UserPassword = (dtgusertour.Items[index] as User).UserPassword;
                    dBForAdmin.UpdateUser(user);                  
                    AdminUpdateDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
 
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Adminview.Visibility = Visibility.Hidden;
            Userview.Visibility = Visibility.Hidden;
            auth.Visibility = Visibility.Visible;
            txt1.Text = "";
            psb1.Password = "";
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "1.jpg")));
            dBForAdmin.tours.Clear();
            dBforUsers.tours.Clear();
            dBForAdmin.users.Clear();
            dBforUsers.tourists.Clear();
            dBforUsers.discounts.Clear();
            txt1.BorderBrush = null;
            psb1.BorderBrush = null;
       
        }
    }
    }

