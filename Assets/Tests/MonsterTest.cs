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
            //패트롤 / 이동 상태에서 공격범위 안으로 플레이어 오브젝트가 들어오면
            //Run -> Attack으로 전이
            //무슨 상태에서든지 피격당할 경우
            //AnyState -> Hit으로 전이
            //Ex) Run -> 피격 -> Hit -> Run
            //체력이 0 아래로 떨어질경우
            //AnyState -> Daed -> Destroy Object

            //일반몬스터는 생성했을때 기본적으로 Run(Idle) 상태
            GreyWolf wolf = Object.Instantiate(Resources.Load<GreyWolf>("Prefabs/Grey_wolf"));
            wolf.GetComponent<Rigidbody2D>().gravityScale = 0f;
            yield return null;

            Assert.AreEqual(Monster.MonsterBehaviour.Run, wolf.BehaviourStack.Peek());
            yield return null;

            //테스트용 Player
            GameObject  player = new GameObject();
            player.AddComponent<BoxCollider2D>(); //LayCasting을 위한 Collider
            player.tag = "Player";
            yield return null;

            //추격 상태 전이 테스트
            wolf.transform.position = Vector3.zero;
            player.transform.position = new Vector3(wolf.DetectionRange, 0f, 0f);
            yield return null;
            yield return null;

            Assert.AreEqual(Monster.MonsterBehaviour.Chase, wolf.BehaviourStack.Peek());
            yield return null;

            //공격 상태 전이 테스트
            wolf.transform.position = Vector3.zero;
            player.transform.position = new Vector3(wolf.AttackRange, 0f, 0f);
            yield return null; //늑대가 이동해서 상대를 찾는 프레임을 벌기위해 2번
            yield return null;

            Assert.AreEqual(Monster.MonsterBehaviour.Attack, wolf.BehaviourStack.Peek());
            yield return null;

            //공격 -> 추격 상태 전이 테스트
            wolf.transform.position = Vector3.zero;
            player.transform.position = new Vector3(wolf.DetectionRange, 0f, 0f);
            yield return null;
            yield return null;

            Assert.AreEqual(Monster.MonsterBehaviour.Chase, wolf.BehaviourStack.Peek());
            yield return null;

            //추격 -> 패트롤 상태 전이 테스트
            wolf.transform.position = Vector3.zero;
            player.transform.position = new Vector3(1000f, 1000f, 0f);
            Object.Destroy(player.gameObject);
            yield return null;

            Assert.AreEqual(Monster.MonsterBehaviour.Run, wolf.BehaviourStack.Peek());
            yield return null;

            //피격 상태 전이 테스트
            wolf.Hit(0.1f);
            yield return null;

            Assert.AreEqual(Monster.MonsterBehaviour.Hit, wolf.BehaviourStack.Peek());
            yield return null;

            yield return new WaitForSeconds(1f); //Hit Animation이 끝난뒤

            //피격 -> 패트롤 전이 테스트
            Assert.AreEqual(Monster.MonsterBehaviour.Run, wolf.BehaviourStack.Peek());
            yield return null;

            //사망 테스트
            wolf.Hit(10000f);
            yield return null;

            Assert.AreEqual(Monster.MonsterBehaviour.Dead, wolf.BehaviourStack.Peek());
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
