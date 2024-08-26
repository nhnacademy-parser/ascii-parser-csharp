using DocumentParser.Domain;
using DocumentParser.Elements;
using DocumentParser.Visitors;
using JetBrains.Annotations;

namespace DocumentParserTest.Domains;

[TestSubject(typeof(Cartridge<>))]
public class CartridgeTest
{
    [Fact]
    public void TryRemove_EmptyCartridge()
    {
        Cartridge<object> cartridge = new();

        Assert.Throws<InvalidOperationException>(() => cartridge.TryRemove());
    }


    [Fact]
    public void TryAdd_AlreadyExists()
    {
        Cartridge<object> cartridge = new();

        cartridge.TryAdd(new { });
        Assert.Throws<InvalidOperationException>(() => cartridge.TryAdd(new { }));
    }


    [Fact]
    public void TryAdd()
    {
        Cartridge<object> cartridge = new();

        cartridge.TryAdd(new { });

        Assert.True(cartridge.IsFilled);
    }


    [Fact]
    public void TryRemove()
    {
        Cartridge<object> cartridge = new();

        cartridge.TryAdd(new { });

        Assert.NotNull(cartridge.TryRemove());
    }
}