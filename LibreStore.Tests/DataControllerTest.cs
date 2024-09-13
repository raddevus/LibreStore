using System;
using LibreStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LibreStore.Tests;

public class DataControllerTest
{
    [Fact]
    public void CreateDataController(){

        //Discovered how to create Configuration on the fly
        // https://stackoverflow.com/questions/55497800/populate-iconfiguration-for-unit-tests/55497919#55497919
        
        var myConfiguration = new Dictionary<string, string>
        {
            {"dbType", "sqlite"},
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();


        DataController dc = new DataController(configuration);
        String [] allLines = System.IO.File.ReadAllLines(@"pwd.txt");
         ActionResult ar = dc.GetAllTokens(allLines[0]);
        // ActionResult ar = dc.GetAllTokens("wrongo!");
        var tempResult = ar as JsonResult;
        Console.WriteLine(tempResult.Value);
        // Assert.Equal(5, 10);
    }
}
