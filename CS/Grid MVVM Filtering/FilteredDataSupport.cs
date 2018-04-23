using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Collections;
using DevExpress.Xpf.Grid;
using DevExpress.Data.Filtering.Helpers;

namespace Grid_MVVM_Filtering {
    public class FilteredDataSupport {
        public static readonly DependencyProperty VisibleDataProperty =
            DependencyProperty.RegisterAttached("VisibleData", typeof(IList), typeof(FilteredDataSupport), new PropertyMetadata(onDataChanged));

        public static void SetVisibleData(UIElement element, IList value) {
            element.SetValue(VisibleDataProperty, value);
        }
        public static IList GetVisibleData(UIElement element) {
            return (IList)element.GetValue(VisibleDataProperty);
        }

        static void OnCustomRowFilter(object sender, RowFilterEventArgs e) {
            if(e.ListSourceRowIndex != 0)
                return;
            ChangeVisibleData(sender as GridControl);
        }

        static void OnFilterChanged(object sender, RoutedEventArgs e) {
            GridControl grid = sender as GridControl;
            if(grid.IsFilterEnabled == true)
                ChangeVisibleData(grid);
        }

        static void ChangeVisibleData(GridControl grid) {
            var res = grid.DataController.GetAllFilteredAndSortedRows().Cast<object>();
            var visibleData = grid.GetValue(VisibleDataProperty) as IEnumerable<object>;
            var excludedData = grid.GetValue(ExcludedDataProperty) as IEnumerable<object>;
            if(excludedData != null) {
                res = res.Except(excludedData);
            }
            (visibleData as IList).Clear();
            foreach(object item in res) {
                (visibleData as IList).Add(item);
            }
        }

        private static Type GetItemType(IEnumerable data) {
            foreach(object item in data)
                return item.GetType();
            return null;
        }

        public static readonly DependencyProperty ExcludedDataProperty =
            DependencyProperty.RegisterAttached("ExcludedData", typeof(IList), typeof(FilteredDataSupport), new PropertyMetadata(null, onDataChanged, coerceExcludedData));

        public static void SetExcludedData(UIElement element, IList value) {
            element.SetValue(ExcludedDataProperty, value);
        }
        public static IList GetExcludedData(UIElement element) {
            return (IList)element.GetValue(ExcludedDataProperty);
        }

        private static void onDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            GridControl grid = sender as GridControl;
            if(grid == null)
                return;
            if(e.OldValue == null && e.NewValue != null) {
                if(e.Property == ExcludedDataProperty)
                    grid.CustomRowFilter += excludeOnCustomRowFilter;
                else if(e.Property == VisibleDataProperty) {
                    grid.CustomRowFilter += OnCustomRowFilter;
                    grid.FilterChanged += OnFilterChanged;
                }
            } else if(e.OldValue != null && e.NewValue == null) {
                if(e.Property == ExcludedDataProperty)
                    grid.CustomRowFilter -= excludeOnCustomRowFilter;
                else if(e.Property == VisibleDataProperty) {
                    grid.CustomRowFilter -= OnCustomRowFilter;
                    grid.FilterChanged -= OnFilterChanged;
                }
            }
            grid.RefreshData();
        }

        private static object coerceExcludedData(DependencyObject sender, object data) {
            GridControl grid = sender as GridControl;
            grid.RefreshData();
            return data;
        }

        static void excludeOnCustomRowFilter(object sender, RowFilterEventArgs e) {
            GridControl grid = sender as GridControl;
            IList data = grid.ItemsSource as IList;
            IList excludedData = grid.GetValue(ExcludedDataProperty) as IList;
            e.Visible = !excludedData.Contains(data[e.ListSourceRowIndex]);
            e.Handled = !e.Visible;
        }
    }
}
