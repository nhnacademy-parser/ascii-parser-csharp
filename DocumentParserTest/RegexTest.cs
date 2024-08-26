using System.Text.RegularExpressions;

namespace DocumentParserTest;

public class RegexTest
{
    [Fact]
    public void RegexTest1()
    {
        Regex regex = new Regex("/{4,}$");

        string threeTime = "///";
        string fourTime = "////";
        string fiveTime = "/////";
        string additionChar = "/////asdad";
        
        
        Assert.DoesNotMatch(regex, threeTime);
        Assert.Matches(regex, fourTime);
        Assert.Matches(regex, fiveTime);
        Assert.DoesNotMatch(regex, additionChar);
    }
    
    [Fact]
    public void RegexTest2()
    {
        Regex regex = new Regex(":={3,}$");

        string twoTime = ":==";
        string threeTime = ":====";
        string additionChar = ":===asdad";
        
        Assert.DoesNotMatch(regex, twoTime);
        Assert.Matches(regex, threeTime);
        Assert.DoesNotMatch(regex, additionChar);
    }

    [Fact]
    void RegexTest3()
    {
        Regex regex = new Regex("\\*{4,}\n.*|^[^\n]*$");

        string s =
            "****\nThis is content in a sidebar block.\n\nimage::name.png[]\n\nThis is more content in the sidebar block.";
        
        Assert.Matches(regex, s);
    }
}

//