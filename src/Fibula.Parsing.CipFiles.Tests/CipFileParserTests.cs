// -----------------------------------------------------------------
// <copyright file="CipFileParserTests.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Parsing.CipFiles.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using Fibula.Definitions.Enumerations;
    using Fibula.Parsing.CipFiles.Enumerations;
    using Fibula.Utilities.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="CipFileParser"/> class.
    /// </summary>
    [TestClass]
    public class CipFileParserTests
    {
        private const string GoodMonsterFileName = "monster_example.mon";
        private const string AnotherGoodMonsterFileName = "monster_example2.mon";

        /// <summary>
        /// Checks <see cref="CipFileParser"/> initialization.
        /// </summary>
        [TestMethod]
        [DeploymentItem($"Fixtures/{GoodMonsterFileName}")]
        [DeploymentItem($"Fixtures/{AnotherGoodMonsterFileName}")]
        public void TestParseMonsterFile()
        {
            const string ExpectedName = "warlock";
            const string ExpectedArticle = "a";
            const BloodType ExpectedBloodType = BloodType.Blood;
            const uint ExpectedExperience = 4000;
            const CipOutfitType ExpectedOutfitType = CipOutfitType.Outfit;
            const ushort ExpectedOutfitId = 130;
            const byte ExpectedHeadColor = 0;
            const byte ExpectedBodyColor = 52;
            const byte ExpectedLegsColor = 128;
            const byte ExpectedFeetColor = 95;
            const uint ExpectedCorpse = 4240;
            const ushort ExpectedSummonCost = 0;
            const ushort ExpectedFleeThreshold = 1000;
            const ushort ExpectedAttack = 40;
            const ushort ExpectedDefense = 50;
            const ushort ExpectedArmor = 32;
            const byte ExpectedLoseTarget = 50;
            const byte ExpectedStrategyClosest = 100;
            const byte ExpectedStrategyWeakest = 0;
            const byte ExpectedStrategyStrongest = 0;
            const byte ExpectedStrategyRandom = 0;
            const uint ExpectedHitpoints = 3200;
            const uint ExpectedSpeed = 75;
            const uint ExpectedCapacity = 900;
            const uint ExpectedFistSkill = 50;
            const int ExpectedSpellRulesCount = 10;
            const int ExpectedInventoryCount = 18;

            CipCreatureFlag[] expectedFlags = new CipCreatureFlag[]
            {
                CipCreatureFlag.KickBoxes,
                CipCreatureFlag.KickCreatures,
                CipCreatureFlag.SeeInvisible,
                CipCreatureFlag.Unpushable,
                CipCreatureFlag.DistanceFighting,
                CipCreatureFlag.NoSummon,
                CipCreatureFlag.NoIllusion,
                CipCreatureFlag.NoBurning,
                CipCreatureFlag.NoPoison,
                CipCreatureFlag.NoEnergy,
                CipCreatureFlag.NoParalyze,
            };

            string[] expectedPhrases = new string[]
            {
                "Learn the secret of our magic! YOUR death!",
                "Even a rat is a better mage than you.",
                "We don't like intruders!",
            };

            (ushort id, byte max, ushort chance)[] expectedInventory = new (ushort, byte, ushort)[]
            {
                (3567, 1, 20),
                (3600, 1, 110),
                (2917, 1, 150),
                (3590, 4, 200),
                (3007, 1, 10),
                (3728, 1, 30),
                (3051, 1, 30),
                (3031, 80, 300),
                (3360, 1, 3),
                (3509, 1, 130),
                (3062, 1, 25),
                (3299, 1, 100),
                (2852, 1, 4),
                (3006, 1, 2),
                (3324, 1, 70),
                (3029, 1, 14),
                (3081, 1, 5),
                (3034, 1, 11),
            };

            ((CipMonsterSpellCastType castType, long[] values), (CipMonsterSpellEffectType effectType, long[] values), byte chance)[] expectedSpellRules =
                new ((CipMonsterSpellCastType, long[]), (CipMonsterSpellEffectType, long[]), byte)[]
                {
                    ((CipMonsterSpellCastType.Actor, new long[] { 13 }), (CipMonsterSpellEffectType.Healing, new long[] { 80, 20 }), 4),
                    ((CipMonsterSpellCastType.Actor, new long[] { 13 }), (CipMonsterSpellEffectType.Outfit, new long[] { 20 }), 10),
                    ((CipMonsterSpellCastType.Victim, new long[] { 7, 5, 0 }), (CipMonsterSpellEffectType.Damage, new long[] { 1, 75, 30 }), 2),
                    ((CipMonsterSpellCastType.Victim, new long[] { 7, 0, 0 }), (CipMonsterSpellEffectType.Damage, new long[] { 512, 55, 20 }), 6),
                    ((CipMonsterSpellCastType.Victim, new long[] { 7, 0, 14 }), (CipMonsterSpellEffectType.Speed, new long[] { -80, 20, 40 }), 9),
                    ((CipMonsterSpellCastType.Origin, new long[] { 0, 13 }), (CipMonsterSpellEffectType.Summon, new long[] { 67, 1 }), 10),
                    ((CipMonsterSpellCastType.Destination, new long[] { 7, 4, 2, 7 }), (CipMonsterSpellEffectType.Damage, new long[] { 4, 130, 40 }), 3),
                    ((CipMonsterSpellCastType.Destination, new long[] { 7, 4, 0, 0 }), (CipMonsterSpellEffectType.Field, new long[] { 1 }), 7),
                    ((CipMonsterSpellCastType.Destination, new long[] { 7, 4, 1, 0 }), (CipMonsterSpellEffectType.Field, new long[] { 1 }), 5),
                    ((CipMonsterSpellCastType.Angle, new long[] { 0, 8, 12 }), (CipMonsterSpellEffectType.Damage, new long[] { 8, 175, 30 }), 8),
                };

            string pathToMonsterFile = Path.Combine("Fixtures", GoodMonsterFileName);
            var monsterFileInfo = new FileInfo(pathToMonsterFile);

            var parsedModel = CipFileParser.ParseMonsterFile(monsterFileInfo);

            Assert.AreEqual(ExpectedName, parsedModel.Name);
            Assert.AreEqual(ExpectedArticle, parsedModel.Article);
            Assert.AreEqual(ExpectedBloodType, parsedModel.BloodType);
            Assert.AreEqual(ExpectedExperience, parsedModel.Experience);
            Assert.AreEqual(ExpectedOutfitType, parsedModel.Outfit?.Type);
            Assert.AreEqual(ExpectedOutfitId, parsedModel.Outfit?.Id);
            Assert.AreEqual(ExpectedHeadColor, parsedModel.Outfit?.Head);
            Assert.AreEqual(ExpectedBodyColor, parsedModel.Outfit?.Body);
            Assert.AreEqual(ExpectedLegsColor, parsedModel.Outfit?.Legs);
            Assert.AreEqual(ExpectedFeetColor, parsedModel.Outfit?.Feet);
            Assert.AreEqual(ExpectedCorpse, parsedModel.Corpse);
            Assert.AreEqual(ExpectedSummonCost, parsedModel.SummonCost);
            Assert.AreEqual(ExpectedFleeThreshold, parsedModel.FleeThreshold);
            Assert.AreEqual(ExpectedAttack, parsedModel.Attack);
            Assert.AreEqual(ExpectedDefense, parsedModel.Defense);
            Assert.AreEqual(ExpectedArmor, parsedModel.Armor);
            Assert.AreEqual(ExpectedLoseTarget, parsedModel.LoseTarget);
            Assert.AreEqual(ExpectedStrategyClosest, parsedModel.Strategy.Closest);
            Assert.AreEqual(ExpectedStrategyWeakest, parsedModel.Strategy.Weakest);
            Assert.AreEqual(ExpectedStrategyStrongest, parsedModel.Strategy.Strongest);
            Assert.AreEqual(ExpectedStrategyRandom, parsedModel.Strategy.Random);

            Assert.AreEqual(ExpectedHitpoints, parsedModel.Skills.Single(s => s.Name == "HitPoints").DefaultLevel);
            Assert.AreEqual(ExpectedSpeed, parsedModel.Skills.Single(s => s.Name == "GoStrength").DefaultLevel);
            Assert.AreEqual(ExpectedCapacity, parsedModel.Skills.Single(s => s.Name == "CarryStrength").DefaultLevel);
            Assert.AreEqual(ExpectedFistSkill, parsedModel.Skills.Single(s => s.Name == "FistFighting").DefaultLevel);

            // Check spell rules
            Assert.AreEqual(ExpectedSpellRulesCount, parsedModel.Spells.Count);

            var i = 0;
            foreach (var spellRules in expectedSpellRules)
            {
                var (castType, castValues) = spellRules.Item1;
                var (effectType, effectValues) = spellRules.Item2;

                Assert.AreEqual(castType, parsedModel.Spells[i].condition.Type);
                CollectionAssert.AreEquivalent(castValues, parsedModel.Spells[i].condition.Values.ToArray());
                Assert.AreEqual(effectType, parsedModel.Spells[i].effect.Type);
                CollectionAssert.AreEquivalent(effectValues, parsedModel.Spells[i].effect.Values.ToArray());
                Assert.AreEqual(spellRules.chance, parsedModel.Spells[i].chance);

                i++;
            }

            // Check Inventory
            Assert.AreEqual(ExpectedInventoryCount, parsedModel.Inventory.Count);

            i = 0;
            foreach (var (id, max, chance) in expectedInventory)
            {
                Assert.AreEqual(id, parsedModel.Inventory[i].typeId);
                Assert.AreEqual(max, parsedModel.Inventory[i].maxAmount);
                Assert.AreEqual(chance, parsedModel.Inventory[i].dropChance);

                i++;
            }

            // Check flags
            foreach (var checkFlag in expectedFlags)
            {
                Assert.IsTrue(parsedModel.Flags.Contains(checkFlag));
            }

            // Check phrases
            foreach (var checkPhrase in expectedPhrases)
            {
                Assert.IsTrue(parsedModel.Phrases.Contains(checkPhrase), $"Phrase: {checkPhrase} is not included.");
            }
        }
    }
}
