using System;
using System.Collections.Generic;
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

using System.Configuration;
//using Microsoft.Win32;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Net.NetworkInformation;
//using Microsoft.Win32;

namespace _2324_2Y_2A_Integ_FileUploadSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext _dbConn = null;
        string picPath = null;
        string profPath = null;
        BitmapImage _default = new BitmapImage();
        public MainWindow()
        {
            InitializeComponent();
            _dbConn = new DataClasses1DataContext(Properties.Settings.Default._2324_2A_FileUploadSampleConnectionString1);
            populateCB();

            //picPath = @"C:\ProgrammingShit\Images\";
            picPath = @"C:\Standard\";
            profPath = @"C:\ProgrammingShit\Profile\";

            _default.BeginInit();
            _default.UriSource = new Uri(picPath + "Default.png");
            _default.EndInit();
        }

        private void btnAddNewUser_Click(object sender, RoutedEventArgs e)
        {
            _dbConn.uspAddUser(txtNewUser.Text);
            txtNewUser.Text = "";
            populateCB();
        }

        private void populateCB()
        {
            var things = _dbConn.uspSelectAllUsers().ToList();
            cboxUserList.SelectedIndex = -1;
            cboxUserList.Items.Clear();
            foreach(var thing in things)
            {
                cboxUserList.Items.Add(thing.UserName);
            }
        }

        private void cboxUserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboxUserList.SelectedIndex != -1) 
            {
                //MessageBox.Show(cboxUserList.SelectedValue.ToString());
                var things = _dbConn.uspSelectOneUser(cboxUserList.SelectedValue.ToString()).ToList();
                BitmapImage bmi = new BitmapImage();

                foreach(var thing in things)
                {
                    //string picPath = thing.UserPicturePath.ToString();
                    if (thing.UserPicturePath.ToString() != "")
                    {
                        bmi.BeginInit();
                        bmi.UriSource = new Uri(picPath + thing.UserPicturePath.ToString());
                        //bmi.UriSource = new Uri(@"\Testing2.jpg");
                        bmi.EndInit();

                        imageProfile.Source = bmi;
                        
                    }
                }
            }
        }

        private void btnUpdatePic_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Browse Photos...";
            ofd.DefaultExt = "png";
            ofd.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" +
                "All files (*.*)|*.*";

            ofd.ShowDialog();

            if (ofd.FileName.Length > 0)
            {
                txtPath.Text = ofd.FileName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(txtPath.Text.Length > 0 && cboxUserList.SelectedIndex > -1) 
            {
                string[] temp = txtPath.Text.Split('.');
                string ext = temp[temp.Length - 1];
                //_dbConn.uspUpdatePicturePath(cboxUserList.SelectedItem.ToString(), txtPath.Text);
                
                imageProfile.Source = _default;
                File.Copy(txtPath.Text, picPath + cboxUserList.SelectedItem + "." + ext, true);
                _dbConn.uspUpdatePicturePath(cboxUserList.SelectedItem.ToString(),cboxUserList.SelectedItem + "." + ext);
                txtPath.Text = "";
                populateCB();
            }
        }
    }
}
