Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.ComponentModel
Imports System.Windows
Imports System.Collections
Imports DevExpress.Xpf.Grid
Imports DevExpress.Data.Filtering.Helpers

Namespace Grid_MVVM_Filtering
	Public Class FilteredDataSupport
        Public Shared ReadOnly VisibleDataProperty As DependencyProperty = DependencyProperty.RegisterAttached("VisibleData", GetType(IList), GetType(FilteredDataSupport), New PropertyMetadata(AddressOf onDataChanged))

		Public Shared Sub SetVisibleData(ByVal element As UIElement, ByVal value As IList)
			element.SetValue(VisibleDataProperty, value)
		End Sub
		Public Shared Function GetVisibleData(ByVal element As UIElement) As IList
			Return CType(element.GetValue(VisibleDataProperty), IList)
		End Function

		Private Shared Sub OnCustomRowFilter(ByVal sender As Object, ByVal e As RowFilterEventArgs)
			If e.ListSourceRowIndex <> 0 Then
				Return
			End If
			ChangeVisibleData(TryCast(sender, GridControl))
		End Sub

		Private Shared Sub OnFilterChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Dim grid As GridControl = TryCast(sender, GridControl)
			If grid.IsFilterEnabled = True Then
				ChangeVisibleData(grid)
			End If
		End Sub

		Private Shared Sub ChangeVisibleData(ByVal grid As GridControl)
			Dim res = grid.DataController.GetAllFilteredAndSortedRows().Cast(Of Object)()
			Dim visibleData = TryCast(grid.GetValue(VisibleDataProperty), IEnumerable(Of Object))
			Dim excludedData = TryCast(grid.GetValue(ExcludedDataProperty), IEnumerable(Of Object))
			If excludedData IsNot Nothing Then
				res = res.Except(excludedData)
			End If
			TryCast(visibleData, IList).Clear()
			For Each item As Object In res
				TryCast(visibleData, IList).Add(item)
			Next item
		End Sub

		Private Shared Function GetItemType(ByVal data As IEnumerable) As Type
			For Each item As Object In data
				Return item.GetType()
			Next item
			Return Nothing
		End Function

        Public Shared ReadOnly ExcludedDataProperty As DependencyProperty = DependencyProperty.RegisterAttached("ExcludedData", GetType(IList), GetType(FilteredDataSupport), New PropertyMetadata(Nothing, AddressOf onDataChanged, AddressOf coerceExcludedData))

		Public Shared Sub SetExcludedData(ByVal element As UIElement, ByVal value As IList)
			element.SetValue(ExcludedDataProperty, value)
		End Sub
		Public Shared Function GetExcludedData(ByVal element As UIElement) As IList
			Return CType(element.GetValue(ExcludedDataProperty), IList)
		End Function

		Private Shared Sub onDataChanged(ByVal sender As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
			Dim grid As GridControl = TryCast(sender, GridControl)
			If grid Is Nothing Then
				Return
			End If
			If e.OldValue Is Nothing AndAlso e.NewValue IsNot Nothing Then
				If e.Property Is ExcludedDataProperty Then
					AddHandler grid.CustomRowFilter, AddressOf excludeOnCustomRowFilter
				ElseIf e.Property Is VisibleDataProperty Then
					AddHandler grid.CustomRowFilter, AddressOf OnCustomRowFilter
					AddHandler grid.FilterChanged, AddressOf OnFilterChanged
				End If
			ElseIf e.OldValue IsNot Nothing AndAlso e.NewValue Is Nothing Then
				If e.Property Is ExcludedDataProperty Then
					RemoveHandler grid.CustomRowFilter, AddressOf excludeOnCustomRowFilter
				ElseIf e.Property Is VisibleDataProperty Then
					RemoveHandler grid.CustomRowFilter, AddressOf OnCustomRowFilter
					RemoveHandler grid.FilterChanged, AddressOf OnFilterChanged
				End If
			End If
			grid.RefreshData()
		End Sub

		Private Shared Function coerceExcludedData(ByVal sender As DependencyObject, ByVal data As Object) As Object
			Dim grid As GridControl = TryCast(sender, GridControl)
			grid.RefreshData()
			Return data
		End Function

		Private Shared Sub excludeOnCustomRowFilter(ByVal sender As Object, ByVal e As RowFilterEventArgs)
			Dim grid As GridControl = TryCast(sender, GridControl)
			Dim data As IList = TryCast(grid.ItemsSource, IList)
			Dim excludedData As IList = TryCast(grid.GetValue(ExcludedDataProperty), IList)
			e.Visible = Not excludedData.Contains(data(e.ListSourceRowIndex))
			e.Handled = Not e.Visible
		End Sub
	End Class
End Namespace
