using System;

namespace LiquidRechner
{
    public class LiquidRecipe
    {
        // Eingaben
        public double TargetAmount { get; set; } = 100;
        public double TargetNicotine { get; set; } = 8;
        public double AromaPercentage { get; set; } = 3;

        // Basis 1 (z.B. 70/30)
        public double Base1VgRatio { get; set; } = 70;

        // Basis 2 (z.B. reines PG)
        public double Base2VgRatio { get; set; } = 0; // 0 VG = 100% PG

        // Feste Werte (könnten auch Inputs sein)
        public double NicShotStrength { get; set; } = 20;
        public double NicShotVgRatio { get; set; } = 70; // Ihr aktueller Shot
        public double TargetVgRatio { get; set; } = 50;  // Ziel Liquid

        // Ergebnisse
        public double VolNicShot { get; private set; }
        public double VolAroma { get; private set; }
        public double VolBase1 { get; private set; }
        public double VolBase2 { get; private set; }
        public bool IsPossible { get; private set; } = true;
        public string ErrorMessage { get; private set; } = string.Empty;

        public void Calculate()
        {
            ErrorMessage = "";
            IsPossible = true;

            // 1. Aroma (angenommen 100% PG)
            VolAroma = TargetAmount * (AromaPercentage / 100.0);
            double aromaVg = 0;

            // 2. Nikotin
            // Benötigtes Nikotin absolut in mg
            double totalNicNeeded = TargetAmount * TargetNicotine;
            VolNicShot = totalNicNeeded / NicShotStrength;

            double nicShotVg = VolNicShot * (NicShotVgRatio / 100.0);

            // 3. Restmenge für Basen
            double remainingVol = TargetAmount - VolAroma - VolNicShot;

            if (remainingVol < 0)
            {
                IsPossible = false;
                ErrorMessage = "Nikotinanteil/Aroma zu hoch für Zielmenge.";
                return;
            }

            // 4. Ziel-VG berechnen
            double totalTargetVg = TargetAmount * (TargetVgRatio / 100.0);
            double currentVg = aromaVg + nicShotVg;
            double missingVg = totalTargetVg - currentVg;

            // 5. Lineares Gleichungssystem lösen für Base 1 und Base 2
            // V1 + V2 = remainingVol
            // V1 * b1_vg + V2 * b2_vg = missingVg

            double b1 = Base1VgRatio / 100.0;
            double b2 = Base2VgRatio / 100.0;

            // Division durch Null verhindern (wenn beide Basen gleiches Verhältnis haben)
            if (Math.Abs(b1 - b2) < 0.001)
            {
                // Sonderfall: Einfaches Auffüllen, wenn VG-Ratio zufällig passt
                VolBase1 = remainingVol;
                VolBase2 = 0;
                // Check ob das math. aufgeht, sonst Fehler
            }
            else
            {
                // Formel umgestellt nach VolBase1
                VolBase1 = (missingVg - (remainingVol * b2)) / (b1 - b2);
                VolBase2 = remainingVol - VolBase1;
            }

            // Plausibilitätscheck
            if (VolBase1 < -0.01 || VolBase2 < -0.01)
            {
                IsPossible = false;
                ErrorMessage = "Mischungsverhältnis mit diesen Basen nicht erreichbar.";
            }
        }
    }
}