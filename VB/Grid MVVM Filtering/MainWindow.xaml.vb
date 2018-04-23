Imports Microsoft.VisualBasic
Imports Grid_MVVM_Filtering.ViewModel
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows

Namespace Grid_MVVM_Filtering
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
			DataContext = New DataViewModel(10)
		End Sub
	End Class
End Namespace