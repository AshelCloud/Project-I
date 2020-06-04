using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MonsterTest
    {
        [UnityTest]
        public IEnumerator MonsterDataTableParsingCheck()
        {
            var json = JsonManager.LoadJson<Serialization<string, Monster.MonsterData>>(Application.dataPath + "/Resources/MonsterJsons/", "MonsterTable").ToDictionary();

            Assert.AreNotEqual(null, json);
            yield return null;
        }

        [UnityTest]
        public IEnumerator MonsterFiniteStateMachineTest()
        {
            yield return null;
        }

        [UnityTest]
        public IEnumerator MonsterAttackTest()
        {
            yield return null;
        }

        [UnityTest]
        public IEnumerator MonsterHitCheck()
        {
            yield return null;
        }

        [UnityTest]
        public IEnumerator MonsterDieCheck()
        {
            GreyWolf wolf = Object.Instantiate(Resources.Load<GreyWolf>("Prefabs/Grey_wolf"));
            yield return null;

            while (wolf.HP > 0)
            {
                wolf.Hit(10);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            Assert.IsTrue(wolf.Dead && wolf == null);
        }
    }
}
