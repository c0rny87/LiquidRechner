using System.Reflection;
using Bunit;
using LiquidRechner.Components.Pages;
using Microsoft.AspNetCore.Components;

namespace LiquidRechner.Tests;

public sealed class HomeComponentTests : TestContext
{
    [Fact]
    public void OnInitialized_CalculatesRecipeDuringInitialRender()
    {
        var cut = RenderComponent<Home>();
        var recipe = cut.Instance.Recipe;

        var expectedNicVolume = (recipe.TargetAmount * recipe.TargetNicotine) / recipe.NicShotStrength;

        Assert.Equal(expectedNicVolume, recipe.VolNicShot, 5);
        Assert.True(recipe.IsPossible);
    }

    [Fact]
    public void Recalculate_RecomputesRecipeWhenInputsChange()
    {
        var cut = RenderComponent<Home>();
        var recipe = cut.Instance.Recipe;

        recipe.TargetAmount = 120;
        recipe.TargetNicotine = 5.5;
        recipe.AromaPercentage = 12;
        recipe.Base1VgRatio = 80;
        recipe.Base2VgRatio = 20;
        recipe.NicShotVgRatio = 55;

        InvokeVoidPrivateMethod(cut.Instance, "Recalculate");

        var expectedNicShotVolume = (recipe.TargetAmount * recipe.TargetNicotine) / recipe.NicShotStrength;
        var expectedRemaining = recipe.TargetAmount - recipe.VolNicShot - recipe.VolAroma;

        Assert.Equal(expectedNicShotVolume, recipe.VolNicShot, 6);
        Assert.True(recipe.IsPossible);
        Assert.Equal(expectedRemaining, recipe.VolBase1 + recipe.VolBase2, 6);
    }

    [Fact]
    public void GetWeight_UsesInterpolatedDensity()
    {
        var cut = RenderComponent<Home>();

        const double volume = 30d;
        const double vgRatio = 70d;

        var result = InvokePrivateFunction<double>(cut.Instance, "GetWeight", volume, vgRatio);

        var expectedDensity = (vgRatio / 100d * 1.261d) + ((100d - vgRatio) / 100d * 1.036d);
        var expectedWeight = volume * expectedDensity;

        Assert.Equal(expectedWeight, result, 6);
    }

    private static void InvokeVoidPrivateMethod(ComponentBase component, string methodName, params object?[] arguments)
    {
        var method = component.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Methode '{methodName}' nicht gefunden.");

        _ = method.Invoke(component, arguments);
    }

    private static TResult InvokePrivateFunction<TResult>(ComponentBase component, string methodName, params object?[] arguments)
    {
        var method = component.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Methode '{methodName}' nicht gefunden.");

        return (TResult)(method.Invoke(component, arguments) ?? throw new InvalidOperationException($"Methode '{methodName}' lieferte null."));
    }
}