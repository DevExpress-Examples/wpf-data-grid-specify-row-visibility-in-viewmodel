using Grid_MVVM_Filtering.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Grid_MVVM_Filtering {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            DataContext = new DataViewModel(10);
        }
    }
}