using EEA.Services.SaveServices;
using NUnit.Framework;

namespace EEA.Test
{
    public class SaveServiceTests
    {
        private SaveService _saveService;

        [SetUp]
        public void Setup()
        {
            var _saveHandler = new EncryptedSaveHandler();
            _saveService = new SaveService(_saveHandler);
        }

        [Test]
        public void SaveData_StoresValueCorrectly()
        {
            _saveService.Raw.Save("playerName", "player");

            var result = _saveService.Raw.LoadData("playerName");

            Assert.AreEqual("player", result);
        }

        [Test]
        public void LoadData_ReturnsDefault_WhenKeyNotFound()
        {
            var result = _saveService.Raw.LoadData("missingKey");

            Assert.AreEqual("", result);
        }

        [Test]
        public void DeleteData_RemovesValue()
        {
            _saveService.Raw.Save("score", "100");
            _saveService.Raw.Save("score", "0");

            var result = _saveService.Raw.LoadInt("score");

            Assert.AreEqual(0, result);
        }
    }
}