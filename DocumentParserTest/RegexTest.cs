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


    [Fact]
    public void RegexTest4()
    {
        Regex regex = new Regex("(\\*{4,})+$|(\n.*)");

        string s =
            "****\nThis is content in a sidebar block.\n\nimage::name.png[]\n\nThis is more content in the sidebar block.";

        Assert.Matches(regex, s);
        Assert.Matches(regex, "****");
    }

    [Fact]
    public void RegexTest5()
    {
        Regex regex = new Regex("(\\.{4,})+(\n.*|^[^\n]*$)");

        string s =
            ".Red Pill";

        Assert.DoesNotMatch(regex, s);
    }


    [Fact]
    public void RegexTest6()
    {
        Regex regex = new Regex("^\\[+(.*)+]");

        string s = "[%collapsible]";

        Assert.Matches(regex, s);
    }

    [Fact]
    public void RegexTest7()
    {
        Regex regex = new Regex("(\\.{1,})+ (.*)");
        Regex regex2 = new Regex("(\\*{4,})+(\n.*|^[^\n]*$)");

        string s = "... level 3 ordered List";

        Assert.Equal(3, regex.Match(s).Groups[1].Length);
        Assert.Equal("level 3 ordered List", regex.Match(s).Groups[2].Value);
        Assert.Matches(regex, s);
        Assert.DoesNotMatch(regex2, s);
    }

    [Fact]
    public void RegexTest8()
    {
        // Regex regex = new Regex(@"^image::([^\[\]]+)(?:\[(.*?)\])?$");
        Regex regex = new Regex("^image::([^\\[]+)(?:\\[(.*)?\\])?$");
        

        string s = "image::image-file-name.png[I am the image alt text.]";
        string c = "image::image-file-name.png[]";
        string u = "image::image-file-name.png[uncloseed";

        GroupCollection groups = regex.Match(s).Groups;

        Assert.Matches(regex, s);
        Assert.Matches(regex, c);
        Assert.DoesNotMatch(regex, u);
        Assert.Equal("I am the image alt text.", groups[2].Value);
    }

    [Fact]
    public void RegexTest9()
    {
        Regex regex = new Regex("(\\S+://.+)(?:\\[(.+)\\])");

        string input =
            "This is a link to the https://docs.asciidoctor.org/home/[Asciidoctor documentation].\n";
        
        GroupCollection groups = regex.Match(input).Groups;
        
        Assert.Matches(regex, input);
    }
}

//