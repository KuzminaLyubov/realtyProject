﻿using System;
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
        RealtyDatabaseEntities _db = new RealtyDatabaseEntities();

        bool IsReadOnlyMode { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _db.Dispose();
        }

        private void Realty_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();

            loginWindow.ShowDialog();

            if (loginWindow.Regime == LoginWindow.LoginRegime.Exit)
            {
                Close();
                return;
            }

            CollectionViewSource realEstateViewSource = ((CollectionViewSource)(this.FindResource("realEstateViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            //realEstateViewSource.Source = [generic data source]

            //https://msdn.microsoft.com/en-us/library/jj574514(v=vs.113).aspx

            // Load is an extension method on IQueryable, 
            // defined in the System.Data.Entity namespace.
            // This method enumerates the results of the query, 
            // similar to ToList but without creating a list.
            // When used with Linq to Entities this method 
            // creates entity objects and adds them to the context.

            _db.RealEstates.Load();

            // After the data is loaded call the DbSet<T>.Local property 
            // to use the DbSet<T> as a binding source.
            realEstateViewSource.Source = _db.RealEstates.Local;
        }

        private void _realtyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Search()
        {
            _realtyListBox.ItemsSource = _db.RealEstates.Local
                .Where(realEstate => realEstate.Address.ToLower()
                            .Contains(_searchTextBox.Text.ToLower()));
        }

        private void _searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
    }
}

