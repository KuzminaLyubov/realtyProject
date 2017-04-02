using System;
using System.Collections.Generic;
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
using RealtyApp.Models;
using System.Data.Entity;

namespace RealtyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RealtyDatabaseEntities _context = new RealtyDatabaseEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            this._context.Dispose();
        }

        private void Realty_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource realtyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("realtyViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // realtyViewSource.Source = [generic data source]

            // Load is an extension method on IQueryable, 
            // defined in the System.Data.Entity namespace.
            // This method enumerates the results of the query, 
            // similar to ToList but without creating a list.
            // When used with Linq to Entities this method 
            // creates entity objects and adds them to the context.
            _context.Realty.Load();

            // After the data is loaded call the DbSet<T>.Local property 
            // to use the DbSet<T> as a binding source.
            realtyViewSource.Source = _context.Realty.Local;
        }
    }
}

