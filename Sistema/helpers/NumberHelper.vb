Public Class NumberHelper
    Public Shared Function GenerateRandomNumber(n As Integer) As Integer
        ' Input validation
        If n <= 0 Then
            Throw New ArgumentOutOfRangeException("n", "Number of digits must be positive.")
        End If

        ' Create a new Random object for better isolation
        Dim randomizer As New Random()

        ' Calculate the lower and upper bounds for the random number
        Dim lowerBound As Integer = 10 ^ (n - 1)
        Dim upperBound As Integer = (10 ^ n) - 1

        ' Generate a random number within the bounds using Next
        Dim randomNumber As Integer = randomizer.Next(lowerBound, upperBound + 1)

        ' Return the random number
        Return randomNumber
    End Function


End Class