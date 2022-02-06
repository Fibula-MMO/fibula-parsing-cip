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
    using Fibula.Utilities.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="CipFileParser"/> class.
    /// </summary>
    [TestClass]
    public class CipFileParserTests
    {
        private const string GoodMonsterFileName = "monster_example.mon";

        /// <summary>
        /// Checks <see cref="CipFileParser"/> initialization.
        /// </summary>
        [TestMethod]
        [DeploymentItem($"Fixtures/{GoodMonsterFileName}")]
        public void TestMonsterParsing()
        {
            string pathToMonsterFile = Path.Combine("Fixtures", GoodMonsterFileName);
            var monsterFileInfo = new FileInfo(pathToMonsterFile);

            var parsedModel = CipFileParser.ParseMonsterFile(monsterFileInfo);

            Assert.AreEqual("warlock", parsedModel.Name);
            Assert.AreEqual("a", parsedModel.Article);
            Assert.AreEqual(40, parsedModel.Attack);
            Assert.AreEqual(50, parsedModel.Defense);
            Assert.AreEqual(32, parsedModel.Armor);
            Assert.AreEqual(50, parsedModel.LoseTarget);
            Assert.AreEqual(100, parsedModel.Strategy.Closest);
            Assert.AreEqual(0, parsedModel.Strategy.Weakest);
            Assert.AreEqual(0, parsedModel.Strategy.Strongest);
            Assert.AreEqual(0, parsedModel.Strategy.Random);

            Assert.AreEqual(3200u, parsedModel.Skills.Single(s => s.Name == "HitPoints").DefaultLevel);
            Assert.AreEqual(75u, parsedModel.Skills.Single(s => s.Name == "GoStrength").DefaultLevel);
            Assert.AreEqual(900u, parsedModel.Skills.Single(s => s.Name == "CarryStrength").DefaultLevel);
        }
    }
}
