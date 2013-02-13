Imports OPCHDAAutomation

Module Module1

    Dim WithEvents AnOPCHDAServer As OPCHDAServer
    
    Sub Main()
        Dim Item As OPCHDAItem
        Dim History As OPCHDAHistory
        Dim Value As OPCHDAValue
        Dim Message As String
        Dim DateFrom As Object
        Dim DateTo As Object

        AnOPCHDAServer = New OPCHDAServer
        AnOPCHDAServer.UseUTC = True
        AnOPCHDAServer.Connect("Matrikon.OPC.Simulation.1")
        Item = AnOPCHDAServer.OPCHDAItems.AddItem("Bucket Brigade.ArrayOfReal8", 1)

        DateFrom = Convert.ToDateTime("07.07.1676 11:41:00 PM")
        DateTo = Convert.ToDateTime("09.04.2012 12:34:42")

        History = Item.ReadRaw(DateFrom, DateTo)
        History.UseUTC = True

        For Each Value In History
            Message = Value.TimeStamp.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss") & " - " & Value.DataValue.ToString() & " - " & Value.Quality.ToString()
            Console.WriteLine(Message)
        Next Value
        Console.ReadKey()
        AnOPCHDAServer.Disconnect()
    End Sub

End Module
