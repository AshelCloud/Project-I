using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MonsterJson
    {
        [UnityTest]
        public IEnumerator MonsterDataTableParsingCheck()
        {
            var json = JsonManager.LoadJson<Serialization<string, Monster.MonsterData>>(Application.dataPath + "/Resources/MonsterJsons/", "MonsterTable").ToDictionary();

            Assert.AreNotEqual(null, json);
            yield return null;
        }

        [UnityTest]
        public IEnumerator MonsterHitCheck()
        {
            GreyWolf wolf = Object.Instantiate(Resources.Load<GreyWolf>("Prefabs/Grey_wolf"));

            float hp = wolf.HP;

            wolf.Hit(10);

            yield return null;

            Assert.AreEqual(hp - 10, wolf.HP);

        }
    }
}
