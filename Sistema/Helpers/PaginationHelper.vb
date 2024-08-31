Public Class PaginationHelper
    Public Function GetLimitedPageNumbers(ByVal totalItems As Integer, ByVal itemsPerPage As Integer, ByVal currentPage As Integer, ByVal range As Integer) As List(Of Object)
        Dim totalPages As Integer = Math.Ceiling(totalItems / itemsPerPage)
        Dim startPage As Integer = Math.Max(currentPage - range, 1)
        Dim endPage As Integer = Math.Min(currentPage + range, totalPages)

        Dim pageNumbers As New List(Of Object)

        For i As Integer = startPage To endPage
            pageNumbers.Add(i)
        Next

        If startPage > 2 Then
            pageNumbers.Insert(0, "...")
        End If

        If endPage < totalPages - 1 Then
            pageNumbers.Add("...")
        End If

        Return pageNumbers
    End Function
    Public Function IsActivePage(dataItem As Object, pageNumber As Double) As String
        Dim parsedNumber As Double
        If Double.TryParse(Convert.ToString(dataItem), parsedNumber) AndAlso parsedNumber = pageNumber Then
            Return "active"
        Else
            Return String.Empty
        End If
    End Function

End Class
