using DocumentParser.Domain;
using DocumentParser.Elements.Implementations;
using JetBrains.Annotations;

namespace DocumentParserTest.Domain;

[TestSubject(typeof(Cartridge<int>))]
public class CartridgeTest
{

    [Fact]
    public void TryRemove_EmptyCartridge()
    {
        Cartridge<DocsElement> cartridge = new();
        
        Assert.Throws<InvalidOperationException>(() => cartridge.TryRemove());
    }


    [Fact]
    public void TryAdd_AlreadyExists()
    {
        Cartridge<DocsElement> cartridge = new();
        
        cartridge.TryAdd(new DocsElement());
        Assert.Throws<InvalidOperationException>(() => cartridge.TryAdd(new DocsElement()));
    }


    [Fact]
    public void TryAdd()
    {
        Cartridge<DocsElement> cartridge = new();

        cartridge.TryAdd(new DocsElement());
        
        Assert.True(cartridge.IsFilled);
    }
    
    

    [Fact]
    public void TryRemove()
    {
        Cartridge<DocsElement> cartridge = new();

        cartridge.TryAdd(new DocsElement());
        
        Assert.NotNull(cartridge.TryRemove());
    }
}