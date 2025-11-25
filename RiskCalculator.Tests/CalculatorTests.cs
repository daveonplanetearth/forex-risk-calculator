namespace RiskCalculator.Tests;

public class CalculatorTests
{
    [Fact]
    public void CalcRisk_WithUsdPair_ReturnsExpectedRisk()
    {
        var calc = new Calculator();
        double risk = calc.CalcRisk("GBP", "EURUSD", 10, 10000);
        Assert.Equal(8.63, risk);
    }

    [Fact]
    public void CalcRisk_WithJpyPair_ReturnsExpectedRisk()
    {
        var calc = new Calculator();
        double risk = calc.CalcRisk("GBP", "USDJPY", 10, 10000);
        Assert.Equal(6.0, risk);
    }

    [Fact]
    public void CalcRisk_WithSameAccountAndQuoteCurrency_ReturnsRiskWithoutConversion()
    {
        var calc = new Calculator();
        double risk = calc.CalcRisk("USD", "EURUSD", 10, 10000);
        Assert.Equal(10.0, risk);
    }
}
