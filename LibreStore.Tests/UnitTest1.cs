using System;
using Xunit;

namespace LibreStore.Tests;

public class UnitTest1
{
    // [Fact]
    // public void Test1()
    // {

    // }

    [Fact]
    public void CreateMainKey(){
        Models.MainToken mt = new Models.MainToken("testkey");
        Xunit.Assert.True( mt.Key == "testkey");
        
        // Xunit.Assert.Equal(mt.Created.ToString("HH:mm:ss.fff"), DateTime.Now.ToString("HH:mm:ss.fff"));

    }
}