Public Class NumberHelper
    Public Shared Function GenerateRandomNumber(n As Integer) As Integer
        If n <= 0 Then
            Throw New ArgumentOutOfRangeException("n", "Solo se aceptan números positivos en este número aleatorio.")
        End If

        ' Calculate the lower and upper bounds for the random number
        Dim lowerBound As Integer = 10 ^ (n - 1)
        Dim upperBound As Integer = (10 ^ n) - 1

        ' Use the Rnd function to generate a random number within the bounds
        Dim randomNumber As Integer = Int(Rnd() * (upperBound - lowerBound + 1)) + lowerBound

        Return randomNumber
    End Function

End Class