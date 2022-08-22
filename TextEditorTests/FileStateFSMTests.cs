using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditorTests
{
    [TestClass]
    public class FileStateFSMTests
    {
        [TestMethod]
        public void TypicalUsage()
        {
            FileStateFSM stateMachine = new();
            
            Assert.AreEqual(stateMachine.State, FileStates.NoFile);
            
            stateMachine.EventHappened(FileEvents.FileChanged);

            Assert.AreEqual(stateMachine.State, FileStates.NoFile);

            stateMachine.EventHappened(FileEvents.FileSaved);

            Assert.AreEqual(stateMachine.State, FileStates.FileNoChanges);

            stateMachine.EventHappened(FileEvents.FileChanged);

            Assert.AreEqual(stateMachine.State, FileStates.ChangedFile);

            stateMachine.EventHappened(FileEvents.FileSaved);

            Assert.AreEqual(stateMachine.State, FileStates.FileNoChanges);

            stateMachine.EventHappened(FileEvents.FileOpened);

            Assert.AreEqual(stateMachine.State, FileStates.FileNoChanges);

            stateMachine.EventHappened(FileEvents.FileChanged);

            Assert.AreEqual(stateMachine.State, FileStates.ChangedFile);

            stateMachine.EventHappened(FileEvents.FileSaved);

            Assert.AreEqual(stateMachine.State, FileStates.FileNoChanges);
        }
    }
}
