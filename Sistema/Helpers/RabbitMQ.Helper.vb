Imports RabbitMQ.Client
Imports System.Text
Imports Newtonsoft.Json
Public Class RabbitMQHelper

    Public Shared Sub PublishToRabbitMQ(messageType As String, collectorId As String, userId As String, queueName As String)
        Dim factory As New ConnectionFactory() With {
            .HostName = "192.168.20.111", ' Change to your RabbitMQ host
            .UserName = "guest",     ' Change to your RabbitMQ username
            .Password = "Eterno.2020"      ' Change to your RabbitMQ password
        }

        Using connection As IConnection = factory.CreateConnection()
            Using channel As IModel = connection.CreateModel()
                channel.QueueDeclare(queue:=queueName, durable:=True, exclusive:=False, autoDelete:=False, arguments:=Nothing)

                Dim message = New With {
                    .Type = messageType,
                    .CollectorId = collectorId,
                    .UserId = userId,
                    .Timestamp = DateTime.Now()
                }

                Dim messageBody As String = JsonConvert.SerializeObject(message)
                Dim body As Byte() = Encoding.UTF8.GetBytes(messageBody)

                Dim properties = channel.CreateBasicProperties()
                properties.Persistent = True

                channel.BasicPublish(exchange:="", routingKey:=queueName, basicProperties:=properties, body:=body)
            End Using
        End Using
    End Sub
End Class
