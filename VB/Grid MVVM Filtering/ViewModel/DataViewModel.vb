Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Collections.ObjectModel
Imports Grid_MVVM_Filtering.Model
Imports System.Windows.Input

Namespace Grid_MVVM_Filtering.ViewModel
	Friend Class DataViewModel
		Inherits ObservableObject
		Public Sub New(ByVal length As Integer)
			CreateData(length)
		End Sub

		Private Sub CreateData(ByVal length As Integer)
			Data = New ObservableCollection(Of DataModel)()
			VisibleData = New ObservableCollection(Of DataModel)()
			ExcludedData = New ObservableCollection(Of DataModel)()
			For i As Integer = 0 To length - 1
				Data.Add(New DataModel With {.Text = "Row" & i, .Number = i})
			Next i
		End Sub

		Private _Data As ObservableCollection(Of DataModel)
		Private _VisibleData As ObservableCollection(Of DataModel)
		Private _ExcludedData As ObservableCollection(Of DataModel)
		Private _AddExclusionCommand As RelayCommand(Of DataModel)
		Private _RemoveExclusionCommand As RelayCommand(Of DataModel)

		Public Property Data() As ObservableCollection(Of DataModel)
			Get
				Return _Data
			End Get
			Set(ByVal value As ObservableCollection(Of DataModel))
				_Data = value
				OnPropertyChanged("Data")
			End Set
		End Property

		Public Property VisibleData() As ObservableCollection(Of DataModel)
			Get
				Return _VisibleData
			End Get
			Set(ByVal value As ObservableCollection(Of DataModel))
				_VisibleData = value
				OnPropertyChanged("VisibleData")
			End Set
		End Property

		Public Property ExcludedData() As ObservableCollection(Of DataModel)
			Get
				Return _ExcludedData
			End Get
			Set(ByVal value As ObservableCollection(Of DataModel))
				_ExcludedData = value
				OnPropertyChanged("ExcludedData")
			End Set
		End Property

		Public ReadOnly Property AddExclusionCommand() As ICommand
			Get
				If _AddExclusionCommand Is Nothing Then
                    _AddExclusionCommand = New RelayCommand(Of DataModel)(Sub(param) Me.AddExclusion(param), Function(param) Me.CanAddExclusion(param))
				End If
				Return _AddExclusionCommand
			End Get
		End Property

		Private Sub AddExclusion(ByVal param As DataModel)
			ExcludedData.Add(param)
			OnPropertyChanged("ExcludedData")
		End Sub

		Private Function CanAddExclusion(ByVal param As DataModel) As Boolean
			Return param IsNot Nothing AndAlso Data.Contains(param) AndAlso Not ExcludedData.Contains(param)
		End Function

		Public ReadOnly Property RemoveExclusionCommand() As ICommand
			Get
				If _RemoveExclusionCommand Is Nothing Then
                    _RemoveExclusionCommand = New RelayCommand(Of DataModel)(Sub(param) Me.RemoveExclusion(param), Function(param) Me.CanRemoveExclusion(param))
				End If
				Return _RemoveExclusionCommand
			End Get
		End Property

		Private Sub RemoveExclusion(ByVal param As DataModel)
			ExcludedData.Remove(param)
			OnPropertyChanged("ExcludedData")
		End Sub

		Private Function CanRemoveExclusion(ByVal param As DataModel) As Boolean
			Return param IsNot Nothing AndAlso Data.Contains(param) AndAlso ExcludedData.Contains(param)
		End Function
	End Class
End Namespace
