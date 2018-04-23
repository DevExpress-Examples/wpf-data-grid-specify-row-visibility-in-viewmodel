Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Input

Namespace Grid_MVVM_Filtering
	Public Class RelayCommand(Of T)
		Implements ICommand
		Private ReadOnly execute_Renamed As Action(Of T)
		Private ReadOnly canExecute_Renamed As Predicate(Of T)

		Public Sub New(ByVal execute As Action(Of T))
			Me.New(execute, Nothing)
		End Sub

		Public Sub New(ByVal execute As Action(Of T), ByVal canExecute As Predicate(Of T))
			If execute Is Nothing Then
				Throw New ArgumentNullException("execute")
			End If
			Me.execute_Renamed = execute
			Me.canExecute_Renamed = canExecute
		End Sub

		Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
			AddHandler(ByVal value As EventHandler)
				If canExecute_Renamed IsNot Nothing Then
					AddHandler CommandManager.RequerySuggested, value
				End If
			End AddHandler
			RemoveHandler(ByVal value As EventHandler)
				If canExecute_Renamed IsNot Nothing Then
					RemoveHandler CommandManager.RequerySuggested, value
				End If
			End RemoveHandler
			RaiseEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)
			End RaiseEvent
		End Event

		Public Sub RaiseCanExecuteChanged()
			CommandManager.InvalidateRequerySuggested()
		End Sub

		Public Function CanExecute(ByVal parameter As Object) As Boolean Implements ICommand.CanExecute
			Return canExecute_Renamed Is Nothing OrElse canExecute_Renamed(CType(parameter, T))
		End Function

		Public Sub Execute(ByVal parameter As Object) Implements ICommand.Execute
			execute_Renamed(CType(parameter, T))
		End Sub
	End Class
End Namespace
