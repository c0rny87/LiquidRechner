using LiquidRechner;
using Xunit;

namespace LiquidRechner.Tests
{
    public class LiquidRecipeTests
    {
        [Fact]
        public void Calculate_WithDefaultValues_ComputesExpectedVolumes()
        {
            var recipe = new LiquidRecipe();
            recipe.Calculate();

            Assert.True(recipe.IsPossible);
            Assert.Equal(40.0, recipe.VolNicShot, 3);
            Assert.Equal(3.0, recipe.VolAroma, 3);
            Assert.Equal(31.428, recipe.VolBase1, 3);
            Assert.Equal(25.571, recipe.VolBase2, 3);
        }

        [Fact]
        public void Calculate_WithTooHighNicotine_MarksRecipeAsImpossible()
        {
            var recipe = new LiquidRecipe
            {
                TargetAmount = 10,
                TargetNicotine = 100,
                AromaPercentage = 0
            };

            recipe.Calculate();

            Assert.False(recipe.IsPossible);
            Assert.Equal("Nikotinanteil/Aroma zu hoch für Zielmenge.", recipe.ErrorMessage);
        }

        [Fact]
        public void Calculate_WithMatchingBaseRatios_UsesSingleBaseSource()
        {
            var recipe = new LiquidRecipe
            {
                Base1VgRatio = 50,
                Base2VgRatio = 50,
                TargetNicotine = 5,
                AromaPercentage = 5
            };

            recipe.Calculate();

            Assert.True(recipe.IsPossible);
            Assert.Equal(0, recipe.VolBase2, 3);
            Assert.Equal(recipe.TargetAmount - recipe.VolNicShot - recipe.VolAroma, recipe.VolBase1, 6);
        }
    }
}
