﻿using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.defrobo.cryptopals.tests
{
    [TestFixture]
    public class Set1
    {
        [Test]
        public void Challenge1()
        {
            string result = Crypto.HexStringToBase64("49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d");
            Assert.AreEqual("SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t", result);
        }

        [Test]
        public void Challenge2()
        {
            byte[] left = Crypto.HexStringToByteArray("1c0111001f010100061a024b53535009181c");
            byte[] right = Crypto.HexStringToByteArray("686974207468652062756c6c277320657965");
            var result = Crypto.FixedXOR(left, right);
            Assert.AreEqual("the kid don't play", Encoding.UTF8.GetString(result));
        }

        [Test]
        public void Challenge3()
        {
            var candidates = Crypto.BuildXORCipherRangeForScoring(
                Crypto.HexStringToByteArray(
                    "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736"));
            var bestScore = Crypto.ScoreCryptograms(candidates.Values);
            Assert.AreEqual("Cooking MC's like a pound of bacon", bestScore);
        }

        [Test]
        public void Challenge4()
        {
            var lines = File.ReadAllLines(TestContext.CurrentContext.TestDirectory + "\\resources\\4.txt");
            var linesAsBytes = lines.Select(s => Crypto.HexStringToByteArray(s)).ToList();
            var solves = new ConcurrentStack<byte[]>();
            Parallel.ForEach(linesAsBytes, line =>
            {
                Crypto.BuildXORCipherRangeForScoring(line);
                solves.PushRange(Crypto.BuildXORCipherRangeForScoring(line).Values.ToArray());
            });
            var result = Crypto.ScoreCryptograms(solves);
            Assert.AreEqual("Now that the party is jumping\n", Encoding.UTF8.GetString(result));
        }

        [Test]
        public void Challenge5()
        {
            var input = "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal";
            var result = Crypto.EncryptRepeatingKeyXOR(Encoding.UTF8.GetBytes("ICE"), Encoding.UTF8.GetBytes(input));
            Assert.AreEqual("0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f", Crypto.PrettyPrintHex(result));
        }

        [Test]
        public void Challenge6()
        {
            var text = Convert.FromBase64String(File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\resources\\6.txt"));
            var result = Crypto.BreakRepeatingKeyXOR(text);
            Assert.IsTrue(Encoding.UTF8.GetString(result).Contains("Play that funky music"));
        }

        [Test]
        public void Challenge7()
        {
            var encrypted = Convert.FromBase64String(File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\resources\\7.txt"));
            var output = Encoding.UTF8.GetString(AES128.DecryptECB(encrypted, Encoding.UTF8.GetBytes("YELLOW SUBMARINE")));
            Assert.IsTrue(output.StartsWith("I'm back and I'm ringin' the bell"));
        }

        [Test]
        public void Challenge8()
        {
            var lines = File.ReadAllLines(TestContext.CurrentContext.TestDirectory + "\\resources\\8.txt");
            var result = Crypto.DetectAESInECBMode(lines);
            Assert.AreEqual("d880619740a8a19b7840a8a31c810a3d08649af70dc06f4fd5d2d69c744cd283e2dd052f6b641dbf9d11b0348542bb5708649af70dc06f4fd5d2d69c744cd2839475c9dfdbc1d46597949d9c7e82bf5a08649af70dc06f4fd5d2d69c744cd28397a93eab8d6aecd566489154789a6b0308649af70dc06f4fd5d2d69c744cd283d403180c98c8f6db1f2a3f9c4040deb0ab51b29933f2c123c58386b06fba186a", result);
        }
    }
}
