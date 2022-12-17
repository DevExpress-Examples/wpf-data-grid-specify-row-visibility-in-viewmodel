Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.DataAnnotations
Imports DevExpress.Mvvm.Xpf
Imports System.Collections.ObjectModel
Imports System.Linq

Namespace CustomFilteringMVVM

    Public Class DataModel

        Public Property Text As String

        Public Property Number As Integer
    End Class

    Public Class MainViewModel
        Inherits ViewModelBase

        Public Property Data As ObservableCollection(Of DataModel)

        Public Property ExcludedData As ObservableCollection(Of DataModel)

        Public Property CurrentVisibleItem As DataModel

        Public Property CurrentExcludedItem As DataModel

        Public Sub New()
            Data = New ObservableCollection(Of DataModel)(Enumerable.Range(0, 10).[Select](Function(i) New DataModel() With {.Text = $"Row {i}", .Number = i}))
            ExcludedData = New ObservableCollection(Of DataModel)()
        End Sub

        <Command>
        Public Sub AddExclusion()
            ExcludedData.Add(CurrentVisibleItem)
        End Sub

        Public Function CanAddExclusion() As Boolean
            Return CurrentVisibleItem IsNot Nothing AndAlso Not ExcludedData.Contains(CurrentVisibleItem)
        End Function

        <Command>
        Public Sub RemoveExclusion()
            ExcludedData.Remove(CurrentExcludedItem)
        End Sub

        Public Function CanRemoveExclusion() As Boolean
            Return CurrentExcludedItem IsNot Nothing AndAlso ExcludedData.Contains(CurrentExcludedItem)
        End Function

        <Command>
        Public Sub FilterExclusions(ByVal args As RowFilterArgs)
            If ExcludedData.Contains(Data(args.SourceIndex)) Then
                args.Visible = False
            End If
        End Sub
    End Class
End Namespace
