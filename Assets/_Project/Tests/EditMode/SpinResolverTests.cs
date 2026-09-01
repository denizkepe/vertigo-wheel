using System.Collections.Generic;
using NUnit.Framework;

public class SpinResolverTests
{
    [Test]
    public void ZeroWeightSlice_IsNeverSelected()
    {
        
        var slices = new List<WheelSlice>
        {
            new WheelSlice { Weight = 1f },
            new WheelSlice { Weight = 0f }
        };
        var resolver = new SpinResolver(new System.Random(12345));

       
        for (int i = 0; i < 1000; i++)
        {
            int index = resolver.ResolveSliceIndex(slices);
            Assert.AreNotEqual(1, index);
        }
    }

    [Test]
    public void HigherWeight_IsSelectedMoreOften()
    {
        
        var slices = new List<WheelSlice>
        {
            new WheelSlice { Weight = 9f },
            new WheelSlice { Weight = 1f }
        };
        var resolver = new SpinResolver(new System.Random(42));

        int countZero = 0;
        for (int i = 0; i < 1000; i++)
        {
            if (resolver.ResolveSliceIndex(slices) == 0) countZero++;
        }

        Assert.Greater(countZero, 800); 
    }
}