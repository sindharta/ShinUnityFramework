using NUnit.Framework;
using UnityEngine;

namespace Shin.Framework.EditorTests {

    public class GameBehaviourEditorTests {
        [Test]
        public void TransformTest() {
            GameObject go= new GameObject("TransformTest");
            GameBehaviour t = go.AddComponent<GameBehaviour>();
            Assert.IsNotNull(t.GetTransform());
            ObjectUtility.Destroy(go);
        }

//----------------------------------------------------------------------------------------------------------------------
    }
} //end namespace
